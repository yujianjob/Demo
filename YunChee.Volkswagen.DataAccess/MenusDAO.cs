/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/4/17 13:36:56
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

using System.Data;
using System.Text;
using Yunchee.Volkswagen.DataAccess.Base;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess;

namespace Yunchee.Volkswagen.DataAccess
{
    /// <summary>
    /// 数据访问： 0901菜单表 Menus 
    /// 表Menus的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class MenusDAO : BaseDAO<BasicUserInfo>, ICRUDable<MenusEntity>, IQueryable<MenusEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetMenusList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Menus";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Name LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 获取指定客户的菜单列表

        /// <summary>
        /// 获取指定客户的菜单列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetMenusListByClientId()
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.*, ViewPowerName = b.Name ");
            sql.AppendFormat(" FROM dbo.Menus a ");
            sql.AppendFormat(" LEFT JOIN dbo.Powers b ON a.ViewPowerID = b.ID AND b.IsDelete = 0 ");
            sql.AppendFormat(" INNER JOIN dbo.ClientMenus c ON a.ID = c.MenusID AND c.IsDelete = 0 ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 ");
            sql.AppendFormat(" AND c.ClientID = {0} ", this.CurrentUserInfo.ClientID);
            sql.AppendFormat(" ORDER BY a.SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取指定客户的菜单列表

        /// <summary>
        /// 获取指定客户的菜单列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetMenusListById(string id)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.*, ViewPowerName = b.Name ");
            sql.AppendFormat(" FROM dbo.Menus a ");
            sql.AppendFormat(" LEFT JOIN dbo.Powers b ON a.ViewPowerID = b.ID AND b.IsDelete = 0 ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 ");

            if (!string.IsNullOrEmpty(id))
            {
                sql.AppendFormat(" AND a.ID = {0} ", id);
            }

            sql.AppendFormat(" ORDER BY a.SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
