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

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// ���ݷ��ʣ� 0502�ʾ������ QuesQuestion 
    /// ��QuesQuestion�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class QuesQuestionDAO : BaseDAO<BasicUserInfo>, ICRUDable<QuesQuestionEntity>, IQueryable<QuesQuestionEntity>
    {
        #region �����ʾ�ID��ȡ�ʾ���Ŀ�б�

        /// <summary>
        /// �����ʾ�ID��ȡ�ʾ���Ŀ�б�
        /// </summary>
        /// <param name="quesId">�ʾ�ID</param>
        public DataSet GetQuesQuestionListByQuesId(int quesId)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.*, OptionCount = ( SELECT COUNT(*) FROM dbo.QuesOption WHERE QuestionID = a.ID AND IsDelete=0 ) ");
            sql.AppendFormat(" FROM dbo.QuesQuestion a ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND a.QuestionnaireID = {0} ", quesId);
            sql.AppendFormat(" ORDER BY a.SortIndex ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ɾ���ʾ���Ŀ��

        /// <summary>
        /// ɾ���ʾ���Ŀ��
        /// </summary>
        /// <param name="questionIds">��ĿID����  "1,2,3"</param>
        public void DeleteQuesQuestion(string questionIds)
        {
            var sql = new StringBuilder();

            //�����ʾ�ش��
            sql.AppendFormat(" UPDATE dbo.QuesAnswer SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE QuestionID IN ({0}); ", questionIds);
            //��������ѡ���
            sql.AppendFormat(" UPDATE dbo.QuesOption SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE QuestionID IN ({0}); ", questionIds);
            //���������
            sql.AppendFormat(" UPDATE dbo.QuesQuestion SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ID IN ({0}); ", questionIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion
    }
}
