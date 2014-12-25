/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/30 15:11:04
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
using Yunchee.Volkswagen.Common.Const;
using Yunchee.Volkswagen.Common.Enum;

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// ���ݷ��ʣ� 1401�һ���� RedeemCode 
    /// ��RedeemCode�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class RedeemCodeDAO : BaseDAO<BasicUserInfo>, ICRUDable<RedeemCodeEntity>, IQueryable<RedeemCodeEntity>
    {
        #region ��ҳ��ѯ
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">�����ı�</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetRedeemCodeList(PagedQueryEntity pageEntity, string start,string end, RedeemCodeEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"(
SELECT  a.* ,
        ClientName = d.Name ,
        GenderName = e.Name ,
		d.Phone,
        RedeemFlagName = b.Name ,
        IsValidName = c.Name
FROM    dbo.RedeemCode AS a
        LEFT JOIN dbo.BasicData AS b ON a.RedeemFlag = b.value
                                        AND b.IsDelete = 0
                                        AND b.TypeCode = '{0}'
        LEFT JOIN dbo.BasicData AS c ON a.IsValid = c.value
                                        AND c.IsDelete = 0
                                        AND c.TypeCode = '{0}'
        LEFT JOIN dbo.Customer AS d ON d.WxOpenId = a.WxOpenId
                                       AND d.WxId = a.WxId
                                       AND d.IsDelete = 0
									   LEFT JOIN dbo.BasicData AS e ON e.Value=d.WxSex AND e.IsDelete=0 AND e.TypeCode='{1}'
WHERE   a.IsDelete = 0
        --AND a.IsValid = 1
        AND DATEDIFF(d, ExpirationTime, GETDATE()) <= 0) AS t", E_BasicData.YesOrNo.ToString(), E_BasicData.Gender.ToString());
            pageEntity.QueryFieldName = "*";
           

            pageEntity.SortField = pageEntity.SortField;
            pageEntity.SortDirection = pageEntity.SortDirection;

            StringBuilder strcondition = new StringBuilder();

            if (queryEntity.RedeemFlag != "-1")
            {
                strcondition.AppendFormat(" AND RedeemFlag = '{0}'", queryEntity.RedeemFlag);
            }
            if (queryEntity.IsValid != "-1")
            {
                strcondition.AppendFormat(" AND IsValid = '{0}'", queryEntity.IsValid);
            }
            if (!string.IsNullOrEmpty(start))
            {
                strcondition.AppendFormat(" AND GetTime >= '{0}'", start);
            }
            if (!string.IsNullOrEmpty(end))
            {
                strcondition.AppendFormat(" AND GetTime <= '{0}'", end);
            }

            if (!string.IsNullOrEmpty(queryEntity.Code.Trim()))
            {
                strcondition.AppendFormat(" AND t.Code LIKE '%{0}%'",queryEntity.Code.Trim());
            }

            pageEntity.QueryCondition = strcondition.ToString();

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion

        #region ���Ķһ���ĵȼ�(ͨ��΢��ʶ���)

        /// <summary>
        /// ���Ķһ���ĵȼ�
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public void UpdateRedemmCodeLevel(string OpenID,int EventID,int number)
        {
            //����¼ҵ���������3��ʱ���ͱ�ʾ��Ȼ�Ǳ���ע��ʱ�Ķһ���ȼ��������޸Ĳ���   
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RedeemCode SET  "); 
            sql.AppendFormat("  IsValid = '0', ");            
            sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
            sql.AppendFormat(" WHERE WxOpenId='{0}' ", OpenID);               
            sql.AppendFormat(" AND Level<{0}  ",number);
            //sql.AppendFormat(" AND RedeemFlag='0'  ");
            sql.AppendFormat(" AND (EventID={0}  OR EventID=0) ", EventID);
            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region ͨ��΢��ʶ��Ż�ȡ�һ�������

        /// <summary>
        /// ͨ��΢��ʶ��Ż�ȡ�һ�������
        /// </summary>
        /// <returns></returns>
        public DataSet GetStatuByOpenID(string OpenID,int EventID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.RedeemCode a  ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}'  ",OpenID);
            sql.AppendFormat(" AND a.IsDelete=0 and a.RedeemFlag='1' ");
            sql.AppendFormat(" AND a.EventID={0} ",EventID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ͨ��΢��ʶ��Ż�ȡ��Ϸ��Ķһ�������

        /// <summary>
        /// ͨ��΢��ʶ��Ż�ȡ��Ϸ��Ķһ�������
        /// </summary>
        /// <returns></returns>
        public DataSet GetRedeemByOpenID(string OpenID, int EventID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP 1 * FROM dbo.RedeemCode a  ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}'  ", OpenID);
            sql.AppendFormat(" AND a.IsDelete=0 and a.RedeemFlag='0' ");
            sql.AppendFormat(" AND a.EventID={0} ", EventID);
            sql.AppendFormat(" ORDER BY Level desc ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ���Ķһ������Ч״̬

        /// <summary>
        /// ���Ķһ������Ч״̬
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public void UpdateRedemmCodeIsValid(int rid)
        {
            //����¼ҵ���������3��ʱ���ͱ�ʾ��Ȼ�Ǳ���ע��ʱ�Ķһ���ȼ��������޸Ĳ���   
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.RedeemCode SET IsValid='0' WHERE ID in  ");
            sql.AppendFormat("  (SELECT ID FROM dbo.RedeemCode WHERE WxOpenId=(SELECT WxOpenId FROM dbo.RedeemCode WHERE ID={0}) ",rid);
            sql.AppendFormat("  AND (EventID=(SELECT EventID FROM dbo.RedeemCode WHERE ID={0}) OR EventID=0) AND IsDelete=0 ) ", rid);
            this.SQLHelper.ExecuteNonQuery(sql.ToString());
        }

        #endregion

        #region ������Ϸ�һ������Ч״̬

        /// <summary>
        /// ������Ϸ�һ������Ч״̬
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public void UpdateRedemmIsValidByLevel(string openID,string eventID,int level)
        {
            if (eventID != null && eventID.Trim() != "")
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" UPDATE dbo.RedeemCode SET IsValid='0' WHERE ID in  ");
                sql.AppendFormat("  (SELECT ID FROM dbo.RedeemCode WHERE WxOpenId='{0}' ", openID);
                sql.AppendFormat("  AND EventID={0} AND IsDelete=0  ", eventID.ToInt());
                sql.AppendFormat("  AND Level<{0} )", level);
                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region �ж��Ƿ�ע���

        /// <summary>
        /// �ж��Ƿ�ע���
        /// </summary>
        /// <returns></returns>
        public DataSet GetRegisterByOpenID(string OpenID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * FROM dbo.RedeemCode a  ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}'  ", OpenID);
            sql.AppendFormat(" AND a.IsDelete=0 and a.Level=1 ");
            
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ͨ��΢��ʶ����Լ��һ�״̬��ȡ�һ�������

        /// <summary>
        /// ͨ��΢��ʶ��Ż�ȡ�һ�������
        /// </summary>
        /// <returns></returns>
        public DataSet GetRedeemCodeByOpenID(string OpenID, int RedeemFlag)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT b.code,b.EventName,b.Description,b.RedeemFlag,  ");
            sql.AppendFormat(" b.GetTime,b.RedeemTime,b.ExpirationTime,  ");
            sql.AppendFormat(" IsValid = (CASE WHEN b.IsValid='1' AND b.ischeck1='1' THEN '1' ELSE '0' END) FROM  ");
            sql.AppendFormat(" (SELECT code,EventName,Description=Remark,RedeemFlag,  ");
            sql.AppendFormat("  ischeck1=CASE WHEN ExpirationTime>=GETDATE() THEN '1' ELSE '0' END,  ");
            sql.AppendFormat("  IsValid,  ");
            sql.AppendFormat("  GetTime=CONVERT(varchar(100),GetTime, 120),  ");
            sql.AppendFormat("  RedeemTime=CONVERT(varchar(100),RedeemTime, 120),  ");
            sql.AppendFormat("  ExpirationTime=CONVERT(varchar(100),ExpirationTime, 120)  ");
            sql.AppendFormat("  FROM dbo.RedeemCode a  ");           
            sql.AppendFormat(" WHERE a.WxOpenId='{0}'  ", OpenID);
            sql.AppendFormat(" AND a.IsDelete=0 ");
            if (RedeemFlag==1)
                sql.AppendFormat(" AND a.RedeemFlag='1' ");
            if (RedeemFlag == 2)
                sql.AppendFormat(" AND a.RedeemFlag='0' ");
            sql.AppendFormat(" ) b ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

    }
}
