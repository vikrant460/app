using System.Text;
using System.Text.Json;
using Microsoft.Extensions.AI;
using OllamaSharp;
using UglyToad.PdfPig;

namespace BookAnalyzer;

/// <summary>
/// Strongly-typed output schema for LLM responses.
/// </summary>
public record BookAnalysisResult(
    string Summary,
    string CoreReview,
    List<string> KeyQuotes
);

public class BookAnalyzerService
{
    private readonly IChatClient _chatClient;

    public BookAnalyzerService(string endpoint = "http://localhost:11434/", string modelId = "phi4-mini")
    {
        var httpClient = new System.Net.Http.HttpClient
        {
            BaseAddress = new Uri(endpoint),
            Timeout = TimeSpan.FromMinutes(15)
        };

        _chatClient = new OllamaApiClient(httpClient, modelId);
    }

    public async Task<BookAnalysisResult?> AnalyzePdfAsync(string pdfPath)
    {
        if (!File.Exists(pdfPath))
            throw new FileNotFoundException($"PDF not found: {pdfPath}");

        string extractedText = ExtractTextFromPdf(pdfPath);
        if (string.IsNullOrWhiteSpace(extractedText))
            throw new InvalidOperationException("No readable text found.");

        // Limit input size to prevent memory context overflow
        if (extractedText.Length > 10000)
        {
            extractedText = extractedText.Substring(0, 10000);
        }


        string userPrompt = $"""
    You are helping a reader understand and appreciate a book.

    Analyze the following document text and return exactly these three sections:

    1. "Summary"
       Give a concise overview of the main ideas, themes, arguments, or events.
       Focus on what the author is trying to communicate rather than describing the structure of the document.

    2. "CoreReview"
       Explain the deepest or most important takeaway from the text.
       Focus on the author's underlying ideas, worldview, questions, tensions, or insights.
       Do not merely repeat the Summary.

    3. "KeyQuotes"
       Select up to 3 of the most meaningful passages from the ORIGINAL TEXT.

       Quote-selection criteria:
       - Prefer passages that are philosophical, profound, insightful, poetic, emotionally resonant, or beautifully expressed.
       - Prefer passages that reveal something important about human nature, life, society, relationships, meaning, mortality, knowledge, or the central theme of the book.
       - Choose passages that stand on their own and are interesting to reread.
       - Prefer a complete sentence or a short contiguous passage rather than an isolated fragment.
       - Do NOT choose quotes merely because they contain important factual information.
       - Do NOT rewrite, summarize, improve, combine, or paraphrase the quotes.
       - Every quote MUST appear verbatim in the provided document text.
       - Preserve the author's original wording and punctuation.
       - Do not join text from different parts of the document into one quote.
       - If the text contains no genuinely meaningful or beautiful passage, return fewer quotes rather than inventing one.

    IMPORTANT:
    The "KeyQuotes" section is for DIRECT QUOTATIONS ONLY.
    The "Summary" and "CoreReview" may be paraphrased, but "KeyQuotes" must use the author's exact words.

    Document Text:
    {extractedText}
    """;

        var chatMessages = new List<ChatMessage>
        {
            new(ChatRole.System, "You are a precise data extractor. You MUST format responses strictly as JSON with key names: 'Summary', 'CoreReview', and 'KeyQuotes'."),
            new(ChatRole.User, userPrompt)
        };

        var options = new ChatOptions
        {
            Temperature = 0.0f,
            ResponseFormat = ChatResponseFormat.Json
        };

        ChatResponse response = await _chatClient.GetResponseAsync(chatMessages, options);
        string rawText = response.Text ?? string.Empty;

        // Clean any Markdown code fences (```json ... ```)
        string cleanedJson = rawText.Trim();
        if (cleanedJson.StartsWith("```"))
        {
            int firstNewLine = cleanedJson.IndexOf('\n');
            int lastFence = cleanedJson.LastIndexOf("```");
            if (firstNewLine != -1 && lastFence > firstNewLine)
            {
                cleanedJson = cleanedJson.Substring(firstNewLine + 1, lastFence - firstNewLine - 1).Trim();
            }
        }

        return ParseResponse(cleanedJson, extractedText);
    }

    private static BookAnalysisResult ParseResponse(string jsonText, string originalText)
    {
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Attempt 1: Direct Deserialization to BookAnalysisResult
        try
        {
            var result = JsonSerializer.Deserialize<BookAnalysisResult>(jsonText, jsonOptions);
            if (result != null && (!string.IsNullOrEmpty(result.Summary) || !string.IsNullOrEmpty(result.CoreReview)))
            {
                var quotes = result.KeyQuotes ?? new List<string>();
                return result with { KeyQuotes = quotes };
            }
        }
        catch
        {
            // Fall through to manual parsing if standard deserialization fails
        }

        // Attempt 2: Fallback for dynamic JSON keys (e.g. "Guidepost #1", "Guidepost #2")
        try
        {
            using var doc = JsonDocument.Parse(jsonText);
            var root = doc.RootElement;

            var items = new List<string>();
            foreach (var prop in root.EnumerateObject())
            {
                items.Add($"{prop.Name}: {prop.Value.GetString()}");
            }

            if (items.Count > 0)
            {
                string summaryText = string.Join("\n", items.Take(5));
                string reviewText = string.Join("\n", items.Skip(5));

                return new BookAnalysisResult(
                    Summary: summaryText,
                    CoreReview: string.IsNullOrWhiteSpace(reviewText) ? summaryText : reviewText,
                    KeyQuotes: items.Take(3).ToList()
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Parsing Error]: {ex.Message}");
        }

        return null;
    }

    private static string ExtractTextFromPdf(string filePath)
    {
        var sb = new System.Text.StringBuilder();
        using (UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(filePath))
        {
            foreach (var page in document.GetPages())
            {
                sb.AppendLine(page.Text);
            }
        }
        return sb.ToString();
    }
}