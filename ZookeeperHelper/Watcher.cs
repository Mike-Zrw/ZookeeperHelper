using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace ZookeeperHelper
{
    /// <summary>
    /// 客户端连接的监听
    /// </summary>
    public class ConnectWatcher : IWatcher
    {
        private AutoResetEvent connectWaitEvent;


        public ConnectWatcher(AutoResetEvent connectWaitEvent)
        {
            this.connectWaitEvent = connectWaitEvent;
        }

        public void Process(WatchedEvent @event)
        {
            if (@event.State == KeeperState.SyncConnected)
            {
                Console.WriteLine("服务器连接成功。");
                connectWaitEvent.Set(); 
            }
            if (@event.State == KeeperState.Disconnected)
            {
                Console.WriteLine("服务器中断重连。");
                ZooKeeperClient.ReConnect();
            }
            if (@event.State == KeeperState.Expired)
            {
                Console.WriteLine("连接已超时重连。");
                ZooKeeperClient.ReConnect();
            }
        }
    }
    public class LockWatcher : IWatcher
    {
        public LockWatcher()
        {
        }

        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeDeleted)
            {
                LockHelper.ReleaseLock(@event.Path);
            }
        }
    }
    /// <summary>
    /// 客户端对于服务地址变动的监听
    /// </summary>
    public class CustomerWatcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeDeleted)
            {
                ServiceHelper.SerDisConnectListener(@event.Path);
            }
        }
    }
}
