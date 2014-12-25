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

namespace Yunchee.Volkswagen.Utility.Entity
{
    /// <summary>
    /// 实体类的基类 
    /// </summary>
    [Serializable]
    public abstract class BaseEntity : ICloneable,IEntity
    {
        #region ICloneable 成员
        /// <summary>
        /// 实现深拷贝
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region IEntity 成员
        /// <summary>
        /// 持久化句柄
        /// </summary>
        public PersistenceHandle PersistenceHandle { get; set; }
        #endregion
    }
}
