/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/18 12:07:10
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
using Yunchee.Volkswagen.Common.Enum;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// ���ݷ��ʣ� 0302���ͱ� CarType 
    /// ��CarType�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CarTypeDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarTypeEntity>, IQueryable<CarTypeEntity>
    {
        #region ��ҳ��ѯ
        /// <summary>
        /// ��ȡ��ҳ�����б�
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">�����ı�</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetCarTypeList(PagedQueryEntity pageEntity, string searchText, CarTypeEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"dbo.CarType AS a
            LEFT JOIN dbo.CarBrand AS b ON a.CarBrandID = b.ID
            LEFT JOIN dbo.BasicData AS c ON a.IsShow = c.Value AND c.IsDelete=0  AND c.TypeCode = '{0}'
            LEFT JOIN dbo.BasicData AS d ON a.IsSale = d.Value AND d.IsDelete=0  AND d.TypeCode = '{0}'",E_BasicData.YesOrNo.ToString());
            pageEntity.QueryFieldName =
            string.Format(@"a.ID ,
            a.SortIndex ,
            b.Name as BranchName ,
            a.Name ,
            a.EnglishName ,
            IsShow=c.Name ,
            IsSale=d.Name,
            a.Remark,a.Subsidies");
            pageEntity.QueryCondition = string.Format("AND a.IsDelete = 0 ");//ɾ��״̬����ʾ

            pageEntity.SortField = "a." + pageEntity.SortField;
            if (pageEntity.SortField.Equals("a.BranchName"))
                pageEntity.SortField = "b.Name";

            if (queryEntity.CarBrandID != -1)
            {
                pageEntity.QueryCondition += string.Format("AND a.CarBrandID = {0}", queryEntity.CarBrandID);
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                pageEntity.QueryCondition += string.Format(@"AND ( a.Name LIKE '%{0}%'
                  OR a.EnglishName LIKE '%{0}%'
                )", searchText);
            }
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion

        #region ɾ������
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="quesIds">����ID����  "1,2,3"</param>
        public void DeleteCarBrand(string quesIds)
        {
            if (!string.IsNullOrEmpty(quesIds))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.CarType SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", quesIds);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }
        #endregion

        #region ��ȡ�����б�(���ݳ�ϵID)

        /// <summary>
        /// ��ȡ�����б�(���ݳ�ϵID)
        /// </summary>
        /// <param name="carBrandID">��ϵID</param>
        /// <returns></returns>
        public DataSet GetCarTypeListByCarBrandId(int carBrandID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT CarTypeID = a.ID, CarTypeName = a.Name, a.EnglishName, a.CarBrandID, ");
            sql.AppendFormat(" ImageUrl = (SELECT TOP 1 b.ImageUrl FROM dbo.ObjectImages b ");
            sql.AppendFormat(" 			   WHERE IsDelete = 0 AND ObjectType = '1' AND b.ObjectID = a.ID ");
            sql.AppendFormat(" 			   ORDER BY b.SortIndex) ");
            sql.AppendFormat(" FROM dbo.CarType a WHERE a.IsDelete = 0 AND a.IsShow = '1'");
            sql.AppendFormat(" AND CarBrandID = {0} ", carBrandID);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡ�����б�

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataSet GetCarTypeList()
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT CarTypeID = a.ID, CarTypeName = a.Name, a.EnglishName, a.CarBrandID, ");
            sql.AppendFormat(" ImageUrl = (SELECT TOP 1 b.ImageUrl FROM dbo.ObjectImages b ");
            sql.AppendFormat(" 			   WHERE IsDelete = 0 AND ObjectType = '1' AND b.ObjectID = a.ID ");
            sql.AppendFormat(" 			   ORDER BY b.SortIndex) ");
            sql.AppendFormat(" FROM dbo.CarType a WHERE a.IsDelete = 0 AND a.IsShow = '1'");
           
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡ���ͶԱ��б�(΢���û���ʶ)

        /// <summary>
        /// ��ȡ���ͶԱ��б�(΢���û���ʶ)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataSet GetCarTypeCompareList(string openID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT CarStyleID, ");
            sql.AppendFormat(" CarStyleName=(SELECT b.Name FROM dbo.CarStyle b WHERE b.id= a.CarStyleID AND b.IsDelete=0), Selected,");
            sql.AppendFormat(" CarTypeName=(SELECT c.Name FROM dbo.CarType c WHERE c.id= (SELECT b.CarTypeID FROM dbo.CarStyle b WHERE b.id= a.CarStyleID AND b.IsDelete=0) AND c.IsDelete=0)");
            sql.AppendFormat(" FROM CustomerCarStyle a ");
            sql.AppendFormat(" WHERE a.IsDelete=0 ");
            sql.AppendFormat(" AND a.CustomerID=(SELECT ID FROM Customer WHERE WxOpenId='{0}' AND IsDelete=0) ", openID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
