using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZookeeperHelper;

namespace ZkService
{
    class Program
    {
        static void Main(string[] args)
        {
            string serpath = ServiceHelper.RegisterService("localhost:2021", ConstData.ServiceName);
            Console.WriteLine("成功注册服务：" + ConstData.ServiceName + "\t" + serpath);
            string serpath2 = ServiceHelper.RegisterService("localhost:2022", ConstData.ServiceName);
            Console.WriteLine("成功注册服务：" + ConstData.ServiceName + "\t" + serpath2);
            Console.Read();
        }
    }
}
