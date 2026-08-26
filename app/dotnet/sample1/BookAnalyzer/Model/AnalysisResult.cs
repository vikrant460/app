
namespace BookAnalyzer.Model;
/// <summary>
/// Strongly-typed output schema for LLM responses.
/// </summary>
public record AnalysisResult(
    List<string> KeyQuotes
);