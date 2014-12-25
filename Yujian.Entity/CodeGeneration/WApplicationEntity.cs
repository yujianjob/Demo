/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/12/17 23:00:53
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

using System;
using System.Collections.Generic;
using System.Text;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.Entity;

namespace YuJian.WeiXin.Entity
{
    /// <summary>
    /// 实体：  
    /// </summary>
    public partial class WApplicationEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public WApplicationEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public Int32? ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// 1=订阅号（已认证）   2=服务号（未认证）   3=服务号（已认证）      关联 BasicData基础数据表
		/// </summary>
		public String Type { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WeixinID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WeixinNumber { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String DevelopUrl { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String DevelopToken { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String AppID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String AppSecret { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String LoginName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String LoginPassword { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String OAuthUrl { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String AccessToken { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ExpirationTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? CreateBy { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? LastUpdateBy { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? IsDelete { get; set; }


        #endregion

    }
}