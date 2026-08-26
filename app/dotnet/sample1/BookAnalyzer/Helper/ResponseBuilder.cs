using System.Text.Json;
using BookAnalyzer.Model;

namespace BookAnalyzer.Helper;

public static class ResponseBuilder
{
    public static AnalysisResult ParseResponse(string responseJson)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<AnalysisResult>(
            responseJson,
            options)
            ?? new AnalysisResult(
                KeyQuotes: []);
    }

}