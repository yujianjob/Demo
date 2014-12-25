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
using Yunchee.Volkswagen.Common.Enum;
using Yunchee.Volkswagen.Common.Const;

namespace Yunchee.Volkswagen.DataAccess
{

    /// <summary>
    /// 数据访问： 0705预约保养明细表 MaintenanceOrderDetail 
    /// 表MaintenanceOrderDetail的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class MaintenanceOrderDetailDAO : BaseDAO<BasicUserInfo>, ICRUDable<MaintenanceOrderDetailEntity>, IQueryable<MaintenanceOrderDetailEntity>
    {
        public DataTable GetMaintenanceProject(string mid)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
        SELECT a.Value, a.Name, 
		isChecked = ISNULL((SELECT b.IsChecked FROM dbo.MaintenanceOrderDetail b 
		WHERE b.IsDelete = 0 AND b.MaintenanceOrderID = {1} AND b.MaintenanceProject = a.Value), 0)
		FROM dbo.BasicData a 
		WHERE a.TypeCode = '{0}' AND a.IsDelete = 0
		ORDER BY a.SortIndex", E_BasicData.MaintenanceProject.ToString(), mid);

            return SQLHelper.ExecuteDataset(sb.ToString()).Tables[0];
        }

        public void UpdateMaintenanceProject(string mid, string checkeds)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
            BEGIN TRAN

DECLARE @v NVARCHAR(10)
DECLARE contact_cursor CURSOR
FOR
    SELECT  value
    FROM    dbo.BasicData
    WHERE   IsDelete = 0
            AND TypeCode = 'MaintenanceProject'

OPEN contact_cursor

FETCH NEXT FROM contact_cursor INTO @v

WHILE @@FETCH_STATUS = 0
    BEGIN
        IF EXISTS ( SELECT  1
                    FROM    dbo.MaintenanceOrderDetail
                    WHERE   MaintenanceOrderID = {0}
                            AND MaintenanceProject = @v )
            UPDATE dbo.MaintenanceOrderDetail SET IsChecked='{1}' WHERE MaintenanceOrderID={0} AND MaintenanceProject=@v AND IsDelete=0
        ELSE
		INSERT INTO dbo.MaintenanceOrderDetail
	        ( MaintenanceOrderID ,
	          MaintenanceProject ,
	          IsChecked ,
	          LastMileage ,
	          LastMonth ,
	          CreateBy ,
	          CreateTime ,
	          LastUpdateBy ,
	          LastUpdateTime ,
	          IsDelete
	        )
	VALUES  ( {0} , -- MaintenanceOrderID - int
	          @v , -- MaintenanceProject - nvarchar(10)
	          N'{1}' , -- IsChecked - nvarchar(10)
	          0 , -- LastMileage - int
	          0 , -- LastMonth - int
	          {4} , -- CreateBy - int
	          GETDATE() , -- CreateTime - datetime
	          {4} , -- LastUpdateBy - int
	          GETDATE() , -- LastUpdateTime - datetime
	          0  -- IsDelete - int
	        )
		  FETCH NEXT FROM contact_cursor INTO @v
    END

	CLOSE contact_cursor

	deallocate  contact_cursor

	UPDATE dbo.MaintenanceOrderDetail SET IsChecked='{2}' WHERE MaintenanceOrderID={0} AND MaintenanceProject IN ({3}) AND IsDelete=0

	COMMIT TRAN
", mid, C_YesOrNo.NO,C_YesOrNo.YES, checkeds, this.CurrentUserInfo.ClientID);
            if (checkeds.Length < 1)
            {
                string oldstr = string.Format("UPDATE dbo.MaintenanceOrderDetail SET IsChecked='{0}' WHERE MaintenanceOrderID={1} AND MaintenanceProject IN ({2}) AND IsDelete=0", C_YesOrNo.YES,mid, checkeds);
                sb.Replace(oldstr, "");
            }
            SQLHelper.ExecuteNonQuery(sb.ToString());
        }
    }
}
