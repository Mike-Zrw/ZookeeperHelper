using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace ZookeeperDemo
{
    public class LockWatcher : ConnectWatcher
    {
        private ZooKeeperLock locker;

        public LockWatcher(ZooKeeperLock locker):base(locker.Client)
        {
            this.locker = locker;
        }
        public new void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeDeleted)
                locker.Release(@event.Path);//释放目录对应的锁  
            else if (@event.State == KeeperState.SyncConnected)
                locker.ConnectSuccessEvent.Set();//连接成功
            else
                base.Process(@event);
        }
    }
    public class ConnectWatcher : IWatcher
    {
        private ZooKeeperClient _Client;
        public ConnectWatcher(ZooKeeperClient client)
        {
            _Client = client;
        }
        public void Process(WatchedEvent @event)
        {
            if (@event.State == KeeperState.Disconnected)
            {
                Console.WriteLine("服务器中断重连。");
                _Client.ReConnect();
            }
            if (@event.State == KeeperState.Expired)
            {
                Console.WriteLine("连接已超时重连。");
                _Client.ReConnect();
            }
        }
    }

    internal class Watcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
