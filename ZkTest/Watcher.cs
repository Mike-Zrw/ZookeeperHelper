using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace ZkTest
{
    public class Watcher : IWatcher
    {
        public string Handler { get; set; }
        public Watcher(string handler)
        {
            Handler = handler;
        }
        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.None)
            {
                Console.WriteLine(Handler + " None\n" + JsonConvert.SerializeObject(@event));
            }
            else if (@event.Type == EventType.NodeCreated)
            {
                Console.WriteLine(Handler + " NodeCreated\n" + JsonConvert.SerializeObject(@event));
            }
            else if (@event.Type == EventType.NodeDeleted)
            {
                Console.WriteLine(Handler + " NodeDeleted\n" + JsonConvert.SerializeObject(@event));
            }
            else if (@event.Type == EventType.NodeDataChanged)
            {
                Console.WriteLine(Handler + " NodeDataChanged\n" + JsonConvert.SerializeObject(@event));
            }
            else if (@event.Type == EventType.NodeChildrenChanged)
            {
                Console.WriteLine(Handler + " NodeChildrenChanged\n" + JsonConvert.SerializeObject(@event));
            }
        }
    }
}
