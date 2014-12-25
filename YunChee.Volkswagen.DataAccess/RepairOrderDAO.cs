/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/7 19:54:36
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
using Yunchee.Volkswagen.Common.Const;

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// 数据访问： 0703预约维修表 RepairOrder 
    /// 表RepairOrder的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class RepairOrderDAO : BaseDAO<BasicUserInfo>, ICRUDable<RepairOrderEntity>, IQueryable<RepairOrderEntity>
    {
        #region 分页查询
        /// <summary>
        /// 获取分页预约维修列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">搜索文本</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetRepairOrderList(PagedQueryEntity pageEntity, string start, string end, RepairOrderEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"(
    SELECT  a.* ,
            b.Name AS CarTypeName ,--车型名称
            c.Name AS CarStyleName ,--车款名称
            d.Name AS GenderName ,--性别
            bb.Name AS IsHandleName ,-- 是否处理
            bc.Name AS IsSuccessName , --是否预约成功
            cc.Name AS DealerName,--经销商名称
            cc.ParentID AS ClientParentID -- 经销商ParentID
    FROM    dbo.RepairOrder AS a
            LEFT JOIN dbo.CarType AS b ON a.CarTypeID = b.ID
                                          AND b.IsDelete = 0
            LEFT JOIN dbo.CarStyle AS c ON a.CarStyleID = c.ID
                                           AND c.IsDelete = 0
            LEFT JOIN dbo.Client AS cc ON cc.ID = a.ClientID
                                          AND cc.IsDelete = 0
            LEFT JOIN dbo.BasicData AS d ON a.Gender = d.value
                                            AND d.IsDelete = 0
                                            AND d.TypeCode = '{0}'
            LEFT JOIN dbo.BasicData AS bb ON bb.Value = a.IsHandle
                                             AND bb.IsDelete = 0
                                             AND bb.TypeCode = '{1}'
            LEFT JOIN dbo.BasicData AS bc ON bc.Value = a.IsSuccess
                                             AND bc.IsDelete = 0
                                             AND bc.TypeCode = '{2}'
                  WHERE     a.IsDelete = 0
            ) AS t", E_BasicData.Gender.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString());

            pageEntity.QueryFieldName = string.Format("*");

            StringBuilder strcondition = new StringBuilder();
            if (queryEntity.CarTypeID != -1)//车型
                strcondition.AppendFormat(" AND CarTypeID={0}", queryEntity.CarTypeID);
            if (!string.IsNullOrEmpty(start))//提交时间
                strcondition.AppendFormat(" AND TargetTime>='{0}'", start);
            if (!string.IsNullOrEmpty(end))
                strcondition.AppendFormat(" AND TargetTime<DATEADD(d,1,'{0}')", end);
            if (queryEntity.Gender != "-1")//性别
                strcondition.AppendFormat(" AND Gender='{0}'", queryEntity.Gender);
            if (!string.IsNullOrEmpty(queryEntity.CustomerName))//客户姓名
                strcondition.AppendFormat(" AND CustomerName LIKE '%{0}%'", queryEntity.CustomerName);
            if (queryEntity.IsHandle != "-1")//是否处理
                strcondition.AppendFormat(" AND IsHandle = '{0}'", queryEntity.IsHandle);
            if (queryEntity.IsSuccess != "-1")//是否预约成功
                strcondition.AppendFormat(" AND IsSuccess = '{0}'", queryEntity.IsSuccess);

            if (this.CurrentUserInfo.ClientType == C_ClientType.DEALER)
                strcondition.AppendFormat(" AND ClientID={0}", this.CurrentUserInfo.ClientID);
            if (this.CurrentUserInfo.ClientType == C_ClientType.REGIONAL)
            {
                if (queryEntity.ClientID != -1)
                    strcondition.AppendFormat(" AND ClientID={0}", queryEntity.ClientID);
                else
                    strcondition.AppendFormat(" AND (ClientParentID={0} OR ClientID={0})", this.CurrentUserInfo.ClientID);
            }
            if (!string.IsNullOrEmpty(queryEntity.IDs))
                strcondition.AppendFormat(" AND id in {0}", queryEntity.IDs);
            pageEntity.QueryCondition = strcondition.ToString();


            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion

        #region 客户预约维修信息
        /// <summary>
        /// 预约维修ID
        /// </summary>
        /// <param name="roId"></param>
        /// <returns></returns>
        public RepairOrderEntity GetRepairOrderById(object roId)
        {
            var entity = new RepairOrderEntity();
            //参数检查
            if (roId == null)
                return null;
            string id = roId.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"
    SELECT  a.* ,
            b.Name AS CarTypeName ,--车型名称
            c.Name AS CarStyleName ,--车款名称
            d.Name AS GenderName ,--性别
            bb.Name AS IsHandleName ,-- 是否处理
            bc.Name AS IsSuccessName , --是否预约成功
            cc.ParentID AS ClientParentID -- 经销商ParentID
    FROM    dbo.RepairOrder AS a
            LEFT JOIN dbo.CarType AS b ON a.CarTypeID = b.ID
                                          AND b.IsDelete = 0
            LEFT JOIN dbo.CarStyle AS c ON a.CarStyleID = c.ID
                                           AND c.IsDelete = 0
            LEFT JOIN dbo.Client AS cc ON cc.ID = a.ClientID
                                          AND cc.IsDelete = 0
            LEFT JOIN dbo.BasicData AS d ON a.Gender = d.value
                                            AND d.IsDelete = 0
                                            AND d.TypeCode = '{1}'
            LEFT JOIN dbo.BasicData AS bb ON bb.Value = a.IsHandle
                                             AND bb.IsDelete = 0
                                             AND bb.TypeCode = '{2}'
            LEFT JOIN dbo.BasicData AS bc ON bc.Value = a.IsSuccess
                                             AND bc.IsDelete = 0
                                             AND bc.TypeCode = '{3}'
               WHERE     a.IsDelete = 0  AND a.ID={0}", id.ToString(), E_BasicData.Gender.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString());

            DataSet ds = this.SQLHelper.ExecuteDataset(sql.ToString());

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                entity = DataTableToObject.ConvertToObject<RepairOrderEntity>(ds.Tables[0].Rows[0]);
            }


            //返回
            return entity;
        }
        #endregion

        #region 获取车主预约维修列表(通过微信用户标识)

        /// <summary>
        /// 获取车主预约维修列表(通过微信用户标识)
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetCustomerRepairList(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT CustomerName,LicensePlateNumber,Phone, ");
            sql.AppendFormat("  CarTypeName = ( SELECT  name  FROM  dbo.CarType WHERE  id = CarTypeID AND IsDelete=0 ) ,");
            sql.AppendFormat("  CarStyleName = ( SELECT  name  FROM  dbo.CarStyle WHERE  id = CarStyleID AND IsDelete=0 ) ,");
            sql.AppendFormat("  SubmitTime=CONVERT(varchar(100),SubmitTime, 120),RepairReason,");
            sql.AppendFormat("  TargetTime=CONVERT(varchar(100),TargetTime, 120), ");//将时间转换成字符串格式，例如：2006-05-16 10:57:49
            sql.AppendFormat("  OrderStatus = CASE WHEN ( a.IsHandle = 0 AND a.IsSuccess = 0) THEN '处理中' ");//当既没处理又没预约成功时，客户查看时显示状态为“处理中”
            sql.AppendFormat("  WHEN ( a.IsHandle = 1AND a.IsSuccess = 0 ) THEN '预约失败' ");//当已经处理完，且预约状态为失败时，客户查看时显示状态为“预约失败”
            sql.AppendFormat("  ELSE '预约成功' END ");//只要数据库中的的预约状态为成功时，客户查看时显示状态为“预约成功”
            sql.AppendFormat(" FROM dbo.RepairOrder a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' AND a.IsDelete=0 ", OpenID);
            sql.AppendFormat(" ORDER BY a.TargetTime DESC ,a.CreateTime DESC");

            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
