/*
 * Author		:
 * EMail		:
 * Company		:
 * Create On	:
 * Description	:
 * 1st Modified On	:
 * 1st Modified By	:
 * 1st Modified Desc:
 * 1st Modifier EMail	:
 * 2st Modified On	:
 * 2st Modified By	:
 * 2st Modifier EMail	:
 * 2st Modified Desc:
 */

namespace Yunchee.Volkswagen.Utility
{
    /// <summary>
    /// 基础用户信息 
    /// </summary>
    public class BasicUserInfo
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public BasicUserInfo()
        {
        }
        #endregion

        /// <summary>
        /// 客户ID
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// 客户类型  1 = 区域  2 = 经销商
        /// </summary>
        public string ClientType { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string ChineseName { get; set; }
    }
}
