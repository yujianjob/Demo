using System;

namespace Yunchee.Volkswagen.Utility.ExtensionMethod
{
    public static class TransformExtension
    {
        #region int类型转换的方法
        /// <summary>
        /// int 类型转换的方法
        /// </summary>
        /// <param name="value">string 值</param>
        /// <returns>返回转换后的值</returns>
        public static int ToInt(this object value)
        {
            int result = 0;

            if (value != null)
            {
                return int.TryParse(value.ToString(), out result) ? result : 0;
            }

            return result;
        }
        #endregion

        #region datetime类型转换的方法
        /// <summary>
        /// datetime 类型转换的方法
        /// </summary>
        /// <param name="id">string 值</param>
        /// <returns>返回转换后的值</returns>
        public static DateTime ToDateTime(this string value)
        {
            DateTime result = DateTime.Now;

            if (!string.IsNullOrEmpty(value))
            {
                return DateTime.TryParse(value, out result) ? result : DateTime.Now;
            }

            return result;
        }
        #endregion

        #region decimal类型转换的方法
        /// <summary>
        /// decimal 类型转换的方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            decimal result = 0;

            if (!string.IsNullOrEmpty(value))
            {
                return decimal.TryParse(value, out result) ? result : 0;
            }

            return result;
        }
        #endregion
    }
}
