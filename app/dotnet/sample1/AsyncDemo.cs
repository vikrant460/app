public class AsyncDemo
{
    public static async Task DoWorkAsync(long counter, IProgress<int> progress, CancellationToken cancellationToken)
    {
        try
        {
            for (int i = 0; i <= counter; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Console.WriteLine($"Working... {i}");
                progress.Report(CalculatePercentage((int)counter, i));
            }
        }
        catch (OperationCanceledException)
        {
            
            throw;
        }
    }
    private static int CalculatePercentage(int total, int completed)
    {
        if (total == 0) throw new ArgumentException("Total cannot be zero.", nameof(total));
        if (completed < 0 || completed > total) throw new ArgumentOutOfRangeException(nameof(completed), "Completed must be between 0 and total.");

        return (int)((double)completed / total * 100);
    }

}