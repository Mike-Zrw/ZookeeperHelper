using Org.Apache.Zookeeper.Data;
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
    /// 负责提供zookeeper的连接实例,并维护zookeeper的连接
    /// </summary>
    internal class ZooKeeperClient
    {
        public static ZooKeeper Instance;
        static ZooKeeperClient()
        {
            AutoResetEvent ConnectWaitEvent = new AutoResetEvent(false);
            Instance = new ZooKeeperNet.ZooKeeper(ConstData.ConnectionString, new TimeSpan(0, 0, 30), new ConnectWatcher(ConnectWaitEvent));
            WaitHandle.WaitAll(new WaitHandle[] { ConnectWaitEvent });
            Init();
        }
        /// <summary>
        /// 初始化基本的目录
        /// </summary>
        private static void Init()
        {
            try
            {
                if (null == Instance.Exists("/root", false))
                {
                    Console.WriteLine("root目录不存在，已创建");
                    Instance.Create("/root", "root".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }
                if (null == Instance.Exists(ConstData.ServiceUrlRoot, false))
                {
                    Console.WriteLine("ServiceUrls目录不存在，已创建");
                    Instance.Create(ConstData.ServiceUrlRoot, "Config Serveice Urls".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }
                string lockPath = "/Locks" + LockType.Default;
                if (ZooKeeperClient.Instance.Exists(lockPath, false) == null)
                    ZooKeeperClient.Instance.Create(lockPath, null, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>  
        /// 重连  
        /// </summary>  
        /// <returns></returns>  
        public static bool ReConnect()
        {
            AutoResetEvent ConnectWaitEvent = new AutoResetEvent(false);
            Instance = new ZooKeeperNet.ZooKeeper(ConstData.ConnectionString, new TimeSpan(0, 0, 30), new ConnectWatcher(ConnectWaitEvent));
            WaitHandle.WaitAll(new WaitHandle[] { ConnectWaitEvent });
            return true;
        }
    }
}
