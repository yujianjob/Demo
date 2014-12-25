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
    /// 数据访问： 0502问卷问题表 QuesQuestion 
    /// 表QuesQuestion的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class QuesQuestionDAO : BaseDAO<BasicUserInfo>, ICRUDable<QuesQuestionEntity>, IQueryable<QuesQuestionEntity>
    {
        #region 根据问卷ID获取问卷题目列表

        /// <summary>
        /// 根据问卷ID获取问卷题目列表
        /// </summary>
        /// <param name="quesId">问卷ID</param>
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

        #region 删除问卷题目表

        /// <summary>
        /// 删除问卷题目表
        /// </summary>
        /// <param name="questionIds">题目ID集合  "1,2,3"</param>
        public void DeleteQuesQuestion(string questionIds)
        {
            var sql = new StringBuilder();

            //更新问卷回答表
            sql.AppendFormat(" UPDATE dbo.QuesAnswer SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE QuestionID IN ({0}); ", questionIds);
            //更新问题选项表
            sql.AppendFormat(" UPDATE dbo.QuesOption SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE QuestionID IN ({0}); ", questionIds);
            //更新问题表
            sql.AppendFormat(" UPDATE dbo.QuesQuestion SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ID IN ({0}); ", questionIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion
    }
}
