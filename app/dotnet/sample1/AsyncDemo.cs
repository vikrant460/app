public class AsyncDemo
{
    public static async Task DoWorkAsync(long counter, CancellationToken cancellationToken)
    {
        try
        {
            for (int i = 0; i < counter; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Console.WriteLine($"Working... {i}");
            }
        }
        catch (OperationCanceledException)
        {
            
            throw;
        }
    }

}