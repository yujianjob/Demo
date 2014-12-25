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

namespace Yunchee.Volkswagen.Utility.ExtensionMethod
{
    /// <summary>
    /// Guid的扩展方法 
    /// </summary>
    public static class GuidExtensionMethods
    {
        /// <summary>
        /// 扩展方法：获取GUID的文本值,不带连字符
        /// </summary>
        /// <param name="pCaller">调用者</param>
        /// <returns></returns>
        public static string ToText(this Guid pCaller)
        {
            return pCaller.ToString("N");
        }
    }
}
