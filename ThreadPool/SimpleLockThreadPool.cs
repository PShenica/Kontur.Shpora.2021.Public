using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace ThreadPool
{
    public class SimpleLockThreadPool : IThreadPool
    {
        private Thread[] pool;
        private ConcurrentQueue<Action> queue;

        public SimpleLockThreadPool(int concurrency)
        {

            queue = new ConcurrentQueue<Action>();
            pool = new Thread[concurrency];
            for (int i = 0; i < concurrency; i++)
            {
                var thread = new Thread(() =>
                {
                    while (true)
                    {
                        if (queue.TryDequeue(out var action))
                        {
                            action();
                        }

                    }
                });

                pool[i] = thread;
            }

            foreach (var thread in pool)
            {
                thread.Start();
            }
        }

        public void Dispose()
        {

        }

        public void EnqueueAction(Action action)
        {
            queue.Enqueue(action);
        }
    }
}