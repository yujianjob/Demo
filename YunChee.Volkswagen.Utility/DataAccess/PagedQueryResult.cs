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

using System;
using Yunchee.Volkswagen.Utility.Entity;

namespace Yunchee.Volkswagen.Utility.DataAccess
{
    /// <summary>
    /// 分页查询结果 
    /// </summary>
    [Serializable]
    public class PagedQueryResult<T> where T:IEntity
    {
        /// <summary>
        /// 结果数组
        /// </summary>
        public T[] Entities { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int RowCount { get; set; }
    }
}
