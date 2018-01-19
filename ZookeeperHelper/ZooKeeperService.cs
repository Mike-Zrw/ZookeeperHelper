using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZookeeperHelper
{
    /// <summary>
    /// 服务的注册等操作
    /// </summary>
    public class ZooKeeperService
    {
        public static void RegisterService(string url, string serName = null)
        {
            ZooKeeperClient client = new ZooKeeperClient(ConstData.ConnectionString);
            string path = ConstData.ServiceUrlRoot + "/" + serName ?? ConstData.ServiceName;
            client.CreateEphemeralSequentialNode(path, url);
        }
    }
}
