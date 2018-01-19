using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace ZookeeperHelper
{
    public class ConnectWatcher : IWatcher
    {
        private ZooKeeperClient _Client;
        public ConnectWatcher(ZooKeeperClient client)
        {
            _Client = client;
        }
        public void Process(WatchedEvent @event)
        {
            if (@event.State == KeeperState.SyncConnected)
            {
                Console.WriteLine("服务器连接成功。");
                _Client.ConnectSuccess();
            }
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
    /// <summary>
    /// 客户端对于服务地址变动的监听
    /// </summary>
    public class CustomerWatcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeDeleted)
            {
                ZooKeeperCustomer.ServiceDisConnect(@event.Path);
            }
        }
    }
}
