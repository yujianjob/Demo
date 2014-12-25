using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yunchee.Volkswagen.Entity.Interface.Base;

namespace YuJian.WeiXin.Entity.Interface.Response
{
    public class GetCurrentDataResponse : BaseEntity
    {
        public GetCurrentDataData Data;
    }

    public class GetCurrentDataData
    {
        public int LotteryNumber { get; set; }
        /// <summary>
        /// 已中奖集合
        /// </summary>
        public List<PrizeInfoEntity> PrizeInfoList{ get; set; }
    }


}
