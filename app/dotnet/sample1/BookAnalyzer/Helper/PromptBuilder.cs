namespace BookAnalyzer.Helper;
public static class PromptBuilder
{
    public static string BuildPrompt(string extractedText)
    {
        return $"""
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
           Provide them in their original form without paraphrasing.

        Here is the document text:
        {extractedText}
        """;
    }
}