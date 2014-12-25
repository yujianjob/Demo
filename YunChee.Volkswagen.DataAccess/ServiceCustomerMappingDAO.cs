/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/9 21:08:37
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
using Yunchee.Volkswagen.Entity.Interface.Request;

namespace Yunchee.Volkswagen.DataAccess
{
    
    /// <summary>
    /// 数据访问： 1108客服与客户关系表 ServiceCustomerMapping 
    /// 表ServiceCustomerMapping的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class ServiceCustomerMappingDAO : BaseDAO<BasicUserInfo>, ICRUDable<ServiceCustomerMappingEntity>, IQueryable<ServiceCustomerMappingEntity>
    {
        #region 分页查询
        /// <summary>
        /// 获取分页历史消息列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">搜索文本</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetSCMList(PagedQueryEntity pageEntity,string clientId, string searchText, string timeSpan, ServiceCustomerMappingEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"
          ( SELECT    a.* ,
                    MessageType = b.Name ,
                    c.WxNickName ,
                    u.ChineseName ,
                    cc.ID AS ClientID ,
                    cc.ParentID AS ClientParentID , -- 经销商ParentID
                    ISNULL(STUFF(( SELECT   ',' + cu.ChineseName
                                   FROM     dbo.ServiceCustomerMapping AS scm
                                            LEFT JOIN dbo.Users AS cu ON cu.WxOpenId = scm.ServiceOpenId
                                   WHERE    scm.BusinessID = a.BusinessID
                                            AND scm.BusinessType = a.BusinessType
                                            AND SessionStatus = 0
                                 FOR
                                   XML PATH('')
                                 ), 1, 1, ''), '') AS HisService
          FROM      dbo.ServiceCustomerMapping AS a
                    LEFT JOIN dbo.BasicData AS b ON b.Value = a.BusinessType
                                                    AND b.IsDelete = 0
                                                    AND b.TypeCode = 'BusinessType'
                    LEFT JOIN dbo.Customer AS c ON a.CustomerOpenId = c.WxOpenId
                                                   AND c.IsDelete = 0
                    LEFT JOIN dbo.Users AS u ON u.WxOpenId = a.ServiceOpenId
                                                AND u.IsDelete = 0
                    LEFT JOIN dbo.Client AS cc ON c.ClientID = cc.ID
                                                  AND cc.IsDelete = 0
        ) AS t", E_BasicData.BusinessType.ToString());

            pageEntity.QueryFieldName = string.Format("*");

            StringBuilder strcondition = new StringBuilder();
            strcondition.Append(" AND IsDelete = 0");
            if (!string.IsNullOrEmpty(searchText))
            {
                strcondition.AppendFormat("AND WxNickName like '%{0}%'", searchText);
            }

            if (queryEntity.BusinessType != "-1")
            {
                strcondition.AppendFormat("AND BusinessType = '{0}'", queryEntity.BusinessType);
            }

            if (this.CurrentUserInfo.ClientType == C_ClientType.DEALER)
                strcondition.AppendFormat(" AND ClientID={0}", this.CurrentUserInfo.ClientID);
            if (this.CurrentUserInfo.ClientType == C_ClientType.REGIONAL)
            {
                if (clientId != "-1")
                    strcondition.AppendFormat(" AND ClientID={0}", clientId);
                else
                    strcondition.AppendFormat(" AND (ClientParentID={0} OR ClientID={0})", this.CurrentUserInfo.ClientID);
            }

            if (timeSpan != "-1")
            {
                strcondition.AppendFormat(" AND DATEDIFF(d,t.AcceptTime,GETDATE())<={0}",timeSpan);
            }

            pageEntity.QueryCondition = strcondition.ToString();


            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }
        #endregion

        #region 判断客服是哪种类型(通过客服微信识别号)

        /// <summary>
        /// 判断客服是哪种类型(通过客服微信识别号)
        /// </summary>
        /// <param name="OpenID">客服微信识别号</param>
        public DataSet GetServiceStatus(string OpenID,int ClientID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.Roles a, ");
            sql.AppendFormat(" ( SELECT   *  FROM     dbo.RoleUsers ");
            sql.AppendFormat("               WHERE    UserID = ( SELECT   ID ");
            sql.AppendFormat("                                    FROM     dbo.Users ");
            sql.AppendFormat("                                    WHERE    WxOpenId = '{0}'",OpenID);
            sql.AppendFormat("                                             AND ClientID={0} ",ClientID);
            sql.AppendFormat("                                            AND IsDelete = 0 ");
            sql.AppendFormat("                                 ) AND RoleUsers.IsDelete=0 ");
            sql.AppendFormat("  ) b WHERE a.ID=b.RoleID ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取售后客服在线消息(通过客服微信识别号)

        /// <summary>
        /// 获取售后客服在线消息(通过客服微信识别号)
        /// </summary>
        /// <param name="OpenID">客服微信识别号</param>
        public DataSet GetAfterSaleServiceMessage()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT CustomerOpenId=a.CustomerOpenid ,BusinessType=a.MessageType , ");
            sql.AppendFormat(" BusinessTypeName=(SELECT Name FROM dbo.BasicData WHERE TypeCode='BusinessType' AND Value=a.MessageType), ");
            sql.AppendFormat(" BusinessID=a.OrderID , ");
            sql.AppendFormat(" AcceptTime=CONVERT(varchar(100),a.CreateTime, 120), ");
            sql.AppendFormat(" DateDiffs=cast(DATEDIFF(MINUTE,a.CreateTime,GETDATE())/60 AS varchar(20))+'时'+cast(DATEDIFF(MINUTE,a.CreateTime,GETDATE())%60 AS varchar(20))+'分',");
            sql.AppendFormat(" b.WxNickName,b.WxHeadImgUrl,CustomerName=b.Name ");
            sql.AppendFormat("  FROM    dbo.ServiceWaitQueue a JOIN dbo.Customer b ON a.CustomerOpenid = b.WxOpenId ");
            sql.AppendFormat("  WHERE Status = 1 ");
            sql.AppendFormat("  AND a.MessageType IN ('1','2','5') ");
            sql.AppendFormat("  AND a.IsDelete=0 AND b.IsDelete=0 ");
            sql.AppendFormat("  ORDER  BY b.CreateTime ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取销售客服在线消息(通过客服微信识别号)

        /// <summary>
        /// 获取销售客服在线消息(通过客服微信识别号)
        /// </summary>
        /// <param name="OpenID">客服微信识别号</param>
        public DataSet GetSaleServiceMessage()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT CustomerOpenId=a.CustomerOpenid ,BusinessType=a.MessageType , ");
            sql.AppendFormat(" BusinessTypeName=(SELECT Name FROM dbo.BasicData WHERE TypeCode='BusinessType' AND Value=a.MessageType), ");
            sql.AppendFormat(" BusinessID=a.OrderID , ");
            sql.AppendFormat(" AcceptTime=CONVERT(varchar(100),a.CreateTime, 120), ");
            sql.AppendFormat(" DateDiffs=cast(DATEDIFF(MINUTE,a.CreateTime,GETDATE())/60 AS varchar(20))+'时'+cast(DATEDIFF(MINUTE,a.CreateTime,GETDATE())%60 AS varchar(20))+'分',");
            sql.AppendFormat(" b.WxNickName,b.WxHeadImgUrl,CustomerName=b.Name ");
            sql.AppendFormat("  FROM    dbo.ServiceWaitQueue a JOIN dbo.Customer b ON a.CustomerOpenid = b.WxOpenId ");
            sql.AppendFormat("  WHERE Status = 1 ");
            sql.AppendFormat("  AND a.MessageType IN ('3','4','5') ");
            sql.AppendFormat("  AND a.IsDelete=0 AND b.IsDelete=0 ");
            sql.AppendFormat("  ORDER  BY b.CreateTime ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取售后客服在线消息总数(通过客服微信识别号)

        /// <summary>
        /// 获取售后客服在线消息(通过客服微信识别号)
        /// </summary>
        /// <param name="OpenID">客服微信识别号</param>
        public int GetAfterSaleServiceCount()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT Count(*) ");
            sql.AppendFormat("  FROM    dbo.ServiceWaitQueue a JOIN dbo.Customer b ON a.CustomerOpenid = b.WxOpenId ");
            sql.AppendFormat("  WHERE Status = 1 ");
            sql.AppendFormat("  AND a.MessageType IN ('1','2','5') ");
            sql.AppendFormat("  AND a.IsDelete=0 AND b.IsDelete=0 ");
     
            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取销售客服在线消息总数(通过客服微信识别号)

        /// <summary>
        /// 获取销售客服在线消息总数(通过客服微信识别号)
        /// </summary>
        /// <param name="OpenID">客服微信识别号</param>
        public int GetSaleServiceCount()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT Count(*) ");
            sql.AppendFormat("  FROM    dbo.ServiceWaitQueue a JOIN dbo.Customer b ON a.CustomerOpenid = b.WxOpenId ");
            sql.AppendFormat("  WHERE Status = 1 ");
            sql.AppendFormat("  AND a.MessageType IN ('3','4','5') ");
            sql.AppendFormat("  AND a.IsDelete=0 AND b.IsDelete=0 ");

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        #endregion

        #region 获取售后历史消息列表(通过客服微信识别号)

        /// <summary>
        /// 获取售后历史消息列表(通过客服微信识别号)
        /// </summary>
        /// <param name="OpenID">客服微信识别号</param>
        public DataSet GetAfterSaleHistoryMessage(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT f.CustomerOpenId,g.WxNickName,CustomerName=g.Name,g.WxHeadImgUrl,f.MaintenanceCount,f.RepairCount,f.ConsultCount,AcceptTime=CONVERT(varchar(100),f.AcceptTime, 120)  ");
            sql.AppendFormat("  FROM dbo.Customer g JOIN  ");
            sql.AppendFormat(" (SELECT  CustomerOpenId ,ISNULL(MAX("+"[1]"+"),0) AS MaintenanceCount ,ISNULL(MAX("+"[2]"+"),0) AS RepairCount , ");
            sql.AppendFormat("          ISNULL(MAX("+"[3]"+"),0) AS TestDriverCount ,ISNULL(MAX("+"[4]"+"),0) AS AskPriceCount , ");
            sql.AppendFormat("          ISNULL(MAX(" + "[5]" + "),0) AS ConsultCount ,MAX(e.acceptime) AS AcceptTime ");
            sql.AppendFormat("  FROM    ( SELECT    * ");
            sql.AppendFormat("            FROM      ( SELECT  CustomerOpenId , BusinessType , COUNT(BusinessType) AS valueCount ,MAX(AcceptTime) AS acceptime ");
            sql.AppendFormat("                        FROM    dbo.ServiceCustomerMapping ");
            sql.AppendFormat("                        WHERE   CustomerOpenId != ''  AND  IsDelete=0 AND   ServiceOpenId='{0}'",OpenID);
            sql.AppendFormat("                        GROUP BY  CustomerOpenId , BusinessType");
            sql.AppendFormat("                       ) c PIVOT ( MAX(valueCount) FOR BusinessType IN ( "+"[1]"+", "+"[2]"+","+"[3]"+", "+"[4]"+", "+"[5]"+" ) ) a");
            sql.AppendFormat("          ) e");
            sql.AppendFormat("  GROUP BY e.CustomerOpenId) f ON g.WxOpenId =f.CustomerOpenId ");
            sql.AppendFormat("  AND g.IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取销售历史消息列表(通过客服微信识别号)

        /// <summary>
        /// 获取销售历史消息列表(通过客服微信识别号)
        /// </summary>
        /// <param name="OpenID">客服微信识别号</param>
        public DataSet GetSaleHistoryMessage(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT f.CustomerOpenId,g.WxNickName,CustomerName=g.Name,g.WxHeadImgUrl,f.TestDriverCount,f.AskPriceCount,f.ConsultCount,AcceptTime=CONVERT(varchar(100),f.AcceptTime, 120)  ");
            sql.AppendFormat("  FROM dbo.Customer g JOIN  ");
            sql.AppendFormat(" (SELECT  CustomerOpenId ,ISNULL(MAX(" + "[1]" + "),0) AS MaintenanceCount ,ISNULL(MAX(" + "[2]" + "),0) AS RepairCount , ");
            sql.AppendFormat("          ISNULL(MAX(" + "[3]" + "),0) AS TestDriverCount ,ISNULL(MAX(" + "[4]" + "),0) AS AskPriceCount , ");
            sql.AppendFormat("          ISNULL(MAX(" + "[5]" + "),0) AS ConsultCount ,MAX(e.acceptime) AS AcceptTime ");
            sql.AppendFormat("  FROM    ( SELECT    * ");
            sql.AppendFormat("            FROM      ( SELECT  CustomerOpenId , BusinessType , COUNT(BusinessType) AS valueCount ,MAX(AcceptTime) AS acceptime ");
            sql.AppendFormat("                        FROM    dbo.ServiceCustomerMapping ");
            sql.AppendFormat("                        WHERE   CustomerOpenId != ''  AND  IsDelete=0 AND   ServiceOpenId='{0}'", OpenID);
            sql.AppendFormat("                        GROUP BY  CustomerOpenId , BusinessType");
            sql.AppendFormat("                       ) c PIVOT ( MAX(valueCount) FOR BusinessType IN ( " + "[1]" + ", " + "[2]" + "," + "[3]" + ", " + "[4]" + ", " + "[5]" + " ) ) a");
            sql.AppendFormat("          ) e");
            sql.AppendFormat("  GROUP BY e.CustomerOpenId) f ON g.WxOpenId =f.CustomerOpenId ");
            sql.AppendFormat("  AND g.IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取预约保养订单

        /// <summary>
        /// 获取预约保养订单
        /// </summary>
        public DataSet GetMaintenanceOrder(string ServiceOpenID, string CustomerOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  BusinessID, ");
            sql.AppendFormat("  CarStyleName=(SELECT Name FROM dbo.CarStyle WHERE ID=b.CarStyleID AND IsDelete=0), ");
            sql.AppendFormat("  CarTypeName=(SELECT Name FROM dbo.CarType WHERE ID= b.CarTypeID AND IsDelete=0), ");
            sql.AppendFormat("  TargetTime= CONVERT(varchar(100),b.TargetTime, 120), ");
            sql.AppendFormat("  SubmitTime= CONVERT(varchar(100),b.SubmitTime, 120) ");
            sql.AppendFormat("  FROM dbo.ServiceCustomerMapping a JOIN dbo.MaintenanceOrder b ON  a.BusinessID =b.ID ");
            sql.AppendFormat("  WHERE ServiceOpenId='{0}'  ", ServiceOpenID);
            sql.AppendFormat("   AND CustomerOpenId='{0}'  ", CustomerOpenID);
            sql.AppendFormat("   AND BusinessType='1'  ");
            //sql.AppendFormat("   AND HandleStatus='1' ");
            sql.AppendFormat("   AND b.IsDelete=0 AND a.IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取预约维修订单

        /// <summary>
        /// 获取预约维修订单
        /// </summary>
        public DataSet GetRepairOrder(string ServiceOpenID, string CustomerOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  BusinessID, ");
            sql.AppendFormat("  CarStyleName=(SELECT Name FROM dbo.CarStyle WHERE ID=b.CarStyleID AND IsDelete=0), ");
            sql.AppendFormat("  CarTypeName=(SELECT Name FROM dbo.CarType WHERE ID= b.CarTypeID AND IsDelete=0), ");
            sql.AppendFormat("  TargetTime= CONVERT(varchar(100),b.TargetTime, 120), ");
            sql.AppendFormat("  SubmitTime= CONVERT(varchar(100),b.SubmitTime, 120) ");
            sql.AppendFormat("  FROM dbo.ServiceCustomerMapping a JOIN dbo.RepairOrder b ON  a.BusinessID =b.ID ");
            sql.AppendFormat("  WHERE ServiceOpenId='{0}'  ", ServiceOpenID);
            sql.AppendFormat("   AND CustomerOpenId='{0}'  ", CustomerOpenID);
            sql.AppendFormat("   AND BusinessType='2'  ");
            //sql.AppendFormat("   AND HandleStatus='1' ");
            sql.AppendFormat("   AND b.IsDelete=0 AND a.IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取预约试驾订单

        /// <summary>
        /// 获取预约试驾订单
        /// </summary>
        public DataSet GetTestDriverOrder(string ServiceOpenID, string CustomerOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  BusinessID, ");
           
            sql.AppendFormat("  CarTypeName=(SELECT Name FROM dbo.CarType WHERE ID= b.CarTypeID AND IsDelete=0), ");
            sql.AppendFormat("  TargetTime= CONVERT(varchar(100),b.TargetTime, 120), ");
            sql.AppendFormat("  SubmitTime= CONVERT(varchar(100),b.SubmitTime, 120) ");
            sql.AppendFormat("  FROM dbo.ServiceCustomerMapping a JOIN dbo.TestDriveOrder b ON  a.BusinessID =b.ID ");
            sql.AppendFormat("  WHERE ServiceOpenId='{0}'  ", ServiceOpenID);
            sql.AppendFormat("   AND CustomerOpenId='{0}'  ", CustomerOpenID);
            sql.AppendFormat("   AND BusinessType='3' ");
            //sql.AppendFormat("   AND HandleStatus='1' ");
            sql.AppendFormat("   AND b.IsDelete=0 AND a.IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取在线询价订单

        /// <summary>
        /// 获取在线询价订单
        /// </summary>
        public DataSet GetAskPriceOrder(string ServiceOpenID, string CustomerOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  BusinessID, ");
            sql.AppendFormat("  CarStyleName=(SELECT Name FROM dbo.CarStyle WHERE ID=b.CarStyleID AND IsDelete=0), ");
            sql.AppendFormat("  CarTypeName=(SELECT Name FROM dbo.CarType WHERE ID= b.CarTypeID AND IsDelete=0), ");
            sql.AppendFormat("  SubmitTime= CONVERT(varchar(100),b.SubmitTime, 120) ");
            sql.AppendFormat("  FROM dbo.ServiceCustomerMapping a JOIN dbo.ServiceAskPrice b ON  a.BusinessID =b.ID ");
            sql.AppendFormat("  WHERE ServiceOpenId='{0}'  ", ServiceOpenID);
            sql.AppendFormat("   AND CustomerOpenId='{0}'  ", CustomerOpenID);
            sql.AppendFormat("   AND BusinessType='4' ");
            //sql.AppendFormat("   AND HandleStatus='1' ");
            sql.AppendFormat("   AND b.IsDelete=0 AND a.IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取预约试驾订单详情

        /// <summary>
        /// 获取预约试驾订单详情
        /// </summary>
        public DataSet GetTestDriverOrderInfo(int BusinessID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT ");
            sql.AppendFormat("  CarTypeName=(SELECT Name FROM dbo.CarType WHERE id=a.CarTypeID AND IsDelete=0), ");
            sql.AppendFormat("  CustomerName,Phone, ");
            sql.AppendFormat("  WxNickName=(SELECT WxNickName FROM dbo.Customer WHERE WxOpenId=a.WxOpenId AND IsDelete=0), ");
            sql.AppendFormat("  PlanBuyTimeValue=(SELECT name FROM dbo.BasicData WHERE TypeCode='PlanBuyTime' AND value=a.PlanBuyTime AND IsDelete=0), ");
            sql.AppendFormat("  TargetTime=CONVERT(VARCHAR(100), a.TargetTime, 120), ");
            sql.AppendFormat("  OrderStatus=(CASE WHEN IsHandle=0 AND IsSuccess=0 THEN '1'   ");
            sql.AppendFormat("   WHEN IsHandle=1 AND IsSuccess=1 THEN '2' ELSE '3' END)  ");
            sql.AppendFormat("    FROM dbo.TestDriveOrder a ");
            sql.AppendFormat("   WHERE a.id={0} ",BusinessID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取预约维修订单详情

        /// <summary>
        /// 获取预约维修订单详情
        /// </summary>
        public DataSet GetRepairOrderInfo(int BusinessID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT ");
            sql.AppendFormat("  CarTypeName=(SELECT Name FROM dbo.CarType WHERE id=a.CarTypeID AND IsDelete=0), ");
            sql.AppendFormat("  CarStyleName=(SELECT Name FROM dbo.CarStyle WHERE id=a.CarStyleID AND IsDelete=0), ");
            sql.AppendFormat("  CustomerName,Phone,LicensePlateNumber,RepairReason,Remarks=a.Remark, ");
            sql.AppendFormat("  WxNickName=(SELECT WxNickName FROM dbo.Customer WHERE WxOpenId=a.WxOpenId AND IsDelete=0), ");
            sql.AppendFormat("  TargetTime=CONVERT(VARCHAR(100), a.TargetTime, 120), ");
            sql.AppendFormat("  OrderStatus=(CASE WHEN IsHandle=0 AND IsSuccess=0 THEN '1'   ");
            sql.AppendFormat("   WHEN IsHandle=1 AND IsSuccess=1 THEN '2' ELSE '3' END)  ");
            sql.AppendFormat("    FROM dbo.RepairOrder a ");
            sql.AppendFormat("   WHERE a.id={0} ", BusinessID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取预约保养订单详情

        /// <summary>
        /// 获取预约保养订单详情
        /// </summary>
        public DataSet GetMaintenanceOrderInfo(int BusinessID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT ");
            sql.AppendFormat("  CarTypeName=(SELECT Name FROM dbo.CarType WHERE id=a.CarTypeID AND IsDelete=0), ");
            sql.AppendFormat("  CarStyleName=(SELECT Name FROM dbo.CarStyle WHERE id=a.CarStyleID AND IsDelete=0), ");
            sql.AppendFormat("  CustomerName,Phone,LicensePlateNumber,ReMarks=a.Remark, ");
            sql.AppendFormat("  WxNickName=(SELECT WxNickName FROM dbo.Customer WHERE WxOpenId=a.WxOpenId AND IsDelete=0), ");
            sql.AppendFormat("  TargetTime=CONVERT(VARCHAR(100), a.TargetTime, 120), ");
            sql.AppendFormat("  BugCarTime=CONVERT(VARCHAR(100), a.BuyCarTime, 120), ");
            sql.AppendFormat("  LastMaintenanceTime=CONVERT(VARCHAR(100), a.LastMaintenanceTime, 120), ");
            sql.AppendFormat("  LastMileage,CurrentMileage, ");
            sql.AppendFormat("  OrderStatus=(CASE WHEN IsHandle=0 AND IsSuccess=0 THEN '1'   ");
            sql.AppendFormat("   WHEN IsHandle=1 AND IsSuccess=1 THEN '2' ELSE '3' END)  ");
            sql.AppendFormat("    FROM dbo.MaintenanceOrder a ");
            sql.AppendFormat("   WHERE a.id={0} ", BusinessID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取预约保养保养项目详情

        /// <summary>
        /// 获取预约保养保养项目详情
        /// </summary>
        public DataSet GetMaintenanceOrderDetailInfo(int BusinessID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT Value=a.MaintenanceProject,");
            sql.AppendFormat("  Name=(SELECT Name FROM dbo.BasicData WHERE TypeCode='MaintenanceProject' AND IsDelete=0 AND Value=a.MaintenanceProject), ");
            sql.AppendFormat("  IsChecked ");
            sql.AppendFormat("  FROM dbo.MaintenanceOrderDetail a  ");
            sql.AppendFormat("  WHERE MaintenanceOrderID={0} AND IsDelete=0 ", BusinessID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 修改预约试驾处理状态

        /// <summary>
        /// 修改预约试驾处理状态
        /// </summary>
        /// <param name="BusinessType">业务类型目前有5类，1 = 预约保养   2 = 预约维修   3 = 预约试驾   4 = 在线询价   5 = 在线咨询</param>
        /// <param name="BusinessID">业务单号ID</param>
        public void UpdateTestDriverHandleStatus(int BusinessID, string OrderStatus,string TestDriverTargetTime)
        {
            if (!string.IsNullOrEmpty(OrderStatus) && OrderStatus!="1")
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" UPDATE dbo.TestDriveOrder SET ");
                if (OrderStatus == "2")  //预约成功
                {
                    sql.AppendFormat("  IsHandle = '1' ,");
                    sql.AppendFormat("  IsSuccess = '1' ,");
                }
                if (OrderStatus == "3") //预约失败
                {
                    sql.AppendFormat("  IsHandle = '1' ,");
                    sql.AppendFormat("  IsSuccess = '0' ,");
                }
                sql.AppendFormat("  TargetTime = '{0}' ,", TestDriverTargetTime.ToDateTime());
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat(" WHERE ID={0} ", BusinessID);
                sql.AppendFormat(" AND IsDelete = 0 ");
                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }
        #endregion

        #region 修改预约维修处理状态

        /// <summary>
        /// 修改预约维修处理状态
        /// </summary>
        /// <param name="BusinessType">业务类型目前有5类，1 = 预约保养   2 = 预约维修   3 = 预约试驾   4 = 在线询价   5 = 在线咨询</param>
        /// <param name="BusinessID">业务单号ID</param>
        public void UpdateRapairOrderHandleStatus(int BusinessID, string OrderStatus, string RepairTargetTime, string RepairRemars, string RepairReason)
        {
            if (!string.IsNullOrEmpty(OrderStatus) && OrderStatus != "1")
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" UPDATE dbo.RepairOrder SET ");
                if (OrderStatus == "2")  //预约成功
                {
                    sql.AppendFormat("  IsHandle = '1' ,");
                    sql.AppendFormat("  IsSuccess = '1' ,");
                }
                if (OrderStatus == "3") //预约失败
                {
                    sql.AppendFormat("  IsHandle = '1' ,");
                    sql.AppendFormat("  IsSuccess = '0' ,");
                }
                sql.AppendFormat("  RepairReason = '{0}' ,", RepairReason);
                sql.AppendFormat("  TargetTime = '{0}' ,", RepairTargetTime.ToDateTime());
                sql.AppendFormat("  Remark = '{0}' ,", RepairRemars);
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat(" WHERE ID={0} ", BusinessID);
                sql.AppendFormat(" AND IsDelete = 0 ");
                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }
        #endregion

        #region 修改预约保养处理状态

        /// <summary>
        /// 修改预约保养处理状态
        /// </summary>
        /// <param name="BusinessType">业务类型目前有5类，1 = 预约保养   2 = 预约维修   3 = 预约试驾   4 = 在线询价   5 = 在线咨询</param>
        /// <param name="BusinessID">业务单号ID</param>
        public void UpdateMaintenanceOrderHandleStatus(int BusinessID, string OrderStatus, string MaintenanceTargetTime, string MaintenanceRemarks)
        {
            if (!string.IsNullOrEmpty(OrderStatus) && OrderStatus != "1")
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" UPDATE dbo.MaintenanceOrder SET ");
                if (OrderStatus == "2")  //预约成功
                {
                    sql.AppendFormat("  IsHandle = '1' ,");
                    sql.AppendFormat("  IsSuccess = '1' ,");
                }
                if (OrderStatus == "3") //预约失败
                {
                    sql.AppendFormat("  IsHandle = '1' ,");
                    sql.AppendFormat("  IsSuccess = '0' ,");
                }
                sql.AppendFormat("  TargetTime = '{0}' ,", MaintenanceTargetTime.ToDateTime());
                sql.AppendFormat("  Remark = '{0}' ,", MaintenanceRemarks);
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat(" WHERE ID={0} ", BusinessID);
                sql.AppendFormat(" AND IsDelete = 0 ");
                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }
        #endregion

        #region 修改预约保养具体项目状态

        /// <summary>
        /// 修改预约保养处理状态
        /// </summary>
        /// <param name="BusinessType">业务类型目前有5类，1 = 预约保养   2 = 预约维修   3 = 预约试驾   4 = 在线询价   5 = 在线咨询</param>
        /// <param name="BusinessID">业务单号ID</param>
        public void UpdateMaintenanceOrderDetailStatus(int BusinessID, List<MaintenanceProject> list)
        {
            if (list!=null)
            {
                var sql = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {   
                    sql.AppendFormat(" UPDATE dbo.MaintenanceOrderDetail SET ");
                    sql.AppendFormat("  IsChecked = '{0}' ,",list[i].IsChecked);
                    sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                    sql.AppendFormat(" WHERE MaintenanceOrderID={0} ", BusinessID);
                    sql.AppendFormat(" AND MaintenanceProject = '{0}' ",list[i].Value);
                    sql.AppendFormat(" AND IsDelete = 0 ;");     
                }
                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }
        #endregion

        #region 修改客服处理后的订单状态

        /// <summary>
        /// 修改客服处理后的订单状态
        /// </summary>
        /// <param name="BusinessType">业务类型目前有5类，1 = 预约保养   2 = 预约维修   3 = 预约试驾   4 = 在线询价   5 = 在线咨询</param>
        /// <param name="BusinessID">业务单号ID</param>
        public void UpdateServiceHandleOrderStatus(int BusinessID, string BusinessType)
        {      
            var sql = new StringBuilder();
            sql.AppendFormat(" UPDATE dbo.ServiceCustomerMapping SET ");
            sql.AppendFormat("  HandleStatus = '1' ,");
            sql.AppendFormat("  SessionStatus = '0' ,"); 
            sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
            sql.AppendFormat(" WHERE BusinessID={0} ", BusinessID);
            sql.AppendFormat(" AND BusinessType = '{0}' ", BusinessType);
            sql.AppendFormat(" AND IsDelete = 0 ");
            this.SQLHelper.ExecuteNonQuery(sql.ToString());        
        }
        #endregion
    }
}
