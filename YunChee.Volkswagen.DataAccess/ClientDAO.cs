/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/11 9:49:44
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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.Entity;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using Yunchee.Volkswagen.Utility.DataAccess;
using Yunchee.Volkswagen.Utility.Log;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility.DataAccess.Query;
using Yunchee.Volkswagen.DataAccess.Base;
using Yunchee.Volkswagen.Entity.Query;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// 数据访问： 0909企业客户信息 Client 
    /// 表Client的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ClientDAO : BaseDAO<BasicUserInfo>, ICRUDable<ClientEntity>, IQueryable<ClientEntity>
    {
        #region 获取分页权限区域列表

        /// <summary>
        /// 获取分页权限区域列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetRegionlList(PagedQueryEntity pageEntity, ClientEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = " dbo.Client a ";
            pageEntity.TableName += " LEFT JOIN dbo.CarBrand b ON a.CarBrandID = b.ID AND b.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.BasicData c ON a.RegionPartition = c.Value AND c.IsDelete=0  AND c.TypeCode='RegionPartition' ";


            pageEntity.QueryFieldName = " a.* ";    
            pageEntity.QueryFieldName += " , CarBrandName = b.Name ";
            pageEntity.QueryFieldName += " , RegionPartitionName = c.Name ";
            pageEntity.QueryCondition = " AND a.IsDelete = 0 AND a.Type = 1";
            pageEntity.SortField = "a." + pageEntity.SortField;
            if (pageEntity.SortField.Equals("a.CarBrandName"))
                pageEntity.SortField = "b.Name";
            if (pageEntity.SortField.Equals("a.RegionPartitionName"))
                pageEntity.SortField = "c.Name";

            if (queryEntity.CarBrandID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND a.CarBrandID = {0} ", queryEntity.CarBrandID);
            }
            if (int.Parse(queryEntity.RegionPartition) != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND (a.RegionPartition={0}) ", queryEntity.RegionPartition);
            }
            if (!string.IsNullOrEmpty(queryEntity.Code))
            {
                pageEntity.QueryCondition += string.Format(" AND (a.Code LIKE '%{0}%') ", queryEntity.Code);
            }
            if (!string.IsNullOrEmpty(queryEntity.Name))
            {
                pageEntity.QueryCondition += string.Format(" AND ( a.Name LIKE '%{0}%') ", queryEntity.Name);
            }
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;
        }

        #endregion

        #region 获取分页权限经销商列表

        /// <summary>
        /// 获取分页权限经销商列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetBusinessList(PagedQueryEntity pageEntity, ClientEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = " dbo.Client a ";
            pageEntity.TableName += " LEFT JOIN dbo.Client a1 ON a.ParentID = a1.ID AND a1.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.Region b ON a.ProvinceID = b.ID AND b.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.Region c ON a.CityID = c.ID AND c.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.Region d ON a.DistrictID = d.ID AND d.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.BasicData e ON a.DealerRegion = e.Value AND e.IsDelete=0  AND e.TypeCode='DealerRegion' ";


            pageEntity.QueryFieldName = " a.* ";
            pageEntity.QueryFieldName += " , ParentName = a1.Name ";
            pageEntity.QueryFieldName += " , ProvinceName = b.Name ";
            pageEntity.QueryFieldName += " , CityName = c.Name ";
            pageEntity.QueryFieldName += " , DistrictName = d.Name ";
            pageEntity.QueryFieldName += " , DealerRegionName = e.Name ";
            pageEntity.QueryCondition = " AND a.IsDelete = 0 AND a.Type = 2";
            pageEntity.SortField = "a."+pageEntity.SortField;
            if (pageEntity.SortField.Equals("a.ParentName"))
                pageEntity.SortField = "a1.Name";
            if (pageEntity.SortField.Equals("a.ProvinceName"))
                pageEntity.SortField = "b.Name";

            if (queryEntity.ProvinceID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND a.ProvinceID = {0} ", queryEntity.ProvinceID);
            }
            if (queryEntity.CityID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND a.CityID = {0} ", queryEntity.CityID);
            }
            if (queryEntity.DistrictID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND a.DistrictID = {0} ", queryEntity.DistrictID);
            }
            if (queryEntity.ParentID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND a.ParentID = {0} ", queryEntity.ParentID);
            }
            if (!string.IsNullOrEmpty(queryEntity.Code))
            {
                pageEntity.QueryCondition += string.Format(" AND (a.Code LIKE '%{0}%') ", queryEntity.Code);
            }
            if (!string.IsNullOrEmpty(queryEntity.Name))
            {
                pageEntity.QueryCondition += string.Format(" AND ( a.Name LIKE '%{0}%') ", queryEntity.Name);
            }
            if (queryEntity.DealerRegion!="-1")
            {
                pageEntity.QueryCondition += string.Format(" AND  a.DealerRegion = '{0}'", queryEntity.DealerRegion);
            }
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;
        }

        #endregion

        #region 获取经销商列表

        /// <summary>
        /// 获取经销商列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetBusinessLists(PagedQueryEntity pageEntity, ClientEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = " dbo.Client a ";
            pageEntity.TableName += " LEFT JOIN dbo.Client a1 ON a.ParentID = a1.ID AND a1.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.Region b ON a.ProvinceID = b.ID AND b.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.Region c ON a.CityID = c.ID AND c.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.Region d ON a.DistrictID = d.ID AND d.IsDelete = 0 ";
            pageEntity.TableName += " LEFT JOIN dbo.BasicData e ON a.DealerRegion = e.Value AND e.IsDelete=0  AND e.TypeCode='DealerRegion' ";
            pageEntity.QueryFieldName = " a.* ";
            pageEntity.QueryFieldName += " , ParentName = a1.Name ";
            pageEntity.QueryFieldName += " , ProvinceName = b.Name ";
            pageEntity.QueryFieldName += " , CityName = c.Name ";
            pageEntity.QueryFieldName += " , DistrictName = d.Name ";
            pageEntity.QueryFieldName += " , DealerRegionName = e.Name ";
            pageEntity.QueryCondition = " AND a.IsDelete = 0 AND a.Type = 2";
            pageEntity.SortField = "a." + pageEntity.SortField;
            if (pageEntity.SortField.Equals("a.ParentName"))
                pageEntity.SortField = "a1.Name";
            if (pageEntity.SortField.Equals("a.ProvinceName"))
                pageEntity.SortField = "b.Name";

            if (queryEntity.ProvinceID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND a.ProvinceID = {0} ", queryEntity.ProvinceID);
            }
            if (queryEntity.CityID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND a.CityID = {0} ", queryEntity.CityID);
            }
            if (queryEntity.DistrictID != -1)
            {
                pageEntity.QueryCondition += string.Format(" AND a.DistrictID = {0} ", queryEntity.DistrictID);
            }
            if (!string.IsNullOrEmpty(queryEntity.Code))
            {
                pageEntity.QueryCondition += string.Format(" AND (a.Code LIKE '%{0}%') ", queryEntity.Code);
            }
            if (!string.IsNullOrEmpty(queryEntity.Name))
            {
                pageEntity.QueryCondition += string.Format(" AND ( a.Name LIKE '%{0}%') ", queryEntity.Name);
            }
            if (queryEntity.DealerRegion != "-1")
            {
                pageEntity.QueryCondition += string.Format(" AND  a.DealerRegion = '{0}'", queryEntity.DealerRegion);
            }
            pageEntity.QueryCondition += string.Format(" AND  a.ParentID = '{0}'", this.CurrentUserInfo.ClientID);

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);

            return result;
        }

        #endregion

        #region 删除区域,经销商

        /// <summary>
        /// 删除区域,经销商
        /// </summary>
        /// <param name="quesIds">区域,经销商ID集合  "1,2,3"</param>
        public void DeleteRegionl(string quesIds)
        {
            if (!string.IsNullOrEmpty(quesIds))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Client SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", quesIds);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 根据区域ID获取区域,经销商信息

        /// <summary>
        /// 根据区域ID获取区域,经销商信息
        /// </summary>
        public DataSet GetRegionlByCustomerId(int carBrandId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.Client WHERE ID = {0} ", carBrandId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取距离最近的经销商信息

        /// <summary>
        /// 获取距离最近的经销商信息
        /// </summary>
        public DataSet GetDistance(string Longitude, string Latitude)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP 1 DealerName=b.Name,b.ServiceHotline,DealerLongitude=b.Longitude,DealerLatitude=b.Latitude FROM ");
            sql.AppendFormat(" (SELECT a.*, ABS(dbo.DistanceTwoPoints({0}, {1}, a.Longitude, a.Latitude )) distance  ", Longitude,Latitude);
            sql.AppendFormat(" FROM dbo.Client a) b  ORDER BY b.distance ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

    }
}
