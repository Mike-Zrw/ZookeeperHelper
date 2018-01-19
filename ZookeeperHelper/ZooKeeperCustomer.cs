using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZookeeperHelper
{
    /// <summary>
    /// 客户端对服务的查询操作
    /// </summary>
    public class ZooKeeperCustomer
    {
        public static CacheHelper CacheHelper = new CacheHelper();
        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <param name="serName">服务名称</param>
        /// <returns></returns>
        public static string GetServiceUrl(string serName)
        {
            string cachePrex = "ZooKeeperServiceUrl_serName";
            string cacheData = CacheHelper.GetCache<string>(cachePrex + serName);
            if (cacheData != null)
                return cacheData;
            ZooKeeperClient client = new ZooKeeperClient(ConstData.ConnectionString);
            List<string> childs = client.GetChildren(ConstData.ServiceUrlRoot);
            if (childs.Count == 0)
            {
                throw new Exception("没有启动任何服务");
            }
            string selectSer = childs.Where(m => m.IndexOf(serName) == 0).FirstOrDefault();
            if (selectSer == null)
            {
                throw new Exception("请求的服务没有启动");
            }
            string serPath = ConstData.ServiceUrlRoot + "/" + selectSer;
            string url = client.GetData(serPath, new CustomerWatcher());
            CacheHelper.SetCache(cachePrex + serName, url, new TimeSpan(0, 0, 30));
            CacheHelper.SetCache(cachePrex + serPath, serName, new TimeSpan(0, 0, 30));
            return url;
        }
        /// <summary>
        /// 某个服务的连接断开,客户端删除该服务的缓存
        /// </summary>
        /// <param name="serPath"></param>
        public static void ServiceDisConnect(string serPath)
        {
            string cachePrex = "ZooKeeperServiceUrl_serName";
            string serName = CacheHelper.GetCache<string>(cachePrex + serPath);
            if (serName != null)
            {
                CacheHelper.RemoveCache(cachePrex + serPath);
                CacheHelper.RemoveCache(cachePrex + serName);
            }
        }
    }
}
