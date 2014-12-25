using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Yunchee.Volkswagen.DataAccess.Base;
using Yunchee.Volkswagen.Entity;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using Yunchee.Volkswagen.Utility.Log;

namespace Yunchee.Volkswagen.DataAccess.Report
{
    public class ReportDAO : BaseDAO<BasicUserInfo>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public ReportDAO(BasicUserInfo pUserInfo)
            : base(pUserInfo, ConfigInfo.CURRENT_CONNECTION_STRING_MANAGER)
        {
            this.SQLHelper.OnExecuted += new EventHandler<SqlCommandExecutionEventArgs>(SQLHelper_OnExecuted);
        }
        #endregion

        #region 事件处理
        /// <summary>
        /// SQL助手执行完毕后，记录日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SQLHelper_OnExecuted(object sender, SqlCommandExecutionEventArgs e)
        {
            if (e != null)
            {
                var log = new DatabaseLogInfo();
                //获取用户信息
                if (e.UserInfo != null)
                {
                    log.ClientID = e.UserInfo.ClientID;
                    log.UserID = e.UserInfo.UserID;
                }
                //获取T-SQL相关信息
                if (e.Command != null)
                {
                    TSQL tsql = new TSQL();
                    tsql.CommandText = e.Command.GenerateTSQLText();
                    if (e.Command.Connection != null)
                    {
                        tsql.DatabaseName = e.Command.Connection.Database;
                        tsql.ServerName = e.Command.Connection.DataSource;
                    }
                    tsql.ExecutionTime = e.ExecutionTime;
                    log.TSQL = tsql;
                }
                Loggers.DEFAULT.Database(log);
            }
        }
        #endregion

        /// <summary>
        /// 获取昨天关注详细及累计关注数
        /// </summary>
        /// <returns></returns>
        public DataSet GetLastSubscribDetail(string DealerName)
        {
            string sqlStr = @"SELECT SUM(Enrollment) AS AddAttention, -- 新增关注
SUM(Withdraw) AS CanAttention, --取消关注
SUM(Net) AS CountAttention --净增关注 
FROM dbo.ReportEnrollmentDaily 
WHERE CONVERT(DATE,[Date],120) ='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") +
"' and DealerName = '" + DealerName + "'" +
@" SELECT SUM(total) AS SumAttention --累计关注
FROM dbo.ReportEnrollmentDaily where DealerName = '" + DealerName + "'";
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取昨天关注详细及累计关注数--" + sqlStr });
            return this.SQLHelper.ExecuteDataset(sqlStr);
        }

        /// <summary>
        /// 获取查询日期关注详细
        /// </summary>
        /// <returns></returns>
        public DataSet GetSubscribDetailByDate(string StartDate, string EndDate)
        {
            string sqlStr = @"SELECT Date AS date ,SUM(Enrollment)  AS AddAttention, --'新增关注',
SUM(Withdraw) AS CanAttention, --'取消关注',
SUM(Net) AS CountAttention --'净增关注' 
FROM dbo.ReportEnrollmentDaily 
WHERE CONVERT(DATE,[Date],120) BETWEEN '" + StartDate + "' AND '" + EndDate + "'" +
"GROUP BY Date ORDER BY Date";
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取查询日期关注详细--" + sqlStr });
            return this.SQLHelper.ExecuteDataset(sqlStr);
        }

        /// <summary>
        /// 获取关注者性别统计
        /// </summary>
        /// <returns></returns>
        public DataSet GetSubscribSex(string dealerName)
        {
            string sqlStr = "select 'man',Net as mCount from ReportEnrollment where DealerName = '" + dealerName + @"' and Gender = 1
union all
select 'women',Net as mCount from ReportEnrollment where DealerName = '" + dealerName + @"' and Gender = 2
union all
SELECT  'unknown','0' as mCount ";
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取关注者性别统计--" + sqlStr });
            return this.SQLHelper.ExecuteDataset(sqlStr);
        }

        /// <summary>
        /// 获取关注者类型统计
        /// </summary>
        /// <returns></returns>
        public DataSet GetSubscribType(string dealerName)
        {
            string sqlStr = @"SELECT SUM(Owner) AS car,
SUM(PotentialCustomer) AS Potential, 0 AS unknown
FROM dbo.ReportEnrollment where  DealerName = '" + dealerName + "'";
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取关注者类型统计--" + sqlStr });
            return this.SQLHelper.ExecuteDataset(sqlStr);
        }

        /// <summary>
        /// 获取车型分析统计
        /// </summary>
        /// <returns></returns>
        public DataSet GetCarTypeAnalysis(int clientid, string date)
        {
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@ClientID",SqlDbType=SqlDbType.Int,Value=clientid},
                new SqlParameter{ParameterName="@ExecDate",SqlDbType=SqlDbType.NVarChar,Value=date}
            };
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取关注者类型统计--" });
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCarTypeAnalysis", parameters);
        }

        /// <summary>
        /// 获取预约维修保养
        /// </summary>
        /// <returns></returns>
        public DataSet GetRepairManitanceOrder(string dealerName, string startDate, string endDate)
        {
            string sqlStr = string.Format(@"select SubmitTime as date,SUM(RepairTotal) AS service,SUM(MaintenanceTotal ) AS serving
FROM ReportRMDaily WHERE   
DealerName = '{0}' and SubmitTime >= '{1}' and SubmitTime <= '{2}'
GROUP BY SubmitTime HAVING SUM(RepairTotal)>0 or SUM(MaintenanceTotal )>0", dealerName, startDate, endDate);
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取预约维修保养--" + sqlStr });
            return this.SQLHelper.ExecuteDataset(sqlStr);
        }

        /// <summary>
        /// 获取预约维修保养详情
        /// </summary>
        /// <returns></returns>
        public DataSet GetRepairManitanceOrderDetail(string dealerName, string queryDate, string Type)
        {
            string sqlStr = string.Format(@"select 
	convert(nvarchar(10),a.TargetTime,121)TargetTime,
	a.ClientID,b.Name as ClientName,a.CarTypeID,c.Name as CarName,a.CarStyleID,d.Name as CarStyleName,count(*) as mCount 
from {0} a 
inner join Client b on a.ClientID = b.ID and b.Type = 2
inner join CarType c on a.CarTypeID = c.ID
inner join CarStyle d on a.CarStyleID = d.ID
where 1 = 1
	and a.IsDelete = 0
    and a.IsSuccess = 1
    AND b.ID={1}
	and convert(nvarchar(10),a.SubmitTime,121) = '{2}'	
group by convert(nvarchar(10),a.TargetTime,121),a.ClientID,b.Name,a.CarTypeID,a.CarStyleID,c.Name,d.Name"
                , Type == "service" ? "RepairOrder" : "MaintenanceOrder"
                , dealerName
                , queryDate);
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取预约维修保养详情--" + sqlStr });
            return this.SQLHelper.ExecuteDataset(sqlStr);
        }

        /// <summary>
        /// 获取预约试驾
        /// </summary>
        /// <returns></returns>
        public DataSet GetTestDriveOrder(string dealerName, string queryDate)
        {
            string sqlStr = string.Format(@"select CarType,sum(Total)Total from (SELECT * FROM ReportTestdriveSubmitDaily b WHERE b.CarType!='') AS a 
WHERE DealerName = '{0}' AND SubmitTime='{1}' group by CarType", dealerName, queryDate);
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取预约试驾--" + sqlStr });
            return this.SQLHelper.ExecuteDataset(sqlStr);
        }

        /// <summary>
        /// 获取预约试驾详情
        /// </summary>
        /// <returns></returns>
        public DataSet GetTestDriveOrderDetail(string dealerName, string queryDate)
        {
            string sqlStr = string.Format(@"select TargetTime,CarType,Total from ReportTestdriveTargetDaily 
WHERE DealerName = '{0}' and SubmitTime = '{1}' 
GROUP BY TargetTime,CarType,Total
ORDER BY TargetTime", dealerName, queryDate);
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "获取预约试驾详情--" + sqlStr });
            return this.SQLHelper.ExecuteDataset(sqlStr);
        }

        /// <summary>
        /// 用户新增量（已经设置当前日期）：
        /// </summary>
        /// <returns></returns>
        public DataSet GetIncrement(int dealerName)
        {
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "用户新增量（已经设置当前日期）：--" + dealerName });
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter(){ ParameterName="@DealerId",SqlDbType=SqlDbType.Int,Value=dealerName}
            };
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCustomerEnrollDaily", param);
        }

        /// <summary>
        /// 用户总量：
        /// </summary>
        /// <returns></returns>
        public DataSet GetTotal(int dealerName)
        {
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "用户总量：--" + dealerName });
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter(){ ParameterName="@DealerId",SqlDbType=SqlDbType.Int,Value=dealerName}
            };
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCustomerEnrollTotal", param);
        }

        /// <summary>
        /// 预约试驾用户增长量：
        /// </summary>
        /// <returns></returns>
        public DataSet GetTestDrive(int dealerName, string beginDate, string endDate)
        {
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "预约试驾用户增长量：--" + dealerName });
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter(){ ParameterName="@DealerId",SqlDbType=SqlDbType.Int,Value=dealerName},
                new SqlParameter(){ParameterName="@StartDate",SqlDbType=SqlDbType.DateTime,Value=Convert.ToDateTime(beginDate+" 00:00:00")},
                new SqlParameter(){ParameterName="@EndDate",SqlDbType=SqlDbType.DateTime,Value=Convert.ToDateTime(endDate+" 00:00:00")}
            };
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCustomerTestDriveByGender", param);
        }

        /// <summary>
        /// 预约试驾用户总量：
        /// </summary>
        /// <returns></returns>
        public DataSet GetTestDriveTotal(int dealerName)
        {
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "预约试驾用户总量：：--" + dealerName });
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter(){ ParameterName="@DealerId",SqlDbType=SqlDbType.Int,Value=dealerName}
            };
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCustomerTestDriveByGenderTotal", param);
        }

        /// <summary>
        /// 预约保养用户增长量：
        /// </summary>
        /// <returns></returns>
        public DataSet GetMaintenance(int dealerName, string beginDate, string endDate)
        {
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "预约保养用户增长量：--" + dealerName });
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter(){ ParameterName="@DealerId",SqlDbType=SqlDbType.Int,Value=dealerName},
                new SqlParameter(){ParameterName="@StartDate",SqlDbType=SqlDbType.DateTime,Value=Convert.ToDateTime(beginDate+" 00:00:00")},
                new SqlParameter(){ParameterName="@EndDate",SqlDbType=SqlDbType.DateTime,Value=Convert.ToDateTime(endDate+" 00:00:00")}
            };
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCustomerMaintenanceByGender", param);
        }

        /// <summary>
        /// 预约保养用户总量：
        /// </summary>
        /// <returns></returns>
        public DataSet GetMaintenanceTotal(int dealerName)
        {
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "预约保养用户总量：：--" + dealerName });
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter(){ ParameterName="@DealerId",SqlDbType=SqlDbType.Int,Value=dealerName}
            };
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCustomerMaintenanceByGenderTotal", param);
        }

        /// <summary>
        /// 预约维修用户增长量：
        /// </summary>
        /// <returns></returns>
        public DataSet GetRepair(int dealerName, string beginDate, string endDate)
        {
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "预约维修用户增长量：--" + dealerName });
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter(){ ParameterName="@DealerId",SqlDbType=SqlDbType.Int,Value=dealerName},
                new SqlParameter(){ParameterName="@StartDate",SqlDbType=SqlDbType.DateTime,Value=Convert.ToDateTime(beginDate+" 00:00:00")},
                new SqlParameter(){ParameterName="@EndDate",SqlDbType=SqlDbType.DateTime,Value=Convert.ToDateTime(endDate+" 00:00:00")}
            };
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCustomerRepairByGender", param);
        }

        /// <summary>
        /// 预约维修用户总量：
        /// </summary>
        /// <returns></returns>
        public DataSet GetRepairTotal(int dealerName)
        {
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "预约维修用户总量：：--" + dealerName });
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter(){ ParameterName="@DealerId",SqlDbType=SqlDbType.Int,Value=dealerName}
            };
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spCustomerRepairByGenderTotal", param);
        }

        /// <summary>
        /// 系统公告：
        /// </summary>
        /// <returns></returns>
        public DataSet GetSystemNotice()
        {
            string sqlStr = @"SELECT id,title,PictureUrl,Content,CreateTime FROM dbo.SystemNotice where isdelete=0
ORDER BY SortIndex ,CreateTime desc";
            Loggers.DEFAULT.Debug(new DebugLogInfo() { Message = "系统公告：：--"+sqlStr });
            return this.SQLHelper.ExecuteDataset(CommandType.Text, sqlStr);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        #region 获取市场活动效果统计
        /// <summary>
        /// 获取市场活动效果统计
        /// </summary>
        /// <returns></returns>
        public DataSet GetMarketEffectList()
        {
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "spEventsStatistics");
        }
        #endregion
    }
}
