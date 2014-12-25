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
    /// ���ݷ��ʣ� 0806����ͼƬ�� ObjectImages 
    /// ��ObjectImages�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ObjectImagesDAO : BaseDAO<BasicUserInfo>, ICRUDable<ObjectImagesEntity>, IQueryable<ObjectImagesEntity>
    {
        #region ��ҳ��ѯ
        /// <summary>
        /// ��ȡ��ҳ����ͼƬ�б�
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetCarTypeIamgeList(PagedQueryEntity pageEntity,  ObjectImagesEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format("dbo.ObjectImages");
            pageEntity.QueryFieldName =
            string.Format("*");
            pageEntity.QueryCondition = string.Format("AND IsDelete = 0 AND ObjectID = {0} AND ObjectType = {1} ",queryEntity.ObjectID,queryEntity.ObjectType);//ɾ��״̬����ʾ            
            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion
    }
}
