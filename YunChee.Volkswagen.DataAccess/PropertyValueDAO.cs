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
    /// ���ݷ��ʣ� 0803����ֵ�� PropertyValue 
    /// ��PropertyValue�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class PropertyValueDAO : BaseDAO<BasicUserInfo>, ICRUDable<PropertyValueEntity>, IQueryable<PropertyValueEntity>
    {
        #region ɾ����չ����ֵ

        /// <summary>
        /// ɾ����չ����ֵ
        /// </summary>
        /// <param name="scope">��չ����������</param>
        /// <param name="objectId">����ID</param>
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
