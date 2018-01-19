using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZookeeperDemo
{
    public class ZooKeeperCustomer
    {
        private static ZooKeeperClient client = new ZooKeeperClient(ConstData.ConnectionString);
        public static string GetServiceUrl(string serName)
        {
            string path = ConstData.ServiceUrlRoot + "/" + serName;
            return client.GetData(path);
        }
    }
}
