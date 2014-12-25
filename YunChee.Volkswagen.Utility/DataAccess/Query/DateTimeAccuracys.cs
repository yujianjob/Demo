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
    /// 时间精度 
    /// </summary>
    public enum DateTimeAccuracys
    {
        /// <summary>
        /// 日期,丢弃了时、分、秒、毫秒
        /// <remarks>
        /// <para>例如:</para>
        /// <para>2011-11-2</para>
        /// </remarks>
        /// </summary>
        Date
        ,
        /// <summary>
        /// 时间,完整的时间格式
        /// </summary>
        DateTime
    }
}
