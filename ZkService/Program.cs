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
            ZooKeeperService.RegisterService("localhost:2021", "testser1");
            ZooKeeperService.RegisterService("localhost:2022", "testser2");
            Console.WriteLine("ok");
            Console.Read();
        }
    }
}
