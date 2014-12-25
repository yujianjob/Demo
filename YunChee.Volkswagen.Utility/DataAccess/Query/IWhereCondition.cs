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

namespace Yunchee.Volkswagen.Utility.DataAccess.Query
{
    /// <summary>
    /// where条件 接口 
    /// </summary>
    public interface IWhereCondition
    {
        /// <summary>
        /// 获取where条件表达式字符串
        /// </summary>
        /// <returns></returns>
        string GetExpression();
    }
}
