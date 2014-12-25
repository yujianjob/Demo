using System.Collections.Generic;

namespace Yunchee.Volkswagen.Entity.Weixin
{
    /// <summary>
    /// 微信菜单 实体
    /// </summary>
    public class MenusEntity
    {
        /// <summary>
        /// 一级菜单数组，个数应为1~3个
        /// </summary>
        public List<MenuEntity> button { get; set; }
    }

    public class MenuEntity
    {
        /// <summary>
        /// 菜单的响应动作类型，目前有click、view两种类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        public string sub_button { get; set; }
        /// <summary>
        /// 网页链接，用户点击菜单可打开链接，不超过256字
        /// </summary>
        public string url { get; set; }
    }
}
