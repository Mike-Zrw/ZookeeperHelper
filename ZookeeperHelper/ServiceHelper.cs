using Org.Apache.Zookeeper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace ZookeeperHelper
{
    /// <summary>
    /// 服务的注册查询等操作
    /// </summary>
    public class ServiceHelper
    {
        public static CacheHelper CacheHelper = new CacheHelper();
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="url">服务地址</param>
        /// <param name="serName">服务名称</param>
        /// <returns>返回服务路径</returns>
        public static string RegisterService(string url, string serName)
        {
            string parentpath = ConstData.ServiceUrlRoot + "/" + serName;
            Stat stat = ZooKeeperClient.Instance.Exists(parentpath, false);
            if (stat == null)
            {
                ZooKeeperClient.Instance.Create(parentpath, null, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }
            string serpath = CreateServiceNode(url, serName);
            return serpath;
        }
        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <param name="serName">服务名称</param>
        /// <returns></returns>
        public static string GetServiceUrl(string serName)
        {
            string cacheData = CacheHelper.GetCache<string>(ConstData.CacheSerName_Prefix + serName);
            if (cacheData != null)
                return cacheData;
            string serParent = ConstData.ServiceUrlRoot + "/" + serName;
            List<string> urls = ZooKeeperClient.Instance.GetChildren(serParent, false).ToList();
            if (urls.Count == 0)
            {
                throw new Exception("请求的服务没有启动");
            }
            string url = urls[0];
            string serPath = BuildServicePath(serName, url);
            ZooKeeperClient.Instance.Exists(serPath, new CustomerWatcher());//监听服务的连接状态
            CacheHelper.SetCache(ConstData.CacheSerName_Prefix + serName, url, new TimeSpan(0, 0, 60));
            CacheHelper.SetCache(ConstData.CacheSerName_Prefix + serPath, serName, new TimeSpan(0, 0, 60));
            return url;
        }
        /// <summary>
        /// 创建一个服务节点
        /// </summary>
        /// <param name="url"></param>
        /// <param name="serName"></param>
        /// <returns>服务节点路径</returns>
        private static string CreateServiceNode(string url, string serName)
        {
            string serPath = BuildServicePath(serName, url);
            ZooKeeperClient.Instance.Create(serPath, null, Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
            return serPath;
        }

      
        /// <summary>
        /// 监听某个服务的连接断开,客户端删除该服务的缓存
        /// </summary>
        /// <param name="serPath"></param>
        public static void SerDisConnectListener(string serPath)
        {
            string serName = CacheHelper.GetCache<string>(ConstData.CacheSerName_Prefix + serPath);
            if (serName != null)
            {
                CacheHelper.RemoveCache(ConstData.CacheSerName_Prefix + serPath);
                CacheHelper.RemoveCache(ConstData.CacheSerName_Prefix + serName);
            }
        }
        /// <summary>
        /// 服务的节点路径
        /// </summary>
        /// <param name="serName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string BuildServicePath(string serName, string url)
        {
            string path = ConstData.ServiceUrlRoot + "/" + serName + "/" + url;
            return path;
        }
    }
}
