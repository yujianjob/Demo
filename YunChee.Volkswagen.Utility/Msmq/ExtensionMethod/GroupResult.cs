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

using System.Collections;
using System.Collections.Generic;

namespace Yunchee.Volkswagen.Utility.ExtensionMethod
{
    /// <summary>
    /// 分组查询结果 
    /// </summary>
    public class GroupResult
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public GroupResult()
        {
        }
        #endregion

        public object Key { get; set; }
        public int Count { get; set; }
        public IEnumerable Items { get; set; }
        public IEnumerable<GroupResult> SubGroups { get; set; }
        public override string ToString()
        {
            return string.Format("{0} ({1})", Key, Count);
        }
    }
}
