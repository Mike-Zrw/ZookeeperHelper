using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZookeeperHelper;

namespace ZkCustomer
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = ServiceHelper.GetServiceUrl(ConstData.ServiceName);
            Console.WriteLine(string.Format("获取服务{0}地址：{1}", ConstData.ServiceName, url));
            Console.ReadLine();
        }
    }
}
