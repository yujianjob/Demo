﻿/*
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

using System;
using Yunchee.Volkswagen.Utility.ExtensionMethod;

namespace Yunchee.Volkswagen.Utility.DataAccess.Query
{
    /// <summary>
    /// 等值表达式 
    /// </summary>
    public class EqualsCondition : IWhereCondition
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public EqualsCondition()
        {
        }
        #endregion

        #region 属性集
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 如果值是时间类型,则通过本属性指定时间精度
        /// </summary>
        public DateTimeAccuracys? DateTimeAccuracy { get; set; }
        #endregion

        #region IWhereCondition Members
        /// <summary>
        /// 获取where条件表达式字符串
        /// </summary>
        /// <returns></returns>
        public string GetExpression()
        {
            //参数处理
            if (string.IsNullOrEmpty(this.FieldName) || this.Value == null)
                throw new ArgumentException("属性FieldName和Value都不能为null或空.");
            //处理字段名
            string fieldName = StringUtils.WrapperSQLServerObject(this.FieldName);
            //处理值
            string value = string.Empty;
            string valueType = this.Value.GetType().ToString();
            switch (valueType)
            {
                case "System.String":
                case "System.Guid":
                    value = string.Format("'{0}'", StringUtils.SqlReplace(this.Value.ToString()));
                    break;
                case "System.DateTime":
                    var accuracy = this.DateTimeAccuracy.HasValue ? this.DateTimeAccuracy.Value : DateTimeAccuracys.Date;
                    switch (accuracy)
                    {
                        case DateTimeAccuracys.Date:
                            fieldName = string.Format("CONVERT(nvarchar(10),{0},120)", fieldName);
                            value = string.Format("'{0}'", ((DateTime)this.Value).ToString("yyyy-MM-dd"));
                            break;
                        case DateTimeAccuracys.DateTime:
                            value = string.Format("'{0}'", this.Value.ToString());
                            break;
                    }
                    break;
                case "System.Char":
                    value = string.Format("'{0}'", this.Value.ToString());
                    break;
                case "System.Int32":
                case "System.UInt32":
                case "System.Int16":
                case "System.UInt16":
                case "System.Int64":
                case "System.UInt64":
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                case "System.Byte":
                case "System.SByte":
                    value = this.Value.ToString();
                    break;
                case "System.Boolean":
                    value = string.Format("{0}", ((bool)this.Value).ToInt32());
                    break;
                default:
                    throw new ArgumentException("无法处理的值类型.");
            }
            //
            return string.Format("{0}={1}", fieldName, value);
        }
        #endregion
    }
}
