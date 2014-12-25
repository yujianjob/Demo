/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/17 21:13:07
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
    /// ���ݷ��ʣ� 1201Υ��ʡ�ݱ� ViolationProvince 
    /// ��ViolationProvince�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class ViolationProvinceDAO : BaseDAO<BasicUserInfo>, ICRUDable<ViolationProvinceEntity>, IQueryable<ViolationProvinceEntity>
    {
        public DataSet dsProvice()
        {
            string str = "select max(ID) from ViolationProvince";
            DataSet ds=SQLHelper.ExecuteDataset(str);
            return ds;
        }

        #region ��ȡΥ�²�ѯʡ���б�

        /// <summary>
        /// ��ȡΥ�²�ѯʡ���б�
        /// </summary>
        public DataSet GetViolationProvinceList()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  ");
            sql.AppendFormat(" ProvinceID=ID, ");
            sql.AppendFormat(" ProvinceName=Name, ");
            sql.AppendFormat(" Abbreviate ");
            sql.AppendFormat(" FROM dbo.ViolationProvince  ");
            sql.AppendFormat(" WHERE  IsDelete=0 ");           
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region ��ȡΥ�²�ѯ�����б�

        /// <summary>
        /// ��ȡΥ�²�ѯ�����б�
        /// </summary>
        public DataSet GetViolationCityList(int ProvinceID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  ");
            sql.AppendFormat(" CityID=ID, ");
            sql.AppendFormat(" CityName=Name, ");
            sql.AppendFormat(" CityCode=Code, ");
            sql.AppendFormat(" IsEngine,EngineNumber,IsClass,ClassNumber ");
            sql.AppendFormat(" FROM dbo.ViolationCity  ");
            sql.AppendFormat(" WHERE  IsDelete=0 ");
            sql.AppendFormat(" AND  ProvinceID={0} ", ProvinceID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

    }
}
