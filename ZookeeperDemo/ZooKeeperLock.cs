using Org.Apache.Zookeeper.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace ZookeeperDemo
{
    public class ZooKeeperLock
    {
        public ZooKeeperClient Client;
        public ZooKeeperLock(ZooKeeperClient client)
        {
            this.Client = client;
        }

        /// <summary>  
        /// 锁释放后通过AutoResetEvent对象通知应用  
        /// </summary>  
        public static Hashtable _noticeResetsEvent = new Hashtable();

        private AutoResetEvent _connectResetEvent = new AutoResetEvent(false);

        public AutoResetEvent ConnectSuccessEvent { get { return _connectResetEvent; } }
        /// <summary>  
        /// 通知释放锁  
        /// </summary>  
        /// <param name="lockName"></param>  
        public void Release(string lockName)
        {
            if (_noticeResetsEvent.ContainsKey(lockName))
            {
                (_noticeResetsEvent[lockName] as AutoResetEvent).Set();
                lock (_noticeResetsEvent)
                {
                    _noticeResetsEvent.Remove(lockName);
                }
            }
        }
        /// <summary>  
        /// 获取锁  
        /// </summary>  
        /// <param name="lockName"></param>  
        /// <returns></returns>  
        public string Lock()
        {
            try
            {
                HashSet<ACL> list = new HashSet<ACL>();
                list.Add(new ACL((int)Perms.ALL, new ZKId("ip", "127.0.0.1")));
                string lockRootName = "/root/locks_";
                string result = Client.Instance.Create(lockRootName, "".GetBytes(), list, CreateMode.EphemeralSequential);
                List<string> childrens = (List<string>)Client.Instance.GetChildren("/root", false);
                IEnumerable<string> order = childrens.OrderBy(t => t);
                string PrevLock = GetPrevLock(result, lockRootName);
                string fristChild = order.First<string>();
                if (result.Replace("/root/", "").Equals(fristChild))
                {
                    Console.WriteLine("当前就是:" + fristChild);
                    return result;
                }
                else
                {
                    if (null != Client.Instance.Exists(PrevLock, new LockWatcher(this)))
                    {
                        ///加入通知事件中  
                        lock (_noticeResetsEvent)
                        {
                            _noticeResetsEvent[PrevLock] = new AutoResetEvent(false);
                        }
                    }
                    Console.WriteLine("当前是:" + fristChild + "新加入的是：" + result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>  
        /// 释放锁  
        /// </summary>  
        /// <param name="lockName"></param>  
        /// <returns></returns>  
        public bool UnLock(string lockName)
        {
            try
            {
                Client.Instance.Delete(lockName, -1);
                Console.WriteLine("释放成功。" + lockName);
                return true;
            }
            catch (ZooKeeperNet.KeeperException.SessionExpiredException se)
            {
                throw se;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>  
        /// 同步等待锁  
        /// </summary>  
        /// <param name="lockName"></param>  
        public void WaitLock(string lockName)
        {
            lockName = GetPrevLock(lockName, "/root/locks_");
            AutoResetEvent are = null;
            lock (_noticeResetsEvent)
            {
                if (_noticeResetsEvent.ContainsKey(lockName))
                {
                    are = (_noticeResetsEvent[lockName] as AutoResetEvent);
                }
            }
            if (null != are)
                WaitHandle.WaitAny(new WaitHandle[] { are });
            lock (_noticeResetsEvent)
            {
                _noticeResetsEvent.Remove(lockName);
            }
        }
        /// <summary>  
        /// 获取前一个锁目录名  
        /// </summary>  
        /// <param name="seq"></param>  
        /// <param name="lockName"></param>  
        /// <returns></returns>  
        private string GetPrevLock(string fileAllname, string lockName)
        {
            if (String.IsNullOrEmpty(fileAllname)) return String.Empty;
            string sSeqIndex = fileAllname.Replace(lockName, "");
            long seqIndex = 0;
            long.TryParse(sSeqIndex, out seqIndex);
            seqIndex = seqIndex - 1;
            string listen = lockName + seqIndex.ToString().PadLeft(fileAllname.Length - lockName.Length, '0');
            return listen;
        }
    }
}
