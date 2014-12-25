/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/27 16:54:25
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
    /// 数据访问： 0101客户信息表 Customer 
    /// 表Customer的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CustomerDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerEntity>, IQueryable<CustomerEntity>
    {
        #region 分页查询
        /// <summary>
        /// 获取分页客户列表列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">搜索文本</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetCustomerList(PagedQueryEntity pageEntity, string[] str)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"
        ( SELECT    CId=a.id,
                    ClientCode=a.Code,
                    ClientName=a.Name,
                    b.*
          FROM      dbo.Client a  JOIN dbo.Customer b 
                    ON a.ID=b.ClientID 
                    AND b.IsDelete = 0
        ) AS t");

            pageEntity.QueryFieldName = string.Format("*");

            StringBuilder strcondition = new StringBuilder();

            strcondition.AppendFormat(" AND CId={0}", (str[0] == null | str[0] == "") ? 1 : str[0].ToInt());
            if (!string.IsNullOrEmpty(str[2]))
                strcondition.AppendFormat(" AND WxNickName like '%{0}%'", str[2]);
            if (!string.IsNullOrEmpty(str[3]) && (str[3] != "-1"))
                strcondition.AppendFormat(" AND WxSex={0}", str[3].ToInt());
            if (!string.IsNullOrEmpty(str[4]) && (str[4] != "-1"))
                strcondition.AppendFormat(" AND SubscribeStatus ={0}", str[4].ToInt());
            if (!string.IsNullOrEmpty(str[5]) && (str[5] != "-1"))
                strcondition.AppendFormat(" AND Type={0}", str[5].ToInt());

            if (!string.IsNullOrEmpty(str[6]))
                strcondition.AppendFormat(" AND Name like '%{0}%'", str[6]);
            if (!string.IsNullOrEmpty(str[7]))
                strcondition.AppendFormat(" AND Phone like '%{0}%'", str[7]);
            if (!string.IsNullOrEmpty(str[8]))
                strcondition.AppendFormat(" AND LicensePlateNumber like '%{0}%'", str[8]);

            pageEntity.QueryCondition = strcondition.ToString();

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }


        #endregion

        #region 删除客户

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="quesIds">客户ID集合  "1,2,3"</param>
        public void DeleteCustomer(string quesIds)
        {
            if (!string.IsNullOrEmpty(quesIds))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Customer SET IsDelete = 1, ");
                sql.AppendFormat(" LastUpdateBy = '{0}', LastUpdateTime = '{1}' ", this.CurrentUserInfo.UserID, DateTime.Now);
                sql.AppendFormat(" WHERE ID IN ({0}) ", quesIds);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 获取车类型

        /// <summary>
        /// 获取车类型
        /// </summary>
        /// <param name="quesIds"></param>
        public DataSet  CarTypeList()
        {
            DataSet dataSet = null;
            var sql = @"select * from dbo.CarType where IsDelete=0";
            if(sql!=null&&sql!="")
                dataSet= this.SQLHelper.ExecuteDataset(sql);
            return dataSet;
        }

        #endregion

        #region 获取车款

        /// <summary>
        /// 获取车款
        /// </summary>
        /// <param name="quesIds"></param>
        public DataSet CarStyleList()
        {
            DataSet dataSet = null;
            var sql = @"select * from dbo.CarStyle where IsDelete=0";
            if (sql != null && sql != "")
                dataSet = this.SQLHelper.ExecuteDataset(sql);
            return dataSet;
        }

        #endregion

        #region 获取客户的ID(通过微信用户标识)

        /// <summary>
        /// 获取客户的ID
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetCustomerByOpenID(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.ID,a.Name FROM dbo.Customer a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' AND IsDelete=0", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region 获取客户信息(通过微信用户标识)

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetCustomerCountByOpenID(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.Customer a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' AND IsDelete=0 ", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region 更改客户(潜客)资料(通过微信识别号获取信息)

        /// <summary>
        /// 更改客户(潜客)资料(通过微信识别号获取信息)(此方法为潜客注册时调用，
        /// 因为在潜客注册前，必须已经关注微信账号，关注时，已经将客户信息录入到数据库中了
        /// 所有潜客注册，只是一个更新的操作。)
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateByRequest(string CarTypeID, string CarStyleID, string CustomerName, string Gender, string Phone, string PlanBuyTimeValue, string OpenID)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Customer SET IsDelete = 0, ");
                sql.AppendFormat("  Name = '{0}', ", CustomerName);
                sql.AppendFormat("  Type = '2', ");
                sql.AppendFormat("  Phone = '{0}', ", Phone);
                if (Gender != null && Gender.Trim() != "")
                sql.AppendFormat("  WxSex = '{0}', ",  Gender);
                sql.AppendFormat("  IntentionCarTypeID = {0} ,", (CarTypeID != null && CarTypeID.Trim()!="")?CarTypeID.ToInt():-1);
                sql.AppendFormat("  IntentionCarStyleID = {0} ,", (CarStyleID != null && CarStyleID.Trim() != "") ? CarStyleID.ToInt() : -1);
                sql.AppendFormat("  PlanBuyTime = '{0}' ,", PlanBuyTimeValue);
                sql.AppendFormat("  EndFansDate = '{0}' ,", DateTime.Now);
                sql.AppendFormat("  StartPotentialDate = '{0}', ", DateTime.Now);
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat(" WHERE WxOpenId='{0}' ", OpenID);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 更改客户(车主)资料(通过微信识别号获取信息)

        /// <summary>
        /// 更改客户(车主)资料(通过微信识别号获取信息)
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateOwerCarByRequest(string CarTypeID, string CarStyleID, string CustomerName, string Gender, string Phone, string LicensePlateNumber, string OpenID,string type)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" UPDATE dbo.Customer SET IsDelete = 0, ");
                sql.AppendFormat("  Name = '{0}' ,", CustomerName);
                sql.AppendFormat("  Type = '3' ,");
                sql.AppendFormat("  Phone = '{0}' ,", Phone);
                if (Gender != null && Gender.Trim() != "")
                    sql.AppendFormat("  WxSex = '{0}', ", Gender);
                sql.AppendFormat("  BuyCarTypeID = {0} ,", (CarTypeID != null && CarTypeID.Trim() != "") ? CarTypeID.ToInt() : -1);
                sql.AppendFormat("  BuyCarStyleID = {0} ,", (CarStyleID != null && CarStyleID.Trim() != "") ? CarStyleID.ToInt() : -1);
                sql.AppendFormat("  LicensePlateNumber = '{0}' ,", LicensePlateNumber);
                //当为粉丝成为车主时，修改最后的粉丝日期
                if (type == "1")
                {
                    sql.AppendFormat("  EndFansDate = '{0}' ,", DateTime.Now);
                }
                //当为潜客成为车主时，修改最后的潜客日期
                if (type == "2")
                {
                    sql.AppendFormat("  EndPotentialDate = '{0}' ,", DateTime.Now);
                }
                sql.AppendFormat("  StartCustomerDate = '{0}' ,", DateTime.Now);
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat(" WHERE WxOpenId='{0}' ", OpenID);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 获取客户信息(通过微信用户标识，移动端展示)

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetCustomerMobileByOpenID(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.Name,a.Type,a.Phone,a.LicensePlateNumber,a.EngineNumber, ");
            sql.AppendFormat(" NickName=a.WxNickName, ");
            sql.AppendFormat(" Gender=a.WxSex, ");
            sql.AppendFormat(" Birthday=CONVERT(varchar(100),a.Birthday, 23), ");   
            sql.AppendFormat(" HeadImgUrl=a.WxHeadImgUrl, ");
            sql.AppendFormat(" Province=a.WxProvince, ");
            sql.AppendFormat(" City=a.WxCity, ");
            sql.AppendFormat(" DealerName=b.Name,b.ServiceHotline, ");
            sql.AppendFormat(" DealerLongitude=b.Longitude, ");
            sql.AppendFormat(" DealerLatitude=b.Latitude, ");
            sql.AppendFormat(" WeixinName=(SELECT TOP 1 e.Name FROM dbo.WApplication e WHERE e.ClientID=a.ClientID AND a.WxId=e.WeixinID), ");
            sql.AppendFormat(" QrcodeUrl=(SELECT TOP 1 e.QrcodeUrl FROM dbo.WApplication e WHERE e.ClientID=a.ClientID AND a.WxId=e.WeixinID), ");
            sql.AppendFormat(" PlanBuyTimeValue= ");
            sql.AppendFormat(" (CASE WHEN (PlanBuyTime!=NULL) OR (PlanBuyTime!='请选择') THEN (SELECT Name FROM dbo.BasicData WHERE TypeCode='PlanBuyTime' AND Value=a.PlanBuyTime) ELSE NULL END), ");
            sql.AppendFormat(" a.IntentionCarStyleID,a.IntentionCarTypeID, a.BuyCarTypeID,a.BuyCarStyleID,");
            //意向车型图片
            sql.AppendFormat(" IntentionCarImageUrl = (SELECT TOP 1 b.ImageUrl FROM dbo.ObjectImages b ");
            sql.AppendFormat(" 			   WHERE IsDelete = 0 AND ObjectType = '1' AND b.ObjectID = a.IntentionCarTypeID ");
            sql.AppendFormat(" 			   ORDER BY b.SortIndex) ,");
            //已购车型图片
            sql.AppendFormat(" BuyCarImageUrl = (SELECT TOP 1 b.ImageUrl FROM dbo.ObjectImages b ");
            sql.AppendFormat(" 			   WHERE IsDelete = 0 AND ObjectType = '1' AND b.ObjectID = a.BuyCarTypeID ");
            sql.AppendFormat(" 			   ORDER BY b.SortIndex) ,");
            //已购车型名称
            sql.AppendFormat(" BuyCarType=(SELECT name FROM dbo.CarType WHERE id=a.BuyCarTypeID), ");
            //已购车款名称
            sql.AppendFormat(" BuyCarStyle=(SELECT name FROM dbo.CarStyle WHERE id=a.BuyCarStyleID), ");
            //意向车款价格   
            sql.AppendFormat(" IntentionCarStylePrice=(SELECT Column1 FROM dbo.CarStyleBasic  WHERE CarStyleID=a.IntentionCarStyleID AND IsDelete=0), ");
            //已购车款价格   
            sql.AppendFormat(" BuyCarStylePrice=(SELECT Column1 FROM dbo.CarStyleBasic  WHERE CarStyleID=a.BuyCarStyleID AND IsDelete=0), ");
            //意向车款名称
            sql.AppendFormat(" IntentionCarStyle=(SELECT name FROM dbo.CarStyle WHERE id=a.IntentionCarStyleID),CarFrameNumber,  ");
            ////意向车型名称
            sql.AppendFormat(" IntentionCarType=(SELECT name FROM dbo.CarType WHERE id=a.IntentionCarTypeID)  ");
            sql.AppendFormat(" FROM dbo.Customer AS a,dbo.Client AS b  ");
            sql.AppendFormat(" WHERE a.ClientID=b.id AND a.IsDelete=0");
            sql.AppendFormat(" AND a.WxOpenId='{0}' ", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region 获取客户资料(通过微信用户标识，移动端展示)

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetCustomerInfoByCustomerOpenID(string CustomerOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.Name,a.Type,a.Phone, ");
            sql.AppendFormat(" NickName=a.WxNickName, ");
            sql.AppendFormat(" Gender=a.WxSex, ");
            sql.AppendFormat(" Birthday=CONVERT(varchar(100),a.Birthday, 23), ");
            sql.AppendFormat(" HeadImgUrl=a.WxHeadImgUrl, ");          
            sql.AppendFormat(" City=a.WxCity, ");                           
            //已购车型名称
            sql.AppendFormat(" BuyCarType=(SELECT name FROM dbo.CarType WHERE id=a.BuyCarTypeID), ");
            //已购车款名称
            sql.AppendFormat(" BuyCarStyle=(SELECT name FROM dbo.CarStyle WHERE id=a.BuyCarStyleID), "); 
            //意向车款名称
            sql.AppendFormat(" IntentionCarStyle=(SELECT name FROM dbo.CarStyle WHERE id=a.IntentionCarStyleID), ");
            //意向车型名称
            sql.AppendFormat(" IntentionCarType=(SELECT name FROM dbo.CarType WHERE id=a.IntentionCarTypeID)  ");
            sql.AppendFormat(" FROM dbo.Customer a  ");
            sql.AppendFormat(" WHERE  a.IsDelete=0");
            sql.AppendFormat(" AND a.WxOpenId='{0}' ", CustomerOpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 修改潜客个人信息

        /// <summary>
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateQianKeByRequest(string OpenID, string NickName, string Gender, string Birthday, string CustomerName, string Phone, string ProvinceName, string CityName, string CarStyleID,string Type)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Customer  set ");
                sql.AppendFormat("  Name = '{0}', ", CustomerName);
                sql.AppendFormat("  Phone = '{0}', ", Phone);
                if (Gender != null && Gender != "")
                sql.AppendFormat("  WxSex = '{0}', ", Gender);
                if (Birthday!=null&&Birthday.Trim() !=""&&Birthday.Trim() != "0001/1/1 0:00:00")
                sql.AppendFormat("  Birthday = '{0}', ", Birthday.ToDateTime());
                if (CarStyleID != null && CarStyleID != "")
                {
                    sql.AppendFormat("  IntentionCarStyleID = {0} ,", CarStyleID);
                    //sql.AppendFormat("  IntentionCarTypeID = CASE WHEN ({0}>0) THEN ( SELECT   d.CarTypeID", CarStyleID);
                    //sql.AppendFormat("  FROM  dbo.CarStyle d WHERE    d.id ={0} AND d.IsDelete = 0 ) ", CarStyleID);
                    //sql.AppendFormat("  ELSE null END, ");
                    sql.AppendFormat("  IntentionCarTypeID = (SELECT d.CarTypeID FROM dbo.CarStyle d WHERE  d.id = {0} AND d.IsDelete = 0) ,", CarStyleID);
                }
                sql.AppendFormat("  WxNickName = '{0}' ,", NickName);
                sql.AppendFormat("  WxProvince = '{0}' ,", ProvinceName);
                sql.AppendFormat("  WxCity = '{0}' ,", CityName);
                if (Type == "1")
                {
                    sql.AppendFormat("  EndFansDate = '{0}' ,", DateTime.Now);
                    sql.AppendFormat("  StartPotentialDate = '{0}', ", DateTime.Now);
                }
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat("  WHERE WxOpenId='{0}' ", OpenID);
                sql.AppendFormat("  AND  IsDelete = 0 ");
                sql.AppendFormat("  AND  (Type = '1' OR Type = '2')"); 

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 修改意向车款

        /// <summary>
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateCarStyleByRequest(string OpenID, string CarStyleID, string type)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Customer  SET ");
                if (CarStyleID != null && CarStyleID.Trim() != "")
                {
                    if (type == "1")  //当我为粉丝时，修改我的意向车款以及车型，以及更改我的状态为潜客
                    {
                        sql.AppendFormat("  EndFansDate = '{0}' ,", DateTime.Now);
                        sql.AppendFormat("  StartPotentialDate = '{0}', ", DateTime.Now);
                        sql.AppendFormat("  IntentionCarStyleID = {0} ,", CarStyleID);
                        sql.AppendFormat("  Type = '2' ,");
                        sql.AppendFormat("  IntentionCarTypeID = (SELECT d.CarTypeID FROM dbo.CarStyle d WHERE  d.id = {0} AND d.IsDelete = 0) ,", CarStyleID);
                    }
                    if (type == "2")  //当我为潜客时，就修改我的意向车款以及车型
                    {
                        sql.AppendFormat("  IntentionCarStyleID = {0} ,", CarStyleID);
                        //sql.AppendFormat("  IntentionCarTypeID = CASE WHEN ({0}>0) THEN ( SELECT   d.CarTypeID", CarStyleID);
                        //sql.AppendFormat("  FROM  dbo.CarStyle d WHERE    d.id ={0} AND d.IsDelete = 0 ) ", CarStyleID);
                        //sql.AppendFormat("  ELSE null END, ");
                        sql.AppendFormat("  IntentionCarTypeID = (SELECT d.CarTypeID FROM dbo.CarStyle d WHERE  d.id = {0} AND d.IsDelete = 0) ,", CarStyleID);
                    }
                    if (type == "3")  //当为车主时，就更改我已经买的车款以及车型
                    {
                        sql.AppendFormat("  BuyCarStyleID = {0} ,", CarStyleID);
                        sql.AppendFormat("  BuyCarTypeID = (SELECT d.CarTypeID FROM dbo.CarStyle d WHERE  d.id = {0} AND d.IsDelete = 0) ,", CarStyleID);
                    }
                }
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat("  WHERE WxOpenId='{0}' ", OpenID);
                sql.AppendFormat("  AND  IsDelete = 0 ");
                //sql.AppendFormat("  AND  Type = '{0}' ",type); 

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 修改车主个人信息

        /// <summary>
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateCheZhuByRequest(string OpenID, string NickName, string Gender, string Birthday, string CustomerName, string Phone, string ProvinceName, string CityName, string CarTypeID, string CarStyleID, string LicensePlateNumber, string EngineNumber, string CarFrameNumber)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Customer  set ");
                sql.AppendFormat("  Name = '{0}', ", CustomerName); 
                sql.AppendFormat("  Phone = '{0}', ", Phone);
                if (Gender!=null&&Gender.Trim()!="")
                sql.AppendFormat("  WxSex = '{0}', ", Gender);
                if (Birthday != null && Birthday.Trim() != ""&&Birthday.Trim() != "0001/1/1 0:00:00" )
                sql.AppendFormat("  Birthday = '{0}', ", Birthday.ToDateTime());
                if(CarTypeID != null && CarTypeID.Trim() != "")
                sql.AppendFormat("  BuyCarTypeID = {0} ,",  CarTypeID.ToInt());
                if(CarStyleID != null && CarStyleID.Trim() != "")
                sql.AppendFormat("  BuyCarStyleID = {0} ,",  CarStyleID.ToInt());
                //sql.AppendFormat("  BuyCarTypeID = {0} ,", CarTypeID);
                //sql.AppendFormat("  BuyCarStyleID = {0} ,", CarStyleID);
                sql.AppendFormat("  WxNickName = '{0}' ,", NickName);
                sql.AppendFormat("  WxProvince = '{0}' ,", ProvinceName);
                sql.AppendFormat("  WxCity = '{0}' ,", CityName);
                sql.AppendFormat("  LicensePlateNumber = '{0}' ,", LicensePlateNumber);
                sql.AppendFormat("  EngineNumber = '{0}' ,", EngineNumber);
                sql.AppendFormat("  CarFrameNumber = '{0}' ,", CarFrameNumber);
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat("  WHERE WxOpenId='{0}' ", OpenID);
                sql.AppendFormat("  AND  IsDelete = 0 ");
                sql.AppendFormat("  AND  Type = '3' ");

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 提交车主车牌号码

        /// <summary>
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateCheZhuCarStyleByRequest(string OpenID, string CustomerPlateNumber)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Customer SET ");
                sql.AppendFormat("  LicensePlateNumber = '{0}' ,", CustomerPlateNumber);
                //sql.AppendFormat("  IntentionCarStyleID = {0} ,", CarStyleID);
                //sql.AppendFormat("  IntentionCarTypeID = CASE WHEN ({0}>0) THEN ( SELECT   d.CarTypeID", CarStyleID);
                //sql.AppendFormat("  FROM  dbo.CarStyle d WHERE    d.id ={0} AND d.IsDelete = 0 ) ", CarStyleID);
                //sql.AppendFormat("  ELSE null END, ");
                //sql.AppendFormat("  IntentionCarTypeID = (SELECT d.CarTypeID FROM dbo.CarStyle d WHERE  d.id = {0} AND d.IsDelete = 0) ,", CarStyleID);
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat("  WHERE WxOpenId='{0}' ", OpenID);
                sql.AppendFormat("  AND  IsDelete = 0 ");
                sql.AppendFormat("  AND  Type = '3' ");

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region 更改粉丝为潜客

        /// <summary>
        /// 更改粉丝为潜客
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public void UpdateByFun(string[] str, string OpenID)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Customer SET  ");
                if (!string.IsNullOrEmpty(str[0]))
                    sql.AppendFormat("  Name = '{0}', ", str[0]);
                sql.AppendFormat("  Type = '2', ");
                if (!string.IsNullOrEmpty(str[1]))
                    sql.AppendFormat("  Phone = '{0}', ", str[1]);
                if (!string.IsNullOrEmpty(str[2]))
                    sql.AppendFormat("  WxSex = '{0}', ", str[2]);
                if (!string.IsNullOrEmpty(str[3]))
                    sql.AppendFormat("  IntentionCarTypeID = {0}, ", str[3].ToInt());
                sql.AppendFormat("  EndFansDate = '{0}' ,", DateTime.Now);
                sql.AppendFormat("  StartPotentialDate = '{0}', ", DateTime.Now);
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat(" WHERE WxOpenId='{0}' ", OpenID);
                sql.AppendFormat(" AND IsDelete = 0 ");

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }
        #endregion

        #region 获取销售顾问列表(通过微信用户标识，移动端展示)

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetSaleConsultantList()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  ConsultantName=a.Name,HeadImgUrl=a.WxHeadImgUrl,Level='6',ConsultantOpenID=a.WxOpenId ");
            sql.AppendFormat(" FROM    dbo.Customer a , ");
            sql.AppendFormat("         ( SELECT    * ");
            sql.AppendFormat("           FROM      dbo.Users ");
            sql.AppendFormat("           WHERE     ID IN ( SELECT  UserID ");
            sql.AppendFormat("                             FROM    dbo.RoleUsers ");
            sql.AppendFormat("                             WHERE   RoleID IN ( SELECT  ID ");
            sql.AppendFormat("                                                 FROM    dbo.Roles ");
            sql.AppendFormat("                                                 WHERE   name = '销售顾问' AND IsDelete=0 ) AND RoleUsers.IsDelete=0 ) ");
            sql.AppendFormat("         ) b  ");
            sql.AppendFormat(" WHERE   a.WxOpenId = b.WxOpenId   ");
            sql.AppendFormat(" AND  a.IsDelete=0 AND  b.IsDelete=0");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取销售顾问详细资料(通过微信用户标识，移动端展示)

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="OpenID">微信用户标识</param>
        public DataSet GetConsultantInfoByOpenID(string ConsultantOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  a.Name ,a.Phone ,NickName = a.WxNickName , ");
            sql.AppendFormat(" HeadImgUrl = a.WxHeadImgUrl ,Praise = '0' ,ClickNumber = '0' ");
            sql.AppendFormat(" FROM dbo.Customer a  ");
            sql.AppendFormat(" WHERE  a.IsDelete=0");
            sql.AppendFormat(" AND a.WxOpenId='{0}' ", ConsultantOpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

    }
}
