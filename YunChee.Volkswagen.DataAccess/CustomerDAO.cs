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
    /// ���ݷ��ʣ� 0101�ͻ���Ϣ�� Customer 
    /// ��Customer�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class CustomerDAO : BaseDAO<BasicUserInfo>, ICRUDable<CustomerEntity>, IQueryable<CustomerEntity>
    {
        #region ��ҳ��ѯ
        /// <summary>
        /// ��ȡ��ҳ�ͻ��б��б�
        /// </summary>
        /// <param name="pageEntity"></param>
        /// <param name="searchText">�����ı�</param>
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

        #region ɾ���ͻ�

        /// <summary>
        /// ɾ���ͻ�
        /// </summary>
        /// <param name="quesIds">�ͻ�ID����  "1,2,3"</param>
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

        #region ��ȡ������

        /// <summary>
        /// ��ȡ������
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

        #region ��ȡ����

        /// <summary>
        /// ��ȡ����
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

        #region ��ȡ�ͻ���ID(ͨ��΢���û���ʶ)

        /// <summary>
        /// ��ȡ�ͻ���ID
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public DataSet GetCustomerByOpenID(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.ID,a.Name FROM dbo.Customer a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' AND IsDelete=0", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }
        #endregion

        #region ��ȡ�ͻ���Ϣ(ͨ��΢���û���ʶ)

        /// <summary>
        /// ��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public DataSet GetCustomerCountByOpenID(string OpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT * FROM dbo.Customer a ");
            sql.AppendFormat(" WHERE a.WxOpenId='{0}' AND IsDelete=0 ", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region ���Ŀͻ�(Ǳ��)����(ͨ��΢��ʶ��Ż�ȡ��Ϣ)

        /// <summary>
        /// ���Ŀͻ�(Ǳ��)����(ͨ��΢��ʶ��Ż�ȡ��Ϣ)(�˷���ΪǱ��ע��ʱ���ã�
        /// ��Ϊ��Ǳ��ע��ǰ�������Ѿ���ע΢���˺ţ���עʱ���Ѿ����ͻ���Ϣ¼�뵽���ݿ�����
        /// ����Ǳ��ע�ᣬֻ��һ�����µĲ�����)
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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

        #region ���Ŀͻ�(����)����(ͨ��΢��ʶ��Ż�ȡ��Ϣ)

        /// <summary>
        /// ���Ŀͻ�(����)����(ͨ��΢��ʶ��Ż�ȡ��Ϣ)
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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
                //��Ϊ��˿��Ϊ����ʱ���޸����ķ�˿����
                if (type == "1")
                {
                    sql.AppendFormat("  EndFansDate = '{0}' ,", DateTime.Now);
                }
                //��ΪǱ�ͳ�Ϊ����ʱ���޸�����Ǳ������
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

        #region ��ȡ�ͻ���Ϣ(ͨ��΢���û���ʶ���ƶ���չʾ)

        /// <summary>
        /// ��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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
            sql.AppendFormat(" (CASE WHEN (PlanBuyTime!=NULL) OR (PlanBuyTime!='��ѡ��') THEN (SELECT Name FROM dbo.BasicData WHERE TypeCode='PlanBuyTime' AND Value=a.PlanBuyTime) ELSE NULL END), ");
            sql.AppendFormat(" a.IntentionCarStyleID,a.IntentionCarTypeID, a.BuyCarTypeID,a.BuyCarStyleID,");
            //������ͼƬ
            sql.AppendFormat(" IntentionCarImageUrl = (SELECT TOP 1 b.ImageUrl FROM dbo.ObjectImages b ");
            sql.AppendFormat(" 			   WHERE IsDelete = 0 AND ObjectType = '1' AND b.ObjectID = a.IntentionCarTypeID ");
            sql.AppendFormat(" 			   ORDER BY b.SortIndex) ,");
            //�ѹ�����ͼƬ
            sql.AppendFormat(" BuyCarImageUrl = (SELECT TOP 1 b.ImageUrl FROM dbo.ObjectImages b ");
            sql.AppendFormat(" 			   WHERE IsDelete = 0 AND ObjectType = '1' AND b.ObjectID = a.BuyCarTypeID ");
            sql.AppendFormat(" 			   ORDER BY b.SortIndex) ,");
            //�ѹ���������
            sql.AppendFormat(" BuyCarType=(SELECT name FROM dbo.CarType WHERE id=a.BuyCarTypeID), ");
            //�ѹ���������
            sql.AppendFormat(" BuyCarStyle=(SELECT name FROM dbo.CarStyle WHERE id=a.BuyCarStyleID), ");
            //���򳵿�۸�   
            sql.AppendFormat(" IntentionCarStylePrice=(SELECT Column1 FROM dbo.CarStyleBasic  WHERE CarStyleID=a.IntentionCarStyleID AND IsDelete=0), ");
            //�ѹ�����۸�   
            sql.AppendFormat(" BuyCarStylePrice=(SELECT Column1 FROM dbo.CarStyleBasic  WHERE CarStyleID=a.BuyCarStyleID AND IsDelete=0), ");
            //���򳵿�����
            sql.AppendFormat(" IntentionCarStyle=(SELECT name FROM dbo.CarStyle WHERE id=a.IntentionCarStyleID),CarFrameNumber,  ");
            ////����������
            sql.AppendFormat(" IntentionCarType=(SELECT name FROM dbo.CarType WHERE id=a.IntentionCarTypeID)  ");
            sql.AppendFormat(" FROM dbo.Customer AS a,dbo.Client AS b  ");
            sql.AppendFormat(" WHERE a.ClientID=b.id AND a.IsDelete=0");
            sql.AppendFormat(" AND a.WxOpenId='{0}' ", OpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());

        }

        #endregion

        #region ��ȡ�ͻ�����(ͨ��΢���û���ʶ���ƶ���չʾ)

        /// <summary>
        /// ��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public DataSet GetCustomerInfoByCustomerOpenID(string CustomerOpenID)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT a.Name,a.Type,a.Phone, ");
            sql.AppendFormat(" NickName=a.WxNickName, ");
            sql.AppendFormat(" Gender=a.WxSex, ");
            sql.AppendFormat(" Birthday=CONVERT(varchar(100),a.Birthday, 23), ");
            sql.AppendFormat(" HeadImgUrl=a.WxHeadImgUrl, ");          
            sql.AppendFormat(" City=a.WxCity, ");                           
            //�ѹ���������
            sql.AppendFormat(" BuyCarType=(SELECT name FROM dbo.CarType WHERE id=a.BuyCarTypeID), ");
            //�ѹ���������
            sql.AppendFormat(" BuyCarStyle=(SELECT name FROM dbo.CarStyle WHERE id=a.BuyCarStyleID), "); 
            //���򳵿�����
            sql.AppendFormat(" IntentionCarStyle=(SELECT name FROM dbo.CarStyle WHERE id=a.IntentionCarStyleID), ");
            //����������
            sql.AppendFormat(" IntentionCarType=(SELECT name FROM dbo.CarType WHERE id=a.IntentionCarTypeID)  ");
            sql.AppendFormat(" FROM dbo.Customer a  ");
            sql.AppendFormat(" WHERE  a.IsDelete=0");
            sql.AppendFormat(" AND a.WxOpenId='{0}' ", CustomerOpenID);
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region �޸�Ǳ�͸�����Ϣ

        /// <summary>
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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

        #region �޸����򳵿�

        /// <summary>
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
        public void UpdateCarStyleByRequest(string OpenID, string CarStyleID, string type)
        {
            if (!string.IsNullOrEmpty(OpenID))
            {
                var sql = new StringBuilder();

                sql.AppendFormat(" UPDATE dbo.Customer  SET ");
                if (CarStyleID != null && CarStyleID.Trim() != "")
                {
                    if (type == "1")  //����Ϊ��˿ʱ���޸��ҵ����򳵿��Լ����ͣ��Լ������ҵ�״̬ΪǱ��
                    {
                        sql.AppendFormat("  EndFansDate = '{0}' ,", DateTime.Now);
                        sql.AppendFormat("  StartPotentialDate = '{0}', ", DateTime.Now);
                        sql.AppendFormat("  IntentionCarStyleID = {0} ,", CarStyleID);
                        sql.AppendFormat("  Type = '2' ,");
                        sql.AppendFormat("  IntentionCarTypeID = (SELECT d.CarTypeID FROM dbo.CarStyle d WHERE  d.id = {0} AND d.IsDelete = 0) ,", CarStyleID);
                    }
                    if (type == "2")  //����ΪǱ��ʱ�����޸��ҵ����򳵿��Լ�����
                    {
                        sql.AppendFormat("  IntentionCarStyleID = {0} ,", CarStyleID);
                        //sql.AppendFormat("  IntentionCarTypeID = CASE WHEN ({0}>0) THEN ( SELECT   d.CarTypeID", CarStyleID);
                        //sql.AppendFormat("  FROM  dbo.CarStyle d WHERE    d.id ={0} AND d.IsDelete = 0 ) ", CarStyleID);
                        //sql.AppendFormat("  ELSE null END, ");
                        sql.AppendFormat("  IntentionCarTypeID = (SELECT d.CarTypeID FROM dbo.CarStyle d WHERE  d.id = {0} AND d.IsDelete = 0) ,", CarStyleID);
                    }
                    if (type == "3")  //��Ϊ����ʱ���͸������Ѿ���ĳ����Լ�����
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

        #region �޸ĳ���������Ϣ

        /// <summary>
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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

        #region �ύ�������ƺ���

        /// <summary>
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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

        #region ���ķ�˿ΪǱ��

        /// <summary>
        /// ���ķ�˿ΪǱ��
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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

        #region ��ȡ���۹����б�(ͨ��΢���û���ʶ���ƶ���չʾ)

        /// <summary>
        /// ��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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
            sql.AppendFormat("                                                 WHERE   name = '���۹���' AND IsDelete=0 ) AND RoleUsers.IsDelete=0 ) ");
            sql.AppendFormat("         ) b  ");
            sql.AppendFormat(" WHERE   a.WxOpenId = b.WxOpenId   ");
            sql.AppendFormat(" AND  a.IsDelete=0 AND  b.IsDelete=0");
            return SQLHelper.ExecuteDataset(sql.ToString());
        }

        #endregion

        #region ��ȡ���۹�����ϸ����(ͨ��΢���û���ʶ���ƶ���չʾ)

        /// <summary>
        /// ��ȡ�ͻ���Ϣ
        /// </summary>
        /// <param name="OpenID">΢���û���ʶ</param>
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
