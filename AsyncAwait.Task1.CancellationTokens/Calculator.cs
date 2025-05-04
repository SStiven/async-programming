using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    public static Task<long> CalculateAsync(int n, CancellationToken token)
    {
        return Task.Run(() =>
        {
            token.ThrowIfCancellationRequested();

            long sum = 0;
            for (int i = 1; i <= n; i++)
            {
                token.ThrowIfCancellationRequested();

                sum = checked(sum + i);
                Thread.Sleep(500);
            }

            return sum;
        }, token);
    }
}
