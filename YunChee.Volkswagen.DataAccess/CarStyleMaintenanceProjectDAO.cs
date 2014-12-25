/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/8/5 14:28:24
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
    /// 数据访问： 0320车款保养项目表 CarStyleMaintenanceProject 
    /// 表CarStyleMaintenanceProject的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CarStyleMaintenanceProjectDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarStyleMaintenanceProjectEntity>, IQueryable<CarStyleMaintenanceProjectEntity>
    {
        #region 获取推荐保养项目列表(以前在这里保养过)

        /// <summary>
        /// 获取推荐保养项目列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetMaintenanceProjectList_1(int CarStyleID, int CurrentMileage, string TargetTime, string OpenID)
        {
           
            
            var sql = new StringBuilder();

            sql.AppendFormat(@"
   SELECT Value,Name,
	     IsChecked=(CASE WHEN ischeck1='0' AND ischeck2='0' THEN '0' ELSE '1' END)
   FROM  
	     (SELECT value=f.MaintenanceProject,
                 name=(SELECT name  FROM dbo.BasicData b 
                 WHERE b.TypeCode='MaintenanceProject' AND b.Value=f.MaintenanceProject),
	             ischeck1=(CASE WHEN (year('{0}') - year(f.TargetTime))*12 + month('{0}')-month(f.TargetTime)-(case when day('{0}')<day(f.TargetTime)
	             then 1 else 0 end)+f.LastMonth>=f.RealityMinMonth THEN '1' ELSE '0' END),
	             ischeck2=(CASE WHEN {1}-f.CurrentMileage+f.LastMileage>=f.RealityMinMileage THEN '1' ELSE '0' END)
	     FROM 
	            (SELECT e.MaintenanceProject,e.LastMileage,e.LastMonth,e.TargetTime,e.CurrentMileage, d.CarStyleID,d.MinMileage,d.MinMonth,d.RealityMinMileage,d.RealityMinMonth
	             FROM CarStyleMaintenanceProject d,
	                (SELECT a.MaintenanceOrderID, a.MaintenanceProject,a.IsChecked,a.LastMileage,a.LastMonth,
	                        b.TargetTime,b.CarStyleID,b.CurrentMileage  
                     FROM dbo.MaintenanceOrderDetail a,
	                      (SELECT TOP 1 * FROM MaintenanceOrder c WHERE  c.WxOpenId='{2}'
	                      AND c.IsDelete=0 AND c.IsHandle='1' AND c.IsSuccess='1' ORDER BY c.TargetTime DESC) as b 
	                      WHERE a.MaintenanceOrderID=b.ID AND a.IsDelete=0) as e
	                      WHERE d.CarStyleID=e.CarStyleID AND d.MaintenanceProject=e.MaintenanceProject) 	 
	                      f WHERE f.CarStyleID={3}) g", TargetTime.ToDateTime(),CurrentMileage, OpenID, CarStyleID);
      
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region 获取推荐保养项目列表(按照第一次保养计算)

        /// <summary>
        /// 获取推荐保养项目列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetMaintenanceProjectList_2(int CarStyleID, int CurrentMileage, string BuyCarTime, string TargetTime, string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT Value,Name, ");
            sql.AppendFormat("        IsChecked=(CASE WHEN ischeck1='0' AND ischeck2='0' THEN '0' ELSE '1' END) ");
            sql.AppendFormat(" FROM  (SELECT value=a.MaintenanceProject,");
            sql.AppendFormat("               name=(SELECT name FROM dbo.BasicData b WHERE b.TypeCode='MaintenanceProject' AND b.Value=a.MaintenanceProject), ");
            sql.AppendFormat("               ischeck1=(CASE WHEN (year('{0}') - year('{1}'))*12 + month('{0}')-month('{1}')-(case when day('{0}')<day('{1}') ", TargetTime.ToDateTime(), BuyCarTime.ToDateTime());
            sql.AppendFormat("               then 1 else 0 end)>=a.RealityMinMonth THEN '1' ELSE '0' END),");
            sql.AppendFormat("               ischeck2=(CASE WHEN ({0}-0)>=a.RealityMinMileage THEN '1' ELSE '0' END) ", CurrentMileage);
            sql.AppendFormat("         FROM CarStyleMaintenanceProject a WHERE a.CarStyleID={0}) c ", CarStyleID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region 判断维修订单以及订单详情是否已经存在（通过微信识别号）

        /// <summary>
        /// 获取预约保养信息（通过微信识别号）
        /// </summary>
        /// <returns></returns>
        public DataSet GetOrderAndProjectByOpenID(string OpenID,int carStyleID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT  * FROM dbo.MaintenanceOrderDetail a, ");
            sql.AppendFormat(" (SELECT TOP 1 * FROM MaintenanceOrder c WHERE  c.WxOpenId='{0}' ", OpenID);
            sql.AppendFormat(" AND c.CarStyleID={0} AND c.IsDelete=0 AND c.IsHandle='1' AND c.IsSuccess='1' ORDER BY c.TargetTime DESC) b  ",carStyleID);
            sql.AppendFormat(" WHERE a.MaintenanceOrderID=b.ID AND a.IsDelete=0 ");

            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion
    }
}
