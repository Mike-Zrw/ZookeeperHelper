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
    public class ZooKeeperClient
    {
        private static readonly string connectionString = ConstData.ConnectionString;
        public static ZooKeeper Instance;
        static ZooKeeperClient()
        {
            AutoResetEvent ConnectWaitEvent = new AutoResetEvent(false);
            Instance = new ZooKeeperNet.ZooKeeper(connectionString, new TimeSpan(0, 0, 30), new ConnectWatcher(ConnectWaitEvent));
            WaitHandle.WaitAll(new WaitHandle[] { ConnectWaitEvent });
            Init();
        }

        /// <summary>
        /// 初始化基本的目录
        /// </summary>
        public static void Init()
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
            Instance = new ZooKeeperNet.ZooKeeper(connectionString, new TimeSpan(0, 0, 30), new ConnectWatcher(ConnectWaitEvent));
            WaitHandle.WaitAll(new WaitHandle[] { ConnectWaitEvent });
            return true;
        }
        /// <summary>
        /// 创建一个临时的自增长的节点
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void CreateEphemeralSequentialNode(string path, string data)
        {
            var stat = Instance.Exists(path, false);
            if (stat == null)
            {
                Instance.Create(path, data.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
                Instance.SetData(path, data.GetBytes(), 0);
            }
            else
            {
                Instance.Create(path, data.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
                Instance.SetData(path, data.GetBytes(), 2);
            }
        }
        /// <summary>
        /// 获取节点绑定的data
        /// </summary>
        /// <param name="path"></param>
        /// <param name="watcher"></param>
        /// <returns></returns>
        public static string GetData(string path, IWatcher watcher)
        {
            byte[] data = Instance.GetData(path, watcher, new Stat());
            string result = System.Text.Encoding.Default.GetString(data);
            return result;
        }
       
    }
}
