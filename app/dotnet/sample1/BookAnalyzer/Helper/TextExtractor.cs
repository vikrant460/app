namespace BookAnalyzer.Helper;

using System.Text;
using UglyToad.PdfPig;
public static class TextExtractor
{
    public static string ExtractText(string filePath, int maxPages = 10)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");
        using var pdf = PdfDocument.Open(filePath);
        var textBuilder = new StringBuilder();

        foreach (var page in pdf.GetPages().Take(maxPages))
        {
            textBuilder.AppendLine(page.Text);
        }

        return textBuilder.ToString();
    }
}