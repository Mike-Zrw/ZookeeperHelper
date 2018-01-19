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
        private readonly string connectionString;
        private AutoResetEvent ConnectWaitEvent = new AutoResetEvent(false);

        private static ZooKeeper _Instance;
        public ZooKeeper Instance
        {
            get { return _Instance; }
        }
        public bool ConnectSuccess()
        {
            this.ConnectWaitEvent.Set();
            return true;
        }
        public ZooKeeperClient(string connectionString)
        {
            if (_Instance == null)
            {
                ConnectWaitEvent = new AutoResetEvent(false);
                this.connectionString = connectionString;
                _Instance = new ZooKeeperNet.ZooKeeper(connectionString, new TimeSpan(0, 0, 30), new ConnectWatcher(this));
                WaitHandle.WaitAll(new WaitHandle[] { ConnectWaitEvent });
            }
        }
        //public void Init()
        //{
        //    try
        //    {
        //        if (null == Instance.Exists("/root", false))
        //        {
        //            Instance.Create("/root", "root".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    if (null == Instance.Exists(ConstData.ServiceUrlRoot, false))
        //    {
        //        Console.WriteLine("ServiceUrls不存在，已创建");
        //        Instance.Create(ConstData.ServiceUrlRoot, "Config the Serveice Urls".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
        //    }
        //}
        /// <summary>  
        /// 重连  
        /// </summary>  
        /// <returns></returns>  
        public bool ReConnect()
        {
            ConnectWaitEvent = new AutoResetEvent(false);
            _Instance = new ZooKeeperNet.ZooKeeper(connectionString, new TimeSpan(0, 0, 30), new ConnectWatcher(this));
            WaitHandle.WaitAll(new WaitHandle[] { ConnectWaitEvent });
            return true;
        }

        public void CreateEphemeralSequentialNode(string path, string data)
        {
            Instance.Create(path, data.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.EphemeralSequential);
        }
        public string GetData(string path,IWatcher watcher)
        {
            byte[] data = Instance.GetData(path, watcher, new Stat());
            string result = System.Text.Encoding.Default.GetString(data);
            return result;
        }
        public List<string> GetChildren(string path)
        {
            List<string> childs = Instance.GetChildren(path, false).ToList();
            return childs;

        }
    }
}
