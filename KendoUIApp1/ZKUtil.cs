﻿namespace KendoUIApp1
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using ZooKeeperNet;

    public abstract class ZKUtil
    {
        static ZKUtil()
        {
            //XmlConfigurator.Configure();
        }

        protected static readonly TimeSpan CONNECTION_TIMEOUT = new TimeSpan(0, 0, 0, 0, 20000);

        protected virtual ZooKeeper CreateClient()
        {
            CountdownWatcher watcher = new CountdownWatcher();
              return new ZooKeeper("10.1.13.1:2181", /*"54.202.130.108:2181",*/ new TimeSpan(0, 0, 0, 20000), watcher);
//            return new ZooKeeper("10.1.13.1:2181", new TimeSpan(0, 0, 0, 10000), watcher);
        }

        protected virtual ZooKeeper CreateClient(string node)
        {
            CountdownWatcher watcher = new CountdownWatcher();
            return new ZooKeeper("10.1.13.1:2181" + node, new TimeSpan(0, 0, 0, 10000), watcher);
        }

        protected ZooKeeper CreateClient(IWatcher watcher)
        {
            return new ZooKeeper("10.1.13.1:2181", new TimeSpan(0, 0, 0, 10000), watcher);
        }

        protected ZooKeeper CreateClientWithAddress(string address)
        {
            CountdownWatcher watcher = new CountdownWatcher();
            return new ZooKeeper(address, new TimeSpan(0, 0, 0, 10000), watcher);
        }

        public class CountdownWatcher : IWatcher
        {
            readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
            private static readonly object sync = new object();

            volatile bool connected;

            public CountdownWatcher()
            {
                Reset();
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Reset()
            {
                resetEvent.Set();
                connected = false;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public virtual void Process(WatchedEvent @event)
            {
                if (@event.State == KeeperState.SyncConnected)
                {
                    connected = true;
                    lock (sync)
                    {
                        Monitor.PulseAll(sync);
                    }
                    resetEvent.Set();
                }
                else
                {
                    connected = false;
                    lock (sync)
                    {
                        Monitor.PulseAll(sync);
                    }
                }
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            bool IsConnected()
            {
                return connected;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            void waitForConnected(TimeSpan timeout)
            {
                DateTime expire = DateTime.UtcNow + timeout;
                TimeSpan left = timeout;
                while (!connected && left.TotalMilliseconds > 0)
                {
                    lock (sync)
                    {
                        Monitor.TryEnter(sync, left);
                    }
                    left = expire - DateTime.UtcNow;
                }
                if (!connected)
                {
                    throw new TimeoutException("Did not connect");

                }
            }

            void waitForDisconnected(TimeSpan timeout)
            {
                DateTime expire = DateTime.UtcNow + timeout;
                TimeSpan left = timeout;
                while (connected && left.TotalMilliseconds > 0)
                {
                    lock (sync)
                    {
                        Monitor.TryEnter(sync, left);
                    }
                    left = expire - DateTime.UtcNow;
                }
                if (connected)
                {
                    throw new TimeoutException("Did not disconnect");
                }
            }
        }

    }
}
