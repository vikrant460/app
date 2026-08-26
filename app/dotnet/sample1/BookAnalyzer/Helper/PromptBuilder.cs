namespace BookAnalyzer.Helper;

public static class PromptBuilder
{
   
   public static string Build(string extractedText)
   {
      return string.Format(Prompts.KeyQuotes, extractedText);
   }
}