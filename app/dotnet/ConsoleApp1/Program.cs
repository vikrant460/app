using BookAnalyzer;
var analyzer = new AnalyzerService(modelId: "phi4-mini");
string pdfPath = @"C:\Users\Vikrant\Desktop\Docs\Books\Fiction\Demian-By-Hermann-Hesse.pdf";


Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("=================================================");
Console.WriteLine("          BOOK ANALYSIS DASHBOARD                ");
Console.WriteLine("=================================================");
Console.ResetColor();

Console.WriteLine($"\n[+] Processing file: {pdfPath}");
Console.WriteLine("[+] Sending context to local model (phi4-mini)... Please wait.\n");
var result = await analyzer.AnalyzeAsync(pdfPath);


if (result == null)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("[X] Analysis failed or returned empty results.");
    Console.ResetColor();
    return;
}


// 3. Key Quotes / Highlighted Points Section
RenderSectionHeader("QUOTES", ConsoleColor.Magenta);
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