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

using System.Data;
using System.Text;
using Yunchee.Volkswagen.Common.Enum;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// ���ݷ��ʣ� 0801���Ա� Property 
    /// ��Property�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class PropertyDAO : BaseDAO<BasicUserInfo>, ICRUDable<PropertyEntity>, IQueryable<PropertyEntity>
    {
        #region ��ȡ��չ����ֵ

        /// <summary>
        /// ��ȡ��չ����ֵ
        /// </summary>
        /// <param name="scope">��չ����������</param>
        /// <param name="objectId">����ID</param>
        /// <returns></returns>
        public DataSet GetPropertyValue(E_PropertyScope scope, int objectId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.*, b.PropertyValue FROM dbo.Property a  ");
            sql.AppendFormat(" INNER JOIN dbo.PropertyValue b ON a.ID = b.PropertyID AND b.IsDelete = 0 ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.ClientID = {0} ", this.CurrentUserInfo.ClientID);
            sql.AppendFormat(" AND a.Scope = '{0}' ", scope.GetHashCode());
            sql.AppendFormat(" AND b.ObjectID = {0} ", objectId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
