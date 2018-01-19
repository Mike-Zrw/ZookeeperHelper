using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace ZookeeperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            serviceTest();
            Console.ReadKey();
            /*
            //创建一个Zookeeper实例，第一个参数为目标服务器地址和端口，第二个参数为Session超时时间，第三个为节点变化时的回调方法 
            using (ZooKeeper zk = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 0, 50000), new Watcher()))
            {
                try
                {
                    var stat = zk.Exists("/root", true);
                }
                catch (Exception ex)
                {
                    ////创建一个节点root，数据是mydata,不进行ACL权限控制，节点为永久性的(即客户端shutdown了也不会消失) 
                   zk.Create("/root", "mydata".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }


                //在root下面创建一个childone znode,数据为childone,不进行ACL权限控制，节点为永久性的 
                zk.Create("/root/childone", "childone".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                //取得/root节点下的子节点名称,返回List<String> 
                zk.GetChildren("/root", true);
                //取得/root/childone节点下的数据,返回byte[] 
                zk.GetData("/root/childone", true, null);

                //修改节点/root/childone下的数据，第三个参数为版本，如果是-1，那会无视被修改的数据版本，直接改掉
                zk.SetData("/root/childone", "childonemodify".GetBytes(), -1);
                //删除/root/childone这个节点，第二个参数为版本，－1的话直接删除，无视版本 
                zk.Delete("/root/childone", -1);
            }
            */

        }
        public static void locktest()
        {
            ZooKeeperLock z = new ZooKeeperLock(new ZooKeeperClient("127.0.0.1:2181"));

            string lockName = z.Lock();
            if (!String.IsNullOrEmpty(lockName))
            {
                z.WaitLock(lockName);
                var count = 1 * 100;
                Console.WriteLine(count);
                z.UnLock(lockName);
            }

            Console.ReadKey();
        }

        public static void serviceTest()
        {
            //ZooKeeperService.RegisterService("localhost:1010", "ser1");
            string url = ZooKeeperCustomer.GetServiceUrl("ser1");
            Console.WriteLine(url);
        }
    }
}
