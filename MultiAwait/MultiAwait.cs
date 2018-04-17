using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sample
{
    class Program
    {
        static readonly object sync = new object();
        static readonly Stopwatch stopwatch = new Stopwatch();

        static async Task Test(bool yield, Task task, int id)
        {
            await task;
            if (yield) await Task.Yield();
            Thread.Sleep(TimeSpan.FromMilliseconds(2000));
            lock (sync) Console.WriteLine($"  {id}: {stopwatch.ElapsedMilliseconds, 5} ms");
        }

        static void Test(bool yield)
        {
            Console.WriteLine($"start (yield = {yield})");

            var tcs = new TaskCompletionSource<object>();
            var task = Task.WhenAll(from id in Enumerable.Range(0, 3) select Test(yield, tcs.Task, id));

            lock (sync)
            {
                Console.WriteLine(" A");
                stopwatch.Restart();
                tcs.SetResult(null);
                Console.WriteLine(" B");
            }

            task.Wait();
            Console.WriteLine("stop");
        }

        static void Main()
        {
            Test(false);
            Console.WriteLine();
            Test(true);
            Console.ReadLine();
        }
    }
}
