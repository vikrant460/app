using System.Text.Json;
using BookAnalyzer.Model;

namespace BookAnalyzer.Helper;

public static class ResponseBuilder
{
    public static BookAnalysisResult ParseResponse(string responseJson)
    {
                // Clean any Markdown code fences (```json ... ```)
        string cleanedJson = responseJson.Trim();
        if (cleanedJson.StartsWith("```"))
        {
            int firstNewLine = cleanedJson.IndexOf('\n');
            int lastFence = cleanedJson.LastIndexOf("```");
            if (firstNewLine != -1 && lastFence > firstNewLine)
            {
                cleanedJson = cleanedJson.Substring(firstNewLine + 1, lastFence - firstNewLine - 1).Trim();
            }
        }
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Attempt 1: Direct Deserialization to BookAnalysisResult
        try
        {
            var result = JsonSerializer.Deserialize<BookAnalysisResult>(cleanedJson, jsonOptions);
            if (result != null && (!string.IsNullOrEmpty(result.Summary) || !string.IsNullOrEmpty(result.CoreReview)))
            {
                var quotes = result.KeyQuotes ?? new List<string>();
                return result with { KeyQuotes = quotes };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Parsing Error]: {ex.Message}");
        }

        return new BookAnalysisResult(Summary: string.Empty, CoreReview: string.Empty, KeyQuotes: new List<string>());
    }

}