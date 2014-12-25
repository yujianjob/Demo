/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/20 10:54:10
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
    /// ���ݷ��ʣ� 0303�����ص��  CarTypeFeature 
    /// ��CarTypeFeature�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CarTypeFeatureDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarTypeFeatureEntity>, IQueryable<CarTypeFeatureEntity>
    {
        #region ��ҳ��ѯ
        /// <summary>
        /// ��ȡ��ҳ�����ص��б�
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetCarTypeFeatureList(PagedQueryEntity pageEntity, CarTypeFeatureEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format("dbo.CarTypeFeature");
            pageEntity.QueryFieldName =
            string.Format("*");
            pageEntity.QueryCondition = string.Format("AND IsDelete = 0 AND CarTypeID = {0} ",queryEntity.CarTypeID);//ɾ��״̬����ʾ            
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion

        #region ��ȡ�����ص��б�(���ݳ���ID)

        /// <summary>
        /// ��ȡ�����ص��б�(���ݳ���ID)
        /// </summary>
        /// <param name="carTypeID">����ID</param>
        /// <returns></returns>
        public DataSet GetCarTypeFeatureList(int carTypeID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT FeatureID=id,Title,ImageUrl,Description,CarTypeID ");
            sql.AppendFormat(" FROM dbo.CarTypeFeature ");
            sql.AppendFormat(" WHERE IsDelete=0 ");
            sql.AppendFormat(" AND CarTypeID = {0} ", carTypeID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
