/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/4/30 16:15:27
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
    /// ���ݷ��ʣ� 0503�ʾ�ѡ��� QuesOption 
    /// ��QuesOption�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class QuesOptionDAO : BaseDAO<BasicUserInfo>, ICRUDable<QuesOptionEntity>, IQueryable<QuesOptionEntity>
    {
        #region ������ĿID��ȡ��Ŀѡ���б�

        /// <summary>
        /// ������ĿID��ȡ��Ŀѡ���б�
        /// </summary>
        /// <param name="questionId">��ĿID</param>
        public DataSet GetQuesOptionListByQuestionId(int questionId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.* ");
            sql.AppendFormat(" ,TypeName = ( SELECT Name FROM dbo.BasicData WHERE IsDelete = 0 AND Value = a.Type ");
            sql.AppendFormat("  AND TypeCode = '{0}' ) ", E_BasicData.QuesOptionType.ToString());
            sql.AppendFormat(" FROM dbo.QuesOption a ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.QuestionID = {0} ", questionId);
            sql.AppendFormat(" ORDER BY a.SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region �����ʾ�ID��ȡ��Ŀѡ���б�

        /// <summary>
        /// �����ʾ�ID��ȡ��Ŀѡ���б�
        /// </summary>
        /// <param name="questionnaireId">�ʾ�ID</param>
        public DataSet GetQuesOptionListByQuestionnaireId(int questionnaireId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.* ");
            sql.AppendFormat(" FROM dbo.QuesOption a ");
            sql.AppendFormat(" INNER JOIN dbo.QuesQuestion b ON a.QuestionID = b.ID AND b.IsDelete = 0 ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.QuestionnaireID = {0} ", questionnaireId);
            sql.AppendFormat(" ORDER BY b.SortIndex, a.SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ɾ���ʾ�ѡ���

        /// <summary>
        /// ɾ���ʾ�ѡ���
        /// </summary>
        /// <param name="optionIds">ѡ��ID����  "1,2,3"</param>
        public void DeleteQuesOption(string optionIds)
        {
            var sql = new StringBuilder();

            //�����ʾ�ش��
            sql.AppendFormat(" UPDATE dbo.QuesAnswer SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE OptionID IN ({0}); ", optionIds);
            //��������ѡ���
            sql.AppendFormat(" UPDATE dbo.QuesOption SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ID IN ({0}); ", optionIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion
    }
}
