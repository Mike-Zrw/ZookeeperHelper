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
        public static Dictionary<string, AutoResetEvent> WaitMaps = new Dictionary<string, AutoResetEvent>();
        public static string GetLock(LockType type = LockType.Default)
        {
            string parentPath = "/Locks" + type;
            string path = ZooKeeperClient.Instance.Create(parentPath + "/Lock_", null, Ids.OPEN_ACL_UNSAFE, CreateMode.EphemeralSequential);
            while (true)
            {
                string prevNode = GetPrevNode(type, path);
                if (prevNode == null)
                {
                    return path;
                }
                else
                {
                    AutoResetEvent deleteWaitEvent = new AutoResetEvent(false);
                    ZooKeeperClient.Instance.Exists(prevNode, new LockWatcher());
                    WaitMaps.Add(prevNode, deleteWaitEvent);
                    WaitHandle.WaitAny(new WaitHandle[] { deleteWaitEvent });
                    WaitMaps.Remove(prevNode);
                }
            }
        }
        public static void Unlock(string lockPath)
        {
            ZooKeeperClient.Instance.Delete(lockPath, -1);
        }
        public static void ReleaseLock(string lockPath)
        {
            if (WaitMaps.ContainsKey(lockPath))
            {
                WaitMaps[lockPath].Set();
            }
        }

        public static string GetPrevNode(LockType type, string path)
        {
            string parentPath = "/Locks" + type;
            List<string> childs = ZooKeeperClient.Instance.GetChildren(parentPath, false).OrderBy(m => m).ToList();
            int pathIndex = childs.FindIndex(m => parentPath + "/" + m == path);
            if (pathIndex == 0)
                return null;
            else
                return parentPath + "/" + childs[pathIndex - 1];
        }
    }
}
