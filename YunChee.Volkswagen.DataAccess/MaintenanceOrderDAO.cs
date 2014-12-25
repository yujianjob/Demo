/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/3 15:50:50
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
    /// 数据访问： 0702预约保养表 MaintenanceOrder 
    /// 表MaintenanceOrder的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class MaintenanceOrderDAO : BaseDAO<BasicUserInfo>, ICRUDable<MaintenanceOrderEntity>, IQueryable<MaintenanceOrderEntity>
    {

        #region 分页查询
        /// <summary>
        /// 获取分页预约保养列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">搜索文本</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetMaintenanceOrderList(PagedQueryEntity pageEntity, string start, string end, MaintenanceOrderEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"(
    SELECT  a.* ,
            b.Name AS CarTypeName ,--车型名称
            c.Name AS CarStyleName ,--车款名称
            d.Name AS GenderName ,--性别
            e.Name AS IsTurboName ,--是否增涡轮
            f.Name AS IsMaintenanceName ,--是否过大保
            bb.Name AS IsHandleName ,-- 是否处理
            bc.Name AS IsSuccessName , --是否预约成功
            cc.Name AS DealerName,--经销商名称
            cc.ParentID AS ClientParentID -- 经销商ParentID
    FROM    dbo.MaintenanceOrder AS a
            LEFT JOIN dbo.CarType AS b ON a.CarTypeID = b.ID
                                          AND b.IsDelete = 0
            LEFT JOIN dbo.CarStyle AS c ON a.CarStyleID = c.ID
                                           AND c.IsDelete = 0
            LEFT JOIN dbo.Client AS cc ON cc.ID = a.ClientID
                                          AND cc.IsDelete = 0
            LEFT JOIN dbo.BasicData AS d ON a.Gender = d.value
                                            AND d.IsDelete = 0
                                            AND d.TypeCode = '{0}'
            LEFT JOIN dbo.BasicData AS e ON a.IsTurbo = e.Value
                                            AND e.IsDelete = 0
                                            AND e.TypeCode = '{1}'
            LEFT JOIN dbo.BasicData AS f ON a.IsMaintenance = f.Value
                                            AND f.IsDelete = 0
                                            AND f.TypeCode = '{2}'
            LEFT JOIN dbo.BasicData AS bb ON bb.Value = a.IsHandle
                                             AND bb.IsDelete = 0
                                             AND bb.TypeCode = '{3}'
            LEFT JOIN dbo.BasicData AS bc ON bc.Value = a.IsSuccess
                                             AND bc.IsDelete = 0
                                             AND bc.TypeCode = '{4}'
                  WHERE     a.IsDelete = 0
            ) AS t", E_BasicData.Gender.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString());

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

        #region 客户预约保养信息
        /// <summary>
        /// 预约保养ID
        /// </summary>
        /// <param name="moId"></param>
        /// <returns></returns>
        public MaintenanceOrderEntity GetMaintenanceOrderById(object moId)
        {
            var entity = new MaintenanceOrderEntity();
            //参数检查
            if (moId == null)
                return null;
            string id = moId.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"
    SELECT  a.* ,
            b.Name AS CarTypeName ,--车型名称
            c.Name AS CarStyleName ,--车款名称
            d.Name AS GenderName ,--性别
            e.Name AS IsTurboName ,--是否增涡轮
            f.Name AS IsMaintenanceName ,--是否过大保
            bb.Name AS IsHandleName ,-- 是否处理
            bc.Name AS IsSuccessName , --是否预约成功
            cc.ParentID AS ClientParentID -- 经销商ParentID
    FROM    dbo.MaintenanceOrder AS a
            LEFT JOIN dbo.CarType AS b ON a.CarTypeID = b.ID
                                          AND b.IsDelete = 0
            LEFT JOIN dbo.CarStyle AS c ON a.CarStyleID = c.ID
                                           AND c.IsDelete = 0
            LEFT JOIN dbo.Client AS cc ON cc.ID = a.ClientID
                                          AND cc.IsDelete = 0
            LEFT JOIN dbo.BasicData AS d ON a.Gender = d.value
                                            AND d.IsDelete = 0
                                            AND d.TypeCode = '{1}'
            LEFT JOIN dbo.BasicData AS e ON a.IsTurbo = e.Value
                                            AND e.IsDelete = 0
                                            AND e.TypeCode = '{2}'
            LEFT JOIN dbo.BasicData AS f ON a.IsMaintenance = f.Value
                                            AND f.IsDelete = 0
                                            AND f.TypeCode = '{3}'
            LEFT JOIN dbo.BasicData AS bb ON bb.Value = a.IsHandle
                                             AND bb.IsDelete = 0
                                             AND bb.TypeCode = '{4}'
            LEFT JOIN dbo.BasicData AS bc ON bc.Value = a.IsSuccess
                                             AND bc.IsDelete = 0
                                             AND bc.TypeCode = '{5}'
               WHERE     a.IsDelete = 0  AND a.ID={0}", id.ToString(), E_BasicData.Gender.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString(), E_BasicData.YesOrNo.ToString());

            DataSet ds = this.SQLHelper.ExecuteDataset(sql.ToString());

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                entity = DataTableToObject.ConvertToObject<MaintenanceOrderEntity>(ds.Tables[0].Rows[0]);
            }

                
            //返回
            return entity;
        }
        #endregion

        #region 获取预约保养信息（通过微信识别号）

        /// <summary>
        /// 获取预约保养信息（通过微信识别号）
        /// </summary>
        /// <returns></returns>
        public DataSet GetMaintenanceByOpenID(string OpenID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT TOP 1 CarTypeID,CarStyleID, ");
            sql.AppendFormat(" CarTypeName=(SELECT b.name FROM dbo.CarType b WHERE b.id=a.CarTypeID), ");
            sql.AppendFormat(" CarStyleName=(SELECT c.name FROM dbo.CarStyle c WHERE c.id=a.CarStyleID), ");
            sql.AppendFormat(" LastMileage,LastMaintenanceTime=CONVERT(varchar(100),LastMaintenanceTime, 23) ");
            sql.AppendFormat(" FROM MaintenanceOrder a WHERE  a.WxOpenId='{0}' ",OpenID);
            sql.AppendFormat(" AND a.IsDelete=0  ");
            //sql.AppendFormat(" AND a.IsDelete=0 AND IsHandle='1' AND IsSuccess='1' ");
            sql.AppendFormat(" ORDER BY a.LastMaintenanceTime DESC ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取车主预约保养列表

        /// <summary>
        /// 获取预约保养信息（通过微信识别号）
        /// </summary>
        /// <returns></returns>
        public DataSet GetCustomerMaintenanceList(string OpenID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.CustomerName,a.LicensePlateNumber,a.Phone, ");
            sql.AppendFormat(" CarTypeName=(SELECT b.name FROM dbo.CarType b WHERE b.id=a.CarTypeID), ");
            sql.AppendFormat(" CarStyleName=(SELECT c.name FROM dbo.CarStyle c WHERE c.id=a.CarStyleID), ");
            sql.AppendFormat(" TargetTime=CONVERT(varchar(100),TargetTime, 120), ");
            sql.AppendFormat(" SubmitTime=CONVERT(varchar(100),SubmitTime, 120), ");
            sql.AppendFormat(" a.LastMileage, ");
            sql.AppendFormat(" OrderStatus = CASE WHEN ( a.IsHandle = 0 AND a.IsSuccess = 0) THEN '未处理' ");
            sql.AppendFormat(" WHEN ( a.IsHandle = 1 AND a.IsSuccess = 0 ) THEN '预约失败' ");
            sql.AppendFormat(" ELSE '预约成功' END ");
            sql.AppendFormat(" FROM MaintenanceOrder a ");
            sql.AppendFormat(" WHERE a.IsDelete=0 ");
            sql.AppendFormat(" AND a.WxOpenId='{0}' ",OpenID);
            sql.AppendFormat(" ORDER BY a.TargetTime DESC ,a.CreateTime DESC");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取累计的保养里程以及保养月数（通过微信识别号）

        /// <summary>
        /// 获取预约保养信息（通过微信识别号）
        /// </summary>
        /// <returns></returns>
        public DataSet GetMileage_MothAdd(string OpenID, int CurrentMileage, string TargetTime, int CarStyleID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT a.MaintenanceProject, ");
            sql.AppendFormat(" MonthCount=((year('{0}') - year(b.TargetTime))*12 + month('{0}')-month(b.TargetTime)-(case when day('{0}')<day(b.TargetTime) ",TargetTime.ToDateTime());
            sql.AppendFormat(" THEN 1 else 0 end) +a.LastMonth), ");
            sql.AppendFormat(" MileageCount= {0}-b.CurrentMileage+a.LastMileage ",CurrentMileage);
            sql.AppendFormat(" FROM  dbo.MaintenanceOrderDetail a, ");
            sql.AppendFormat(" ( SELECT TOP 1 * FROM MaintenanceOrder c  ");
            sql.AppendFormat(" WHERE  c.WxOpenId='{0}' ",OpenID);
            sql.AppendFormat(" AND c.IsDelete=0 AND c.IsHandle='1' AND c.IsSuccess='1' AND c.CarStyleID={0}  ",CarStyleID);
            sql.AppendFormat(" ORDER BY c.TargetTime DESC) as b  ");
            sql.AppendFormat(" WHERE a.MaintenanceOrderID=b.ID AND a.IsDelete=0  ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
