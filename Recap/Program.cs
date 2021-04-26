using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Recap
{
    class Program
    {
        static void Main(string[] args)
        {
            var resolution = (long)1;
            var processorNum = 1;
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)(1 << processorNum);

            var t = new Thread(() =>
            {
                while (true)
                {
                }
            });
            t.Start();

            var secondTread = new Thread(() =>
            {
                var watch = new Stopwatch();
                var time = (long)0;

                while (true)
                {
                    watch.Restart();
                    watch.Stop();
                    if (watch.ElapsedMilliseconds - time > resolution)
                    {
                        Console.WriteLine(watch.ElapsedMilliseconds);
                    }
                    time = watch.ElapsedMilliseconds;
                }

            });
            secondTread.Start();
        }
    }
}
