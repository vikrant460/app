
namespace BookAnalyzer.Model;
/// <summary>
/// Strongly-typed output schema for LLM responses.
/// </summary>
public record BookAnalysisResult(
    string Summary,
    string CoreReview,
    List<string> KeyQuotes
);