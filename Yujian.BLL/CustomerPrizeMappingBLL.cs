/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:Yunchee
 * Create On	:2014/12/19 23:05:56
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
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using YuJian.WeiXin.DataAccess;
using YuJian.WeiXin.Entity;
using Yunchee.Volkswagen.Utility.DataAccess;
using Yunchee.Volkswagen.Utility.DataAccess.Query;

namespace YuJian.WeiXin.BLL
{
    /// <summary>
    /// 业务处理：  
    /// </summary>
    public partial class CustomerPrizeMappingBLL
    {
        public DataSet GetLottery()
        {
            return this._currentDAO.GetLottery();
        }
    }
}