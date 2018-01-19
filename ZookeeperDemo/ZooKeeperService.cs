using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZookeeperDemo
{
    /// <summary>
    /// 服务的注册等操作
    /// </summary>
    public class ZooKeeperService
    {
        public static ZooKeeperClient client = new ZooKeeperClient(ConstData.ConnectionString);
        /// <summary>
        /// 注册当前的服务到zookeeper
        /// </summary>
        /// <param name="url"></param>
        /// <param name="serName"></param>
        public static void RegisterService(string url, string serName = null)
        {
            string hostname=System.Net.Dns.GetHostName();
            string path = ConstData.ServiceUrlRoot + "/" + serName ?? ConstData.ServiceName;
            client.CreateEphemeralNode(path, url);
        }
    }
}
