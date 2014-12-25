/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/5/12 16:49:32
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
    /// ���ݷ��ʣ�  
    /// ��Region�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class RegionDAO : BaseDAO<BasicUserInfo>, ICRUDable<RegionEntity>, IQueryable<RegionEntity>
    {
        #region ��ȡʡ���б�

        /// <summary>
        /// ��ȡʡ���б�
        /// </summary>
        public DataSet GetProvinceList()
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  ProvinceID=ID, ");
            sql.AppendFormat(" ProvinceName=Name ");
            sql.AppendFormat(" FROM dbo.Region  ");
            sql.AppendFormat(" WHERE AreaLevel=1  ");
            sql.AppendFormat(" AND ParentID=0 AND IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion


        #region ��ȡ�����б�
        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        public DataSet GetCityList(int ProvinceID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  ");
            sql.AppendFormat(" CityID=ID, ");
            sql.AppendFormat(" CityName=Name ");
            sql.AppendFormat(" FROM dbo.Region  ");
            sql.AppendFormat(" WHERE ParentID={0}  ", ProvinceID);
            sql.AppendFormat(" AND  AreaLevel=2 AND IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region ��ȡ�������Ƴ����б�

        /// <summary>
        /// ��ȡ��������ʡ���б�
        /// </summary>
        public DataSet GetWeiXinName(string weixin_city, int level)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT  Name ");
            sql.AppendFormat(" FROM dbo.Region  ");
            sql.AppendFormat(" WHERE AreaLevel={0} {1} ", level, level == 1 ? "AND ParentID=0" : "");
            sql.AppendFormat(" AND IsDelete=0 and Name like '{0}%'", weixin_city);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        /// <summary>
        /// ��ȡ����ʡ��
        /// </summary>
        /// <returns></returns>
        public DataSet GetSpecialProvince()
        {
            var sql = @"SELECT Name FROM dbo.Region WHERE ParentID=0 AND 
(Name LIKE '����%' OR Name  LIKE '�Ϻ�%' or Name LIKE '���%'or Name LIKE '����%')";
            return SQLHelper.ExecuteDataset(sql);
        }

    }
}
