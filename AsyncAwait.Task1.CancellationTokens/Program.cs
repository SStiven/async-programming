﻿/*
* Study the code of this application to calculate the sum of integers from 0 to N, and then
* change the application code so that the following requirements are met:
* 1. The calculation must be performed asynchronously.
* 2. N is set by the user from the console. The user has the right to make a new boundary in the calculation process,
* which should lead to the restart of the calculation.
* 3. When restarting the calculation, the application should continue working without any failures.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal class Program
{
    private static CancellationTokenSource cts;

    /// <summary>
    /// The Main method should not be changed at all.
    /// </summary>
    /// <param name="args"></param>
    private static void Main(string[] args)
    {
        Console.WriteLine("Calculating the sum of integers from 0 to N.");
        Console.WriteLine("Use 'q' key to exit...");
        Console.WriteLine();

        Console.WriteLine("Enter N: ");

        var input = Console.ReadLine();
        while (input.Trim().ToUpper() != "Q")
        {
            if (int.TryParse(input, out var n))
            {
                CalculateSum(n);
            }
            else
            {
                Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                Console.WriteLine("Enter N: ");
            }

            input = Console.ReadLine();
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private static void CalculateSum(int n)
    {
        if (cts is not null)
        {
            cts.Cancel();
            cts.Dispose();
        }

        cts = new();
        var token = cts.Token;

        Console.WriteLine();
        Console.WriteLine($"The task for {n} started... Enter a new 'N' to cancel the request:");
        var task = Calculator.CalculateAsync(n, token);

        task.ContinueWith(t =>
        {
            if (t.IsCanceled)
            {
                Console.WriteLine($"-- Sum for {n} cancelled");
            }
            else if (t.IsFaulted)
            {
                Console.WriteLine($"-- Sum from {n} has failed: {t.Exception.InnerException.Message}");
            }
            else
            {
                PrintResult(n, t.Result);
                Console.WriteLine("Enter N: ");
            }


        }, TaskContinuationOptions.RunContinuationsAsynchronously);
    }

    private static void PrintResult(int n, long result)
    {
        Console.WriteLine();
        Console.WriteLine($"Sum for {n} = {result}");
        Console.WriteLine();
    }
}
