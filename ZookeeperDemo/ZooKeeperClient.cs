using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace ZookeeperDemo
{
    public class ZooKeeperClient
    {
        private readonly string connectionString;

        private  ZooKeeper _Instance;
        public ZooKeeper Instance
        {
            get { return _Instance; }
        }
        public ZooKeeperClient(string connectionString)
        {
            this.connectionString = connectionString;
            if (_Instance == null)
            {
                _Instance = new ZooKeeperNet.ZooKeeper(connectionString, new TimeSpan(0, 0, 30), new ConnectWatcher(this));
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
            _Instance = new ZooKeeperNet.ZooKeeper(connectionString, new TimeSpan(0, 0, 30), new ConnectWatcher(this));
            return true;
        }

        public void CreateEphemeralNode(string path, string data)
        {
            Instance.Create(path, data.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
        }
        public string GetData(string path)
        {
            try
            {
                byte[] data = Instance.GetData(path, false, new Stat());
                string result = System.Text.Encoding.Default.GetString(data);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
