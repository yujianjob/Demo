/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/12/17 23:00:52
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
    public partial class CustomerEntity : BaseEntity 
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public CustomerEntity()
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
		/// 
		/// </summary>
		public String Code { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Phone { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxOpenId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxNickName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxSex { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxCity { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxCountry { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxProvince { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxLanguage { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxHeadImgUrl { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String WxSubscribeTime { get; set; }

		/// <summary>
		/// 1=��ע   2=ȡ����ע      ���� BasicData�������ݱ�
		/// </summary>
		public String SubscribeStatus { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? UnSubscribeDate { get; set; }

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