using static BookAnalyzer.Helper.TextExtractor;
using static BookAnalyzer.Helper.ResponseBuilder;
using BookAnalyzer.Model;
using Microsoft.Extensions.AI;
using OllamaSharp;
using BookAnalyzer.Helper;


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

        var extractedText = ExtractText(filePath);

        if (string.IsNullOrWhiteSpace(extractedText))
            throw new InvalidOperationException("No readable text found.");

        var request = new RequestBuilder(extractedText);

        var response = await _chatClient.GetResponseAsync(request.Messages, request.Options);

        return ParseResponse(response.Text ?? string.Empty);
    }


}