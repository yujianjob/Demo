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

namespace Yunchee.Volkswagen.Utility.DataAccess.Query
{
    /// <summary>
    /// 复合表达式 
    /// </summary>
    public class ComplexCondition:IWhereCondition
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public ComplexCondition()
        {
        }
        #endregion

        #region 属性集
        /// <summary>
        /// 左边的表达式
        /// </summary>
        public IWhereCondition Left { get; set; }

        /// <summary>
        /// 右边的表达式
        /// </summary>
        public IWhereCondition Right { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public LogicalOperators Operator { get; set; }
        #endregion

        #region IWhereCondition 成员
        /// <summary>
        /// 获取where条件表达式字符串
        /// </summary>
        /// <returns></returns>
        public string GetExpression()
        {
            if (this.Left == null || this.Right == null)
                throw new ArgumentException("属性Left和Right都不能为null.");
            return string.Format("({0} {1} {2})",this.Left.GetExpression(),this.Operator == LogicalOperators.And?"and":"or",this.Right.GetExpression());
        }
        #endregion
    }
}
