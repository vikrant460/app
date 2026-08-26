using System.Text.Json;
using BookAnalyzer.Model;

namespace BookAnalyzer.Helper;

public static class ResponseBuilder
{
    public static BookAnalysisResult ParseResponse(string responseJson)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<BookAnalysisResult>(
            responseJson,
            options)
            ?? new BookAnalysisResult(
                Summary: string.Empty,
                CoreReview: string.Empty,
                KeyQuotes: []);
    }

}