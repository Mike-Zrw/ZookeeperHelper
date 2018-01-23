using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace ZookeeperHelper
{
    public class LockHelper
    {
        /// <summary>
        /// 锁和等待的映射
        /// </summary>
        internal static Dictionary<string, AutoResetEvent> WaitMaps = new Dictionary<string, AutoResetEvent>();
        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="type">锁类型</param>
        /// <returns>锁的全路径</returns>
        public static string GetLock(LockType type = LockType.Default)
        {
            string parentPath = "/Locks" + type;
            string path = ZooKeeperClient.Instance.Create(parentPath + "/Lock_", null, Ids.OPEN_ACL_UNSAFE, CreateMode.EphemeralSequential);
            while (true)
            {
                string prevNode = GetPrevNode(path);
                if (prevNode == null)
                {
                    //Console.WriteLine(string.Format("创建：{0}", path));
                    return path;
                }
                else
                {
                    AutoResetEvent deleteWaitEvent = new AutoResetEvent(false);
                    var existPrev = ZooKeeperClient.Instance.Exists(prevNode, new LockWatcher());
                    if (existPrev != null)
                    {
                        WaitMaps.Add(prevNode, deleteWaitEvent);
                        //Console.WriteLine(string.Format("创建：{0}，等待：{1}", path, prevNode));
                        WaitHandle.WaitAny(new WaitHandle[] { deleteWaitEvent });
                        WaitMaps.Remove(prevNode);
                        //Console.WriteLine(string.Format("等待完成：{0}", prevNode));
                    }
                }
            }
        }
        /// <summary>
        /// 解除/删除 锁
        /// </summary>
        /// <param name="lockPath">锁的全路径</param>
        public static void Unlock(string lockPath)
        {
            ZooKeeperClient.Instance.Delete(lockPath, -1);
            ReleaseLock(lockPath);
        }
        /// <summary>
        /// 释放锁占用的资源
        /// </summary>
        /// <param name="lockPath"></param>
        public static void ReleaseLock(string lockPath)
        {
            if (WaitMaps.ContainsKey(lockPath))
            {
                WaitMaps[lockPath].Set();
            }
        }
        /// <summary>
        /// 获取顺序目录中,当前目录的前一个目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns>目录全路径</returns>
        public static string GetPrevNode(string path)
        {
            string parentPath = path.Substring(0, path.LastIndexOf("/"));
            List<string> childs = ZooKeeperClient.Instance.GetChildren(parentPath, false).OrderBy(m => m).ToList();
            int pathIndex = childs.FindIndex(m => parentPath + "/" + m == path);
            if (pathIndex == 0)
                return null;
            else
            {
                return parentPath + "/" + childs[pathIndex - 1];
            }
        }
    }
}
