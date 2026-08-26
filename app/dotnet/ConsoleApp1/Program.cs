using BookAnalyzer;

if (args.Length == 0)
{
    Console.WriteLine("Usage: BookAnalyzer <pdf-path>");
    return;
}

var pdfPath = args[0];

if (!File.Exists(pdfPath))
{
    Console.WriteLine($"File not found: {pdfPath}");
    return;
}


Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("=================================================");
Console.WriteLine("          BOOK ANALYSIS DASHBOARD                ");
Console.WriteLine("=================================================");
Console.ResetColor();

Console.WriteLine($"\n[+] Processing file: {pdfPath}");

var analyzer = new AnalyzerService(modelId: "phi4-mini");

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