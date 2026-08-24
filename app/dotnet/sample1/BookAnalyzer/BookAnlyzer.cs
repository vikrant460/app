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
            Summarize the following document text into these specific structural components:
            1. "Summary": A concise overview of the main topics.
            2. "CoreReview": The primary analytical takeaway.
            3. "KeyQuotes": Up to 3 main points or exact phrases.

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