using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            var q = new PriorityQueue<int>();
            var addLow = Task.Run(
                () =>
                {
                    for (int i = 0; i < 1000; i++)
                        q.Enqueue(new Random(i).Next(0, 9), Priority.Low);
                });
            var addNormal = Task.Run(
                () =>
                {
                    ICollection<int> c = q.GetAsICollection(Priority.Normal);
                    for (int i = 0; i < 1000; i++)
                        c.Add(new Random(i).Next(100, 199));
                });
            var addHigh = Task.Run(
                () =>
                {
                    for (int i = 0; i < 1000; i++)
                        q.Enqueue(new Random(i).Next(1000, 1999), Priority.High);
                });

            var sb = new StringBuilder();
            while (!addLow.IsCompleted || !addNormal.IsCompleted || !addHigh.IsCompleted || q.Count > 0)
            {
                var item = 0;
                if (q.TryDequeue(out item))
                {
                    sb.AppendLine(item.ToString());
                }
            }

            sw.Stop();
            Console.WriteLine("Time elapsed: {0} ms.", sw.ElapsedMilliseconds);
            File.WriteAllText("d:\\Test.txt", sb.ToString());
            Console.ReadKey();
        }
    }
}
