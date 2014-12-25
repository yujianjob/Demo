using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.Entity.Weixin
{

    /// <summary>
    /// 创建分组实体
    /// </summary>
    public class CreateGroupEntity : ResultEntity
    {
        /// <summary>
        /// 分组id，由微信分配 
        /// </summary>
        public string id
        { get; set; }
        /// <summary>
        /// 分组名字，UTF8编码 
        /// </summary>
        public string name
        { get; set; }
    }

    /// <summary>
    /// 获取的组类
    /// </summary>
    public class GroupDetails
    {
        /// <summary>
        ///  	分组id，由微信分配 
        /// </summary>
        public string id;
        /// <summary>
        ///  	分组名字，UTF8编码 
        /// </summary>
        public string name;
        /// <summary>
        ///  	分组内用户数量 
        /// </summary>
        public string count;
    }

    /// <summary>
    /// 获取分组结果类
    /// </summary>
    public class GetGroupResult : ResultEntity
    {
        /// <summary>
        ///  	公众平台分组信息列表 
        /// </summary>
        public List<GroupDetails> groups { get; set; }

    }

    /// <summary>
    /// 查询用户所在组
    /// </summary>
    public class QueryGroupResult : ResultEntity
    {
        public string groupid { get; set; }
    }

    
}
