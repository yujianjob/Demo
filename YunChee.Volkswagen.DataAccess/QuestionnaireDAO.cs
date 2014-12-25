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
using System.Text;
using Yunchee.Volkswagen.DataAccess.Base;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess;

namespace Yunchee.Volkswagen.DataAccess
{
    /// <summary>
    /// 数据访问： 0501问卷表 Questionnaire 
    /// 表Questionnaire的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class QuestionnaireDAO : BaseDAO<BasicUserInfo>, ICRUDable<QuestionnaireEntity>, IQueryable<QuestionnaireEntity>
    {
        #region 获取分页权限列表

        /// <summary>
        /// 获取分页权限列表
        /// </summary>
        public PagedQueryObjectResult<DataSet> GetQuestionnaireList(PagedQueryEntity entity, string searchText)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            entity.TableName = "dbo.Questionnaire a";
            entity.QueryFieldName = "a.*";
            entity.QueryFieldName += " , QuestionCount = (SELECT COUNT(*) FROM dbo.QuesQuestion WHERE IsDelete = 0 AND QuestionnaireID = a.ID) ";
            entity.QueryCondition = " AND a.IsDelete = 0 ";
            entity.QueryCondition += string.Format(" AND a.ClientID = {0} ", this.CurrentUserInfo.ClientID);

            if (!string.IsNullOrEmpty(searchText))
            {
                entity.QueryCondition += string.Format(" AND (a.Name LIKE '%{0}%' OR a.[Description] LIKE '%{0}%') ", searchText);
            }

            if (entity.SortField.Equals("QuestionCount"))
            {
                entity.SortField = " (SELECT COUNT(*) FROM dbo.QuesQuestion WHERE IsDelete = 0 AND QuestionnaireID = a.ID) ";
            }

            result.RowCount = query.GetTotalCount(entity);
            result.Data = query.GetPagedData(entity);

            return result;
        }

        #endregion

        #region 删除问卷表

        /// <summary>
        /// 删除问卷表
        /// </summary>
        /// <param name="quesIds">问卷ID集合  "1,2,3"</param>
        public void DeleteQuestionnaire(string quesIds)
        {
            var sql = new StringBuilder();

            //更新问卷回答表
            sql.AppendFormat(" UPDATE dbo.QuesAnswer SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE QuestionID IN (SELECT QuestionID FROM QuesQuestion WHERE QuestionnaireID IN ({0})); ", quesIds);
            //更新问题选项表
            sql.AppendFormat(" UPDATE dbo.QuesOption SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE QuestionID IN (SELECT QuestionID FROM QuesQuestion WHERE QuestionnaireID IN ({0})); ", quesIds);
            //更新问题表
            sql.AppendFormat(" UPDATE dbo.QuesQuestion SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE QuestionnaireID IN ({0}); ", quesIds);
            //更新问卷表
            sql.AppendFormat(" UPDATE dbo.Questionnaire SET IsDelete = 1, ");
            sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
            sql.AppendFormat(" WHERE ID IN ({0}); ", quesIds);

            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region 根据问卷ID获取问卷信息

        /// <summary>
        /// 根据问卷ID获取问卷信息
        /// </summary>
        public DataSet GetQuestionnaireById(int questionnaireId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.Questionnaire WHERE ID = {0} ", questionnaireId);
            sql.AppendFormat(" SELECT * FROM dbo.QuesQuestion WHERE QuestionnaireID = {0} ORDER BY SortIndex ", questionnaireId);
            sql.AppendFormat(" SELECT * FROM dbo.QuesOption a WHERE a.QuestionID IN  ");
            sql.AppendFormat(" (SELECT ID FROM dbo.QuesQuestion WHERE QuestionnaireID = {0}) ORDER BY a.QuestionID, a.SortIndex ", questionnaireId);

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
        
        #region 根据客户ID获取问卷信息

        /// <summary>
        /// 根据客户ID获取问卷信息
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <returns></returns>
        public DataSet GetQuestionnaireByCustomerId(int customerId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.CustomerID, a.QuestionnaireID, b.Name, ");
            sql.AppendFormat(" SubmitDate = CONVERT(VARCHAR(10), MAX(a.CreateTime), 120) ");
            sql.AppendFormat(" FROM dbo.QuesAnswer a ");
            sql.AppendFormat(" INNER JOIN dbo.Questionnaire b ON a.QuestionnaireID = b.ID AND b.IsDelete = 0 ");
            sql.AppendFormat(" WHERE a.IsDelete = 0 AND b.ClientID = {0} ", this.CurrentUserInfo.ClientID);
            sql.AppendFormat(" AND a.CustomerID = {0} ", customerId);
            sql.AppendFormat(" GROUP BY a.CustomerID, a.QuestionnaireID, b.Name ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
