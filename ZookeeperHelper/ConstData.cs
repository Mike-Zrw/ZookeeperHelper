using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZookeeperHelper
{
    public class ConstData
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public static readonly string ServiceName ="ser1";
        /// <summary>
        /// 服务的根目录
        /// </summary>
        public static string ServiceUrlRoot = "/root/service_providers";
        /// <summary>
        /// zk地址
        /// </summary>
        public static string ConnectionString = "localhost:2181";
        /// <summary>
        /// 服务名字缓存前缀
        /// </summary>
        public static string CacheSerName_Prefix = "ZooKeeperServiceUrl_serName";
    }
    /// <summary>
    /// 锁类型
    /// </summary>
    public enum LockType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default=1
    }
}
