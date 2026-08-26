using static BookAnalyzer.Helper.TextExtractor;
using static BookAnalyzer.Helper.ResponseBuilder;
using static BookAnalyzer.Helper.RequestBuilder;
using BookAnalyzer.Model;
using Microsoft.Extensions.AI;
using OllamaSharp;


namespace BookAnalyzer;

public class AnalyzerService
{
    private readonly IChatClient _chatClient;

    public AnalyzerService(string endpoint = "http://localhost:11434/", string modelId = "phi4-mini")
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(endpoint),
            Timeout = TimeSpan.FromMinutes(15)
        };

        _chatClient = new OllamaApiClient(httpClient, modelId);
    }

    public async Task<BookAnalysisResult?> AnalyzeAsync(string filePath)
    {

        string extractedText = ExtractTextFromPdf(filePath);

        if (string.IsNullOrWhiteSpace(extractedText))
            throw new InvalidOperationException("No readable text found.");

        List<ChatMessage> chatMessages = CreateRequest(extractedText);

        ChatOptions options = CreateRequestOptions();

        ChatResponse response = await _chatClient.GetResponseAsync(chatMessages, options);
        string rawText = response.Text ?? string.Empty;

        return ParseResponse(rawText);
    }


}