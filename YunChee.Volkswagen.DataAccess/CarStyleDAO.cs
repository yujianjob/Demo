/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/6/23 14:58:18
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

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// 数据访问： 0304车款表 CarStyle 
    /// 表CarStyle的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>

    public partial class CarStyleDAO : BaseDAO<BasicUserInfo>, ICRUDable<CarStyleEntity>, IQueryable<CarStyleEntity>
    {

        #region 分页查询
        /// <summary>
        /// 获取分页车款列表
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">搜索文本</param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public PagedQueryObjectResult<DataSet> GetCarStyleList(PagedQueryEntity pageEntity, string searchText, CarStyleEntity queryEntity)
        {
            var result = new PagedQueryObjectResult<DataSet>();
            var query = new PagedQuery(this.CurrentUserInfo);

            pageEntity.TableName = string.Format(@"
        ( SELECT    a.ID ,
                    a.SortIndex ,
					a.CarBrandID,
                    BrandName = b.Name ,
					a.CarTypeID,
                    TypeName = c.Name ,
                    StyleName = a.Name ,
                    a.IsShow ,
                    IsSaleName = d.Name,
                    a.Remark
          FROM      dbo.CarStyle AS a
                    LEFT JOIN dbo.CarBrand AS b ON a.CarBrandID = b.id
                                                   AND b.IsDelete = 0
                    LEFT JOIN dbo.CarType AS c ON a.CarTypeID = c.ID
                                                  AND c.IsDelete = 0
                    LEFT JOIN dbo.BasicData AS d ON a.IsSale = d.value
                                        AND d.IsDelete = 0
                                        AND d.TypeCode = '{0}'
          WHERE     a.IsDelete = 0
        ) AS t",E_BasicData.YesOrNo.ToString());

            pageEntity.QueryFieldName = string.Format("*");

            StringBuilder strcondition = new StringBuilder();
            if (queryEntity.CarBrandID != -1)
                strcondition.AppendFormat(" AND CarBrandID={0} ", queryEntity.CarBrandID);
            if (queryEntity.CarTypeID != -1)
                strcondition.AppendFormat(" AND CarTypeID={0}", queryEntity.CarTypeID);
            if (!string.IsNullOrEmpty(searchText))
                strcondition.AppendFormat(" AND StyleName like '%{0}%'", searchText);
            pageEntity.QueryCondition = strcondition.ToString();

            result.RowCount = query.GetTotalCount(pageEntity);
            result.Data = query.GetPagedData(pageEntity);
            return result;
        }


        #endregion

        #region 获取车款列表

        /// <summary>
        /// 获取车款列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataSet GetCarStyleList()
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT CarStyleID=a.Id,CarStyleName=a.Name,FactoryPrice=b.Column1,CarTypeID=a.CarTypeID, ");
            sql.AppendFormat(" CarTypeName=(SELECT name FROM dbo.CarType c WHERE c.id=a.CarTypeID) ,");
            sql.AppendFormat(" Subsidies=(SELECT Subsidies FROM dbo.CarType WHERE id=CarTypeID) ");
            sql.AppendFormat(" FROM dbo.CarStyle AS a JOIN dbo.CarStyleBasic b ");
            sql.AppendFormat(" ON b.CarStyleID=a.Id WHERE a.IsDelete=0 AND a.IsShow = '1' ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取车款列表(根据车型ID)

        /// <summary>
        /// 获取车款列表(根据车型ID)
        /// </summary>
        /// <param name="carTypeID">车型ID</param>
        /// <returns></returns>
        public DataSet GetCarStyleListByCarTypeId(int carTypeID,string Type)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT CarStyleID=a.Id,CarStyleName=a.Name,FactoryPrice=b.Column1,CarTypeID=a.CarTypeID, ");
            sql.AppendFormat(" CarTypeName=(SELECT name FROM dbo.CarType c WHERE c.id=a.CarTypeID) ,");
            sql.AppendFormat(" Subsidies=(SELECT Subsidies FROM dbo.CarType WHERE id=CarTypeID) ");
            sql.AppendFormat(" FROM dbo.CarStyle AS a JOIN dbo.CarStyleBasic b ");
            sql.AppendFormat(" ON b.CarStyleID=a.Id WHERE a.IsDelete=0 ");
            sql.AppendFormat(" AND CarTypeID = {0} ", carTypeID);
            sql.AppendFormat(" AND IsShow = '1' ");
            if(Type=="1")
            sql.AppendFormat(" AND IsSale = '1' ");
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取指定类型的键值对列表

        /// <summary>
        /// 获取指定类型的键值对列表
        /// </summary>
        /// <param name="typecode">类型的编码</param>
        /// <returns></returns>
        public DataSet GetTypeCodeList(string typeCode)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT ConfigID=value,ConfigName=name ");
            sql.AppendFormat(" FROM dbo.BasicData ");
            sql.AppendFormat(" WHERE IsDelete=0 ");
            sql.AppendFormat(" AND TypeCode = '{0}' ", typeCode);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region 获取车款信息(根据车款ID)

        /// <summary>
        /// 获取车款信息(根据车款ID)
        /// </summary>
        /// <param name="carStyleID">车款ID</param>
        /// <returns></returns>
        public DataSet GetCarStyleById(int CarStyleID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT ID, CarTypeID, Name, ");
            sql.AppendFormat(" CarTypeName = (SELECT c.Name FROM dbo.CarType c WHERE c.id= (SELECT d.CarTypeID FROM dbo.CarStyle d WHERE d.id= {0} AND d.IsDelete=0) AND c.IsDelete=0) ,", CarStyleID);
            sql.AppendFormat(" ImageUrl = (SELECT TOP 1 b.ImageUrl FROM dbo.ObjectImages b ");
            sql.AppendFormat(" 			   WHERE IsDelete = 0 AND ObjectType = '1' AND b.ObjectID = CarTypeID ");
            sql.AppendFormat(" 			   ORDER BY b.SortIndex) ");
            sql.AppendFormat(" FROM dbo.CarStyle a WHERE a.IsDelete = 0 ");
            sql.AppendFormat(" AND a.ID={0} ", CarStyleID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region 获取车款配置明细(根据车款ID和配置ID)

        /// <summary>
        /// 获取车款配置明细
        /// </summary>
        /// <param name="typecode">根据车款ID和配置ID</param>
        /// <returns></returns>
        public DataSet GetConfigByStyleIDConfigID(int CarStyleID,int ConfigID)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" SELECT * ");
            sql.AppendFormat(" FROM {0} ", GetCarStyleConfigTable(ConfigID));
            sql.AppendFormat(" WHERE IsDelete=0 ");
            sql.AppendFormat(" AND CarStyleID = {0} ", CarStyleID);
            return this.SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region 获取具体的表名(配置排序)
        /// <summary>
        /// 获取具体的表名
        /// </summary>
        /// <param name="selectIndex">配置排序</param>
        /// <returns></returns>
        public string GetCarStyleConfigTable(int selectIndex)
        {
                string str = "CarStyleBasic";
               
                if(selectIndex==1) //基本信息表
                    str = "CarStyleBasic";           
                  
                if(selectIndex==2) //车体表
                    str = "CarStyleBody";
                   
                if(selectIndex==3)//发动机表
                    str = "CarStyleEngine";
                  
                if(selectIndex==4) //变速箱表
                    str = "CarStyleGearbox";
                  
                if(selectIndex==5) //底盘制动
                    str = "CarStyleChassisBrake";
                   
                if(selectIndex==6) //安全配置
                    str = "CarStyleSecurityConfig";
                  
                if(selectIndex==7) //车轮
                    str = "CarStyleWheel";
                   
                if(selectIndex==8) //行车辅助
                    str = "CarStyleDrivingAuxiliary";
                   
                if(selectIndex==9) //门窗/后视镜
                    str = "CarStyleDoorWindow";
                   
                if(selectIndex==10) //灯光
                    str = "CarStyleLamplight";
                   
                if(selectIndex==11) //内部配置
                    str = "CarStyleInternalConfig";
                   
                if(selectIndex==12) //座椅
                    str = "CarStyleSeat";
                   
                if(selectIndex==13)  //娱乐通讯
                    str = "CarStyleCommunication"; 

                if (selectIndex == 14) //空调/冰箱
                    str = "CarStyleAirConditioning";

                return str;

        }
        #endregion
    }

}
