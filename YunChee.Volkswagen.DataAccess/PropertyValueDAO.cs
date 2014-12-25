/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/5/15 14:34:03
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

using System.Text;
using Yunchee.Volkswagen.Common.Enum;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess;

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// 数据访问： 0803属性值表 PropertyValue 
    /// 表PropertyValue的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class PropertyValueDAO : BaseDAO<BasicUserInfo>, ICRUDable<PropertyValueEntity>, IQueryable<PropertyValueEntity>
    {
        #region 删除扩展属性值

        /// <summary>
        /// 删除扩展属性值
        /// </summary>
        /// <param name="scope">扩展属性作用域</param>
        /// <param name="objectId">对象ID</param>
        /// <returns></returns>
        public void DeletePropertyValue(E_PropertyScope scope, int objectId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" DELETE FROM dbo.PropertyValue WHERE ID IN  ");
            sql.AppendFormat(" ( ");
            sql.AppendFormat(" 	 SELECT a.ID FROM dbo.PropertyValue a ");
            sql.AppendFormat(" 	 INNER JOIN dbo.Property b ON a.PropertyID = b.ID ");
            sql.AppendFormat("   WHERE b.ClientID = {0} ", this.CurrentUserInfo.ClientID);
            sql.AppendFormat("   AND b.Scope = '{0}' ", scope.GetHashCode());
            sql.AppendFormat("   AND a.ObjectID = {0} ", objectId);
            sql.AppendFormat(" ) ");

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion
    }
}
