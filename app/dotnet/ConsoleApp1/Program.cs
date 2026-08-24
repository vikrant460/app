using BookAnalyzer;
var analyzer = new BookAnalyzerService(modelId: "phi4-mini");
string pdfPath = @"C:\Users\Vikrant\Desktop\Docs\Books\Self Improvement\The Gift of Imperfection.pdf";


Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("=================================================");
Console.WriteLine("          BOOK ANALYSIS DASHBOARD                ");
Console.WriteLine("=================================================");
Console.ResetColor();

Console.WriteLine($"\n[+] Processing file: {pdfPath}");
Console.WriteLine("[+] Sending context to local model (phi4-mini)... Please wait.\n");
var result = await analyzer.AnalyzePdfAsync(pdfPath);


if (result == null)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("[X] Analysis failed or returned empty results.");
    Console.ResetColor();
    return;
}

// 1. Executive Summary Section
RenderSectionHeader("EXECUTIVE SUMMARY", ConsoleColor.Yellow);
Console.WriteLine(string.IsNullOrWhiteSpace(result.Summary) ? "No summary provided." : result.Summary.Trim());

// 2. Analytical Takeaway Section
RenderSectionHeader("ANALYTICAL TAKEAWAY", ConsoleColor.Green);
Console.WriteLine(string.IsNullOrWhiteSpace(result.CoreReview) ? "No core review provided." : result.CoreReview.Trim());

// 3. Key Quotes / Highlighted Points Section
RenderSectionHeader("KEY HIGHLIGHTS & QUOTES", ConsoleColor.Magenta);
if (result.KeyQuotes != null && result.KeyQuotes.Count > 0)
{
    foreach (var quote in result.KeyQuotes)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" • ");
        Console.ResetColor();
        Console.WriteLine($"\"{quote.Trim()}\"");
    }
}
else
{
    Console.WriteLine("No verified verbatim quotes extracted.");
}

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("\n=================================================");
Console.ResetColor();


// Helper method for consistent UI styling
static void RenderSectionHeader(string title, ConsoleColor color)
{
    Console.WriteLine();
    Console.ForegroundColor = color;
    Console.WriteLine($"┌─ {title} " + new string('─', 45 - title.Length));
    Console.ResetColor();
}