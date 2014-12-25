/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/1 10:14:29
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
    /// 数据访问： 0701预约试驾表 TestDriveOrder 
    /// 表TestDriveOrder的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class TestDriveOrderDAO : BaseDAO<BasicUserInfo>, ICRUDable<TestDriveOrderEntity>, IQueryable<TestDriveOrderEntity>
    {
        #region 分页查询
        /// <summary>
        /// 获取分页车款列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">搜索文本</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetTestDriveOrderList(PagedQueryEntity pageEntity, string start, string end, TestDriveOrderEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"
        ( SELECT    a.ID ,
						a.CarTypeID,
                        b.Name AS CarTypeName ,
                        CustomerName ,
                        GenderName = ba.Name ,
                        Gender,
                        Phone ,
                        PlanBuyTime ,
						PlanBuyTimeName=bd.Name,
                        CustomerRemark ,
                        SubmitTime ,
                        TargetTime ,
						IsHandle,
                        IsHandleName = bb.Name ,
						IsSuccess,
                        IsSuccessName = bc.Name ,
                        a.Remark ,
                        DealerName= c.Name,
                        c.ID AS ClientID ,
                        c.ParentID AS ClientParentID
              FROM      dbo.TestDriveOrder AS a
                        LEFT JOIN dbo.CarType AS b ON a.CarTypeID = b.ID
                                                      AND b.IsDelete = 0
                        LEFT JOIN dbo.Client AS c ON c.ID = a.ClientID
                                                     AND c.IsDelete = 0
                        LEFT JOIN dbo.BasicData AS ba ON ba.Value = a.Gender
                                                         AND ba.IsDelete = 0
                                                         AND ba.TypeCode = '{0}'
                        LEFT JOIN dbo.BasicData AS bb ON bb.Value = a.IsHandle
                                                         AND bb.IsDelete = 0
                                                         AND bb.TypeCode = '{1}'
                        LEFT JOIN dbo.BasicData AS bc ON bc.Value = a.IsSuccess
                                                         AND bc.IsDelete = 0
                                                         AND bc.TypeCode = '{2}'
                        LEFT JOIN dbo.BasicData AS bd ON bd.Value = a.PlanBuyTime
                                                         AND bd.IsDelete = 0
                                                         AND bd.TypeCode = '{3}'
              WHERE     a.IsDelete = 0
            ) AS t", E_BasicData.Gender.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.PlanBuyTime.ToString());

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
            if (queryEntity.PlanBuyTime != "-1")//计划购车时间
                strcondition.AppendFormat(" AND PlanBuyTime='{0}'", queryEntity.PlanBuyTime);

            if (this.CurrentUserInfo.ClientType == C_ClientType.DEALER)
                strcondition.AppendFormat(" AND ClientID={0}", this.CurrentUserInfo.ClientID);
            if (this.CurrentUserInfo.ClientType == C_ClientType.REGIONAL)
            {
                if (queryEntity.ClientID != -1)
                    strcondition.AppendFormat(" AND ClientID={0}", queryEntity.ClientID);
                else
                    strcondition.AppendFormat(" AND (ClientParentID={0} OR ClientID={0})", this.CurrentUserInfo.ClientID);
            }

            pageEntity.QueryCondition = strcondition.ToString();


            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }

        public DataTable TestDriveOrderTable(string start, string end, TestDriveOrderEntity queryEntity)
        {
            StringBuilder strcondition = new StringBuilder();
            strcondition.AppendFormat(@"SELECT * FROM
            ( SELECT    a.ID ,
						a.CarTypeID,
                        b.Name AS CarTypeName ,
                        CustomerName ,
                        GenderName = ba.Name ,
                        Gender,
                        Phone ,
                        PlanBuyTime ,
						PlanBuyTimeName=bd.Name,
                        CustomerRemark ,
                        SubmitTime ,
                        TargetTime ,
						IsHandle,
                        IsHandleName = bb.Name ,
						IsSuccess,
                        IsSuccessName = bc.Name ,
                        a.Remark ,
                        DealerName= c.Name,
                        c.ID AS ClientID ,
                        c.ParentID AS ClientParentID
              FROM      dbo.TestDriveOrder AS a
                        LEFT JOIN dbo.CarType AS b ON a.CarTypeID = b.ID
                                                      AND b.IsDelete = 0
                        LEFT JOIN dbo.Client AS c ON c.ID = a.ClientID
                                                     AND c.IsDelete = 0
                        LEFT JOIN dbo.BasicData AS ba ON ba.Value = a.Gender
                                                         AND ba.IsDelete = 0
                                                         AND ba.TypeCode = '{0}'
                        LEFT JOIN dbo.BasicData AS bb ON bb.Value = a.IsHandle
                                                         AND bb.IsDelete = 0
                                                         AND bb.TypeCode = '{1}'
                        LEFT JOIN dbo.BasicData AS bc ON bc.Value = a.IsSuccess
                                                         AND bc.IsDelete = 0
                                                         AND bc.TypeCode = '{2}'
                        LEFT JOIN dbo.BasicData AS bd ON bd.Value = a.PlanBuyTime
                                                         AND bd.IsDelete = 0
                                                         AND bd.TypeCode = '{3}'
              WHERE     a.IsDelete = 0
            ) AS t WHERE 1=1 ", E_BasicData.Gender.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.PlanBuyTime.ToString());
            if (queryEntity.CarTypeID != -1)//车型
                strcondition.AppendFormat(" AND CarTypeID={0}", queryEntity.CarTypeID);
            if (!string.IsNullOrEmpty(start))//提交时间
                strcondition.AppendFormat(" AND TargetTime>='{0}'", start);
            if (!string.IsNullOrEmpty(end))
                strcondition.AppendFormat(" AND TargetTime<=DATEADD(d,1,'{0}')", end);
            if (queryEntity.Gender != "-1")//性别
                strcondition.AppendFormat(" AND Gender='{0}'", queryEntity.Gender);
            if (!string.IsNullOrEmpty(queryEntity.CustomerName))//客户姓名
                strcondition.AppendFormat(" AND CustomerName LIKE '%{0}%'", queryEntity.CustomerName);
            if (queryEntity.PlanBuyTime != "-1")//计划购车时间
                strcondition.AppendFormat(" AND PlanBuyTime='{0}'", queryEntity.PlanBuyTime);

            if (this.CurrentUserInfo.ClientType == C_ClientType.DEALER)
                strcondition.AppendFormat(" AND ClientID={0}", this.CurrentUserInfo.ClientID);
            if (this.CurrentUserInfo.ClientType == C_ClientType.REGIONAL)
            {
                if (queryEntity.ClientID != -1)
                    strcondition.AppendFormat(" AND ClientID={0}", queryEntity.ClientID);
                else
                    strcondition.AppendFormat(" AND (ClientParentID={0} OR ClientID={0})", this.CurrentUserInfo.ClientID);
            }

            return SQLHelper.ExecuteDataset(strcondition.ToString()).Tables[0];

        }
        #endregion

        #region 客户预约试驾信息

        /// <summary>
        /// 预约试驾ID
        /// </summary>
        /// <param name="tdoId"></param>
        /// <returns></returns>
        public TestDriveOrderEntity GetTestDriveOrderById(object tdoId)
        {
            var entity = new TestDriveOrderEntity();
            //参数检查
            if (tdoId == null)
                return null;
            string id = tdoId.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"
            SELECT    a.ID ,
						a.CarTypeID,
                        b.Name AS CarTypeName ,
                        CustomerName ,
                        Gender,
                        GenderName = ba.Name ,
                        Phone ,
                        PlanBuyTime ,
						PlanBuyTimeName=bd.Name,
                        CustomerRemark ,
                        SubmitTime ,
                        TargetTime ,
						IsHandle,
                        IsHandleName = bb.Name ,
						IsSuccess,
                        IsSuccessName = bc.Name ,
                        a.Remark ,
                        c.ID AS ClientID ,
                        c.ParentID AS ClientParentID
              FROM      dbo.TestDriveOrder AS a
                        LEFT JOIN dbo.CarType AS b ON a.CarTypeID = b.ID
                                                      AND b.IsDelete = 0
                        LEFT JOIN dbo.Client AS c ON c.ID = a.ClientID
                                                     AND c.IsDelete = 0
                        LEFT JOIN dbo.BasicData AS ba ON ba.Value = a.Gender
                                                         AND ba.IsDelete = 0
                                                         AND ba.TypeCode = '{1}'
                        LEFT JOIN dbo.BasicData AS bb ON bb.Value = a.IsHandle
                                                         AND bb.IsDelete = 0
                                                         AND bb.TypeCode = '{2}'
                        LEFT JOIN dbo.BasicData AS bc ON bc.Value = a.IsSuccess
                                                         AND bc.IsDelete = 0
                                                         AND bc.TypeCode = '{3}'
                        LEFT JOIN dbo.BasicData AS bd ON bd.Value = a.PlanBuyTime
                                                         AND bd.IsDelete = 0
                                                         AND bd.TypeCode = '{4}'
               WHERE     a.IsDelete = 0 and a.ID={0}", id.ToString(), E_BasicData.Gender.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.PlanBuyTime.ToString());

            DataSet ds = this.SQLHelper.ExecuteDataset(sql.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                entity = DataTableToObject.ConvertToObject<TestDriveOrderEntity>(ds.Tables[0].Rows[0]);
            }

            ////读取数据
            //using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            //{
            //    while (rdr.Read())
            //    {

            //            if (rdr["ID"] != DBNull.Value)
            //            {
            //                entity.ID = Convert.ToInt32(rdr["ID"]);
            //            }
            //            if (rdr["CarTypeID"] != DBNull.Value)
            //            {
            //                entity.CarTypeID = Convert.ToInt32(rdr["CarTypeID"]);
            //            }
            //            if (rdr["CustomerName"] != DBNull.Value)
            //            {
            //                entity.CustomerName = Convert.ToString(rdr["CustomerName"]);
            //            }
            //            if (rdr["Gender"] != DBNull.Value)
            //            {
            //                entity.Gender = Convert.ToString(rdr["Gender"]);
            //            }
            //            if (rdr["Phone"] != DBNull.Value)
            //            {
            //                entity.Phone = Convert.ToString(rdr["Phone"]);
            //            }
            //            if (rdr["PlanBuyTime"] != DBNull.Value)
            //            {
            //                entity.PlanBuyTime = Convert.ToString(rdr["PlanBuyTime"]);
            //            }
            //            if (rdr["CustomerRemark"] != DBNull.Value)
            //            {
            //                entity.CustomerRemark = Convert.ToString(rdr["CustomerRemark"]);
            //            }
            //            if (rdr["SubmitTime"] != DBNull.Value)
            //            {
            //                entity.SubmitTime = Convert.ToDateTime(rdr["SubmitTime"]);
            //            }
            //            if (rdr["TargetTime"] != DBNull.Value)
            //            {
            //                entity.TargetTime = Convert.ToDateTime(rdr["TargetTime"]);
            //            }
            //            if (rdr["IsHandle"] != DBNull.Value)
            //            {
            //                entity.IsHandle = Convert.ToString(rdr["IsHandle"]);
            //            }
            //            if (rdr["IsSuccess"] != DBNull.Value)
            //            {
            //                entity.IsSuccess = Convert.ToString(rdr["IsSuccess"]);
            //            }
            //            if (rdr["Remark"] != DBNull.Value)
            //            {
            //                entity.Remark = Convert.ToString(rdr["Remark"]);
            //            }
            //            if (rdr["ClientID"] != DBNull.Value)
            //            {
            //                entity.ClientID = Convert.ToInt32(rdr["ClientID"]);
            //            }
            //            if (rdr["CarTypeName"] != DBNull.Value)
            //            {
            //                entity.CarTypeName = rdr["CarTypeName"].ToString();
            //            }
            //            if (rdr["GenderName"] != DBNull.Value)
            //            {
            //                entity.GenderName = rdr["GenderName"].ToString();
            //            }
            //            if (rdr["PlanBuyTimeName"] != DBNull.Value)
            //            {
            //                entity.PlanBuyTimeName = rdr["PlanBuyTimeName"].ToString();
            //            }
            //            if (rdr["IsHandleName"] != DBNull.Value)
            //            {
            //                entity.IsHandleName = rdr["IsHandleName"].ToString();
            //            }
            //            if (rdr["IsSuccessName"] != DBNull.Value)
            //            {
            //                entity.IsSuccessName = rdr["IsSuccessName"].ToString();
            //            }
            //        break;
            //    }
            //}
            //返回
            return entity;
        }
        #endregion

        #region 获取预约试驾订单(通过微信用户标识)

        /// <summary>
        /// 获取预约试驾订单(通过微信用户标识)
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        /// <param name="CarTypeID">车型ID</param>
        public DataSet GetCountByRequest(string OpenID, string CarTypeID, string Phone, string CustomerName)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.TestDriveOrder a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' AND IsDelete=0 ", OpenID);
            if (CarTypeID != null && CarTypeID.Trim() != "" && CarTypeID != "请选择")
            sql.AppendFormat(" AND CarTypeID={0} ", CarTypeID.ToInt());
            sql.AppendFormat(" AND CustomerName='{0}' ", CustomerName);
            sql.AppendFormat(" AND Phone='{0}' ", Phone);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region 获取预约试驾订单(通过微信)

        /// <summary>
        /// 获取预约试驾订单(通过微信用户标识)
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetTestDriveByRequest(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT CustomerName ,");
            sql.AppendFormat("  CarTypeName = ( SELECT  name  FROM  dbo.CarType WHERE  id = CarTypeID ) ,");
            sql.AppendFormat("  TargetTime=CONVERT(varchar(100),TargetTime, 120), ");//将时间转换成字符串格式，例如：2006-05-16 10:57:49
            sql.AppendFormat("  OrderStatus = CASE WHEN ( a.IsHandle = 0 AND a.IsSuccess = 0) THEN '处理中' ");//当既没处理又没预约成功时，客户查看时显示状态为“处理中”
            sql.AppendFormat("  WHEN ( a.IsHandle = 1 AND a.IsSuccess = 0 ) THEN '处理中' ");//当已经处理完，且预约状态为失败时，客户查看时显示状态为“预约失败”
            sql.AppendFormat("  ELSE '成功' END ");//只要数据库中的的预约状态为成功时，客户查看时显示状态为“预约成功”   按照要求，都改成处理中的状态，然后录入数据库中的值，是默认就是处理，和已成功状态
            sql.AppendFormat(" FROM dbo.TestDriveOrder a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' AND a.IsDelete=0 ", OpenID);
            sql.AppendFormat(" ORDER BY a.TargetTime DESC ,a.CreateTime DESC");

            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        public DataSet GetUnHandleCustomer(string SqlText)
        {
            return this.SQLHelper.ExecuteDataset(SqlText);
        }

        public DataSet GetCustomerOrderType(string SqlText)
        {
            return this.SQLHelper.ExecuteDataset(SqlText);
        }

        public DataSet GetOrderIds(string SqlText)
        {
            return this.SQLHelper.ExecuteDataset(SqlText);
        }
    }
}

