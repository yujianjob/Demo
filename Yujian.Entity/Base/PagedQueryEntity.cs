using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.Entity
{
    /// <summary>
    /// 分页查询实体
    /// </summary>
    public class PagedQueryEntity
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 要显示的字段列表，查询全部传递 *
        /// </summary>
        public string QueryFieldName { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string QueryCondition { get; set; }

        /// <summary>
        /// 当前显示页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示项数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序方向("ASC", "DESC")
        /// </summary>
        public string SortDirection { get; set; }
    }
}
