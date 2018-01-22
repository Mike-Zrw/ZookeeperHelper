using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZookeeperHelper;
using ZooKeeperNet;

namespace ZkTest
{
    class Program
    {
        public static int index = 1;
        static void Main(string[] args)
        {
            List<Task> tsks = new List<Task>();
            for (int i = 0; i < 200; i++)
            {
                tsks.Add(Task.Run(() => {
                    string path=LockHelper.GetLock();
                    index++;
                    LockHelper.ReleaseLock(path);
                }));
            }
            Task.WaitAll(tsks.ToArray());
            Console.WriteLine(index);
            Console.ReadKey();
        }



        public static void TestConnect()
        {
            Console.WriteLine("建立连接");
            //服务地址为：localhost:2181  超时连接30秒
            using (ZooKeeper Instance = new ZooKeeperNet.ZooKeeper("localhost:2181", new TimeSpan(0, 0, 30), new Watcher("new")))
            {
                Console.WriteLine("检测是否有parent目录");
                var sdata = Instance.Exists("/parent", new Watcher("exists"));
                Console.WriteLine(sdata == null ? "否" : "是");
                if (sdata == null)
                {
                    Console.WriteLine("开始创建parent目录");
                    //data：目录关联的data为：this is the parentnode data
                    //acl：目录的访问权限控制
                    //CreateMode：Ephemeral：目录为临时目录，断开连接目录会自动清除  永久目录：Persistent  自增目录：***Sequential
                    Instance.Create("/parent", "this is the parentnode data".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
                    Console.WriteLine("创建parent目录完成");
                    Console.WriteLine("检测是否有parent目录");
                    sdata = Instance.Exists("/parent", new Watcher("exists2"));
                    Console.WriteLine(sdata == null ? "否" : "是");
                    if (sdata != null)
                    {
                        Console.WriteLine("删除parent目录");
                        Instance.Delete("/parent", 0);
                    }
                }
            }
        }
    }
}
