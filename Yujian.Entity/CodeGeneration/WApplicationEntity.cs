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
    /// ʵ�壺  
    /// </summary>
    public partial class WApplicationEntity : BaseEntity 
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public WApplicationEntity()
        {
        }
        #endregion     

        #region ���Լ�
		/// <summary>
		/// 
		/// </summary>
		public Int32? ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// 1=���ĺţ�����֤��   2=����ţ�δ��֤��   3=����ţ�����֤��      ���� BasicData�������ݱ�
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