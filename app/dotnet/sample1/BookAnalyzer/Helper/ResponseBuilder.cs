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

        // Attempt 2: Fallback for dynamic JSON keys (e.g. "Guidepost #1", "Guidepost #2")
        try
        {
            using var doc = JsonDocument.Parse(cleanedJson);
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

        return new BookAnalysisResult(Summary: string.Empty, CoreReview: string.Empty, KeyQuotes: new List<string>());
    }

}