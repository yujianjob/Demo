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

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// ���ݷ��ʣ� 0301��ϵ�� CarBrand 
    /// ��CarBrand�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CarBrandDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarBrandEntity>, IQueryable<CarBrandEntity>
    {
        #region ��ȡ��ҳȨ���б�

        /// <summary>
        /// ��ȡ��ҳȨ���б�
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetCarBrandList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.CarBrand";
            entity.QueryFieldName = "*";
            entity.QueryCondition = " AND IsDelete = 0 ";
            //entity.QueryCondition += string.Format("ORDER BY CreateTime ASC ");
            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (Name LIKE '%{0}%' OR EnglishName LIKE '%{0}%') ", searchText);
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region ɾ����ϵ

        /// <summary>
        /// ɾ����ϵ
        /// </summary>
        /// <param name="quesIds">��ϵID����  "1,2,3"</param>
        public void DeleteCarBrand(string quesIds)
        {
            if (!string.IsNullOrEmpty(quesIds))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.CarBrand SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", quesIds);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region ���ݳ�ϵID��ȡ��ϵ��Ϣ

        /// <summary>
        /// ���ݳ�ϵID��ȡ��ϵ��Ϣ
        /// </summary>
        public DataSet GetCarBrandByCustomerId(int carBrandId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.CarBrand WHERE ID = {0} AND IsDelete = 0", carBrandId);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
