/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/12/18 22:21:09
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
    public partial class VisitIPEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public VisitIPEntity()
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
		public String Openid { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String IPAddress { get; set; }

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