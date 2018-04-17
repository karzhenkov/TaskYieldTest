using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static Task task;
        static readonly object sync = new object();

        static async Task Test(bool yield, int id)
        {
            var prev = task;
            var tcs = new TaskCompletionSource<object>();
            task = tcs.Task;

            await prev;
            if (yield) await Task.Yield();

            lock (sync)
            {
                var indent = new string(' ', id);
                Console.WriteLine($"{indent}{id} A");
                tcs.SetResult(null);
                Console.WriteLine($"{indent}{id} B");
            }
        }

        static void Test(bool yield)
        {
            Console.WriteLine($"yield = {yield}");
            task = Task.Delay(TimeSpan.FromMilliseconds(100));
            Task.WhenAll(from id in Enumerable.Range(0, 5) select Test(yield, id)).Wait();
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
