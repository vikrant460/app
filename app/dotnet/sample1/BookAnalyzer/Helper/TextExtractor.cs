namespace BookAnalyzer.Helper;

using UglyToad.PdfPig;
public static class TextExtractor
{
    public static string ExtractTextFromPdf(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"PDF not found: {filePath}");
        using var pdf = PdfDocument.Open(filePath);
        var textBuilder = new System.Text.StringBuilder();

        foreach (var page in pdf.GetPages())
        {
            textBuilder.AppendLine(page.Text);
        }
      
        return textBuilder.ToString();
    }
}