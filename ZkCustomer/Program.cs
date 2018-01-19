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
            string url = ZooKeeperCustomer.GetServiceUrl("testser1");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                 url = ZooKeeperCustomer.GetServiceUrl("testser1");
            }
            Console.WriteLine(url);
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
