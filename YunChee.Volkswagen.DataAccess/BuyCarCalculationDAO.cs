/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/7/21 17:24:02
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
    /// ���ݷ��ʣ� 1301��������� BuyCarCalculation 
    /// ��BuyCarCalculation�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class BuyCarCalculationDAO : BaseDAO<BasicUserInfo>, ICRUDable<BuyCarCalculationEntity>, IQueryable<BuyCarCalculationEntity>
    {
        #region ��ȡ����������Ϣ(ͨ��΢���û���ʶ����������)

        /// <summary>
        /// ��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        /// <param name="CalculationType" 1=ȫ�����2=�������3=���ռ���>��������</param>
        public DataSet GetContentByOpenIDType(string OpenID, string CalculationType, int CarStyleID, int Price, int TotalPrice)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.BuyCarCalculation a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}'  ", OpenID);
            sql.AppendFormat(" AND a.CalculationType='{0}'  ", CalculationType);
            sql.AppendFormat(" AND a.IsDelete=0 ");
            sql.AppendFormat(" AND a.CarStyleID={0}  ", CarStyleID);
            sql.AppendFormat(" AND a.TotalPrice={0}  ", TotalPrice);
            sql.AppendFormat(" AND a.Price={0}  ", Price);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region ����ȫ�������ṹ(ͨ��΢���û���ʶ����������)

        /// <summary>
        /// ����ȫ�������ṹ(ͨ��΢���û���ʶ����������)
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public void UpdateCarCalculation(List<int> lst, string OpenID, string CalculationType)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.BuyCarCalculation SET IsDelete = 0, ");
                sql.AppendFormat("  CarStyleID = {0}, ", lst[0]);
                sql.AppendFormat("  Price = {0}, ", lst[1]);
                sql.AppendFormat("  TotalPrice = {0}, ", lst[2]);
                sql.AppendFormat("  CompulsoryInsurance = {0}, ", lst[3]);
                sql.AppendFormat("  VehicleUseTax = {0}, ", lst[4]);
                sql.AppendFormat("  ThirdParty = {0}, ", lst[5]);
                sql.AppendFormat("  VehicleDamage = {0}, ", lst[6]);
                sql.AppendFormat("  WholeVehiclePilfer = {0}, ", lst[7]);
                sql.AppendFormat("  BreakageGlass = {0}, ", lst[8]);
                sql.AppendFormat("  SpontaneousCombustion = {0}, ", lst[9]);
                sql.AppendFormat("  NonDeductible = {0}, ", lst[10]);
                sql.AppendFormat("  NoLiability = {0}, ", lst[11]);
                sql.AppendFormat("  PassengerLiability = {0}, ", lst[12]);
                sql.AppendFormat("  BodyScratch = {0}, ", lst[13]);
                sql.AppendFormat("  ShoufuRatio = {0}, ", lst[14]);
                sql.AppendFormat("  RepaymentPeriod = {0}, ", lst[15]);
                sql.AppendFormat("  LastUpdateTime = '{0}' ", DateTime.Now);
                sql.AppendFormat(" WHERE WxOpenId='{0}' ", OpenID);
                sql.AppendFormat(" AND CalculationType='{0}'  ", CalculationType);
                sql.AppendFormat(" AND TotalPrice={0}  ", lst[2]);
                sql.AppendFormat(" AND CarStyleID={0}  ", lst[0]);

                this.SQLHelper.ExecuteNonQuery(sql.ToString());
            }
        }

        #endregion

        #region ��ȡǱ�͹��������б�(ͨ��΢���û���ʶ)

        /// <summary>
        /// ��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
       
        public DataSet GetContentByOpenID(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT CalculationID=ID, ");
            sql.AppendFormat(" CarStyleName=(SELECT b.Name FROM dbo.CarStyle b WHERE b.id=CarStyleID), ");
            sql.AppendFormat(" CarTypeName=( SELECT name FROM dbo.CarType d WHERE d.isdelete=0 AND  d.id=(SELECT CarTypeID  FROM dbo.CarStyle e WHERE e.id=CarStyleID)), ");
            sql.AppendFormat(" Type=a.CalculationType, ");
            sql.AppendFormat(" TypeName=(SELECT c.Name FROM dbo.BasicData c WHERE c.TypeCode='CalculationType' AND c.Value=a.CalculationType), ");
            sql.AppendFormat(" Price,TotalPrice ");
            sql.AppendFormat(" FROM dbo.BuyCarCalculation a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}'  ", OpenID); 
            sql.AppendFormat(" AND a.IsDelete=0 ");
            sql.AppendFormat(" ORDER BY a.ID DESC ");
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region ��ȡǱ�͹��������б�(ͨ��ID)

        /// <summary>
        /// ��ȡǱ�͹��������б�
        /// </summary>
        /// <param name="ID">����ID</param>

        public DataSet GetContentByID(int CalculationID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT CarStyleID, ");
            sql.AppendFormat(" CarStyleName=(SELECT b.Name FROM dbo.CarStyle b WHERE b.id=CarStyleID), ");
            sql.AppendFormat(" CarTypeID=(SELECT CarTypeID  FROM dbo.CarStyle f WHERE f.id=CarStyleID), ");
            sql.AppendFormat(" Subsidies=(SELECT Subsidies FROM dbo.CarType WHERE id= (SELECT CarTypeID from dbo.CarStyle WHERE id=CarStyleID)), ");
            sql.AppendFormat(" CarTypeName=( SELECT name FROM dbo.CarType d WHERE d.isdelete=0 AND  d.id=(SELECT CarTypeID  FROM dbo.CarStyle e WHERE e.id=CarStyleID)), ");
            sql.AppendFormat(" Type=a.CalculationType, ");
            sql.AppendFormat(" Price,TotalPrice,ShoufuRatio,RepaymentPeriod,CompulsoryInsurance,VehicleUseTax,ThirdParty,VehicleDamage, ");
            sql.AppendFormat(" BreakageGlass,SpontaneousCombustion,NonDeductible,WholeVehiclePilfer,NoLiability,PassengerLiability,BodyScratch  ");
            sql.AppendFormat(" FROM dbo.BuyCarCalculation a ");
            sql.AppendFormat(" WHERE a.ID={0}  ", CalculationID);
            sql.AppendFormat(" AND a.IsDelete=0 ");
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

    }
}
