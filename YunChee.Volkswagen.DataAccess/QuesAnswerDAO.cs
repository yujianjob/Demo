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
    /// 数据访问： 0504问卷回答表 QuesAnswer 
    /// 表QuesAnswer的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class QuesAnswerDAO : BaseDAO<BasicUserInfo>, ICRUDable<QuesAnswerEntity>, IQueryable<QuesAnswerEntity>
    {
        #region 新增问卷回答表

        /// <summary>
        /// 新增问卷回答表
        /// </summary>
        /// <param name="answerList">问卷回答集合</param>
        /// <param name="questionnaireId">问卷ID</param>
        public void InsertQuesAnswer(List<QuesAnswerEntity> answerList, int questionnaireId)
        {
            if (answerList.Count > 0)
            {
                var sql = new StringBuilder();

                // 批量增加
                answerList.ForEach(a =>
                {
                    sql.AppendFormat(" INSERT INTO dbo.QuesAnswer ");
                    sql.AppendFormat(" (QuestionID ,OptionID ,OptionValue , QuestionnaireID, ");
                    sql.AppendFormat("  CreateBy ,CreateTime ,LastUpdateBy ,LastUpdateTime ,IsDelete) ");
                    sql.AppendFormat(" VALUES ( ");
                    sql.AppendFormat(" (SELECT QuestionID FROM dbo.QuesOption WHERE ID = {0}), ", a.OptionID);
                    sql.AppendFormat(" {0}, '{1}', {2}, ", a.OptionID, a.OptionValue, questionnaireId);
                    sql.AppendFormat(" {0}, GETDATE(), {0}, GETDATE(), 0", this.CurrentUserInfo.UserID);
                    sql.AppendFormat(" ) ");
                });

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion
    }
}
