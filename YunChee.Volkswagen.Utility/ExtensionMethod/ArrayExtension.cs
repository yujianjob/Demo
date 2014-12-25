using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yunchee.Volkswagen.Utility.ExtensionMethod
{
    /// <summary>
    /// 数组扩展方法
    /// </summary>
    public static class ArrayExtension
    {
        #region 判断数组非空

        /// <summary>
        /// 数组是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(this object[] value) 
        {
            if (value != null && value.Length > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
