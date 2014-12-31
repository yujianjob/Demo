using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuJian.WeiXin.Entity.Interface.Response;
using Yunchee.Volkswagen.Entity.Interface.Base;

namespace YuJian.WeiXin.Entity.Interface.Request
{
    public class ShareResponse:BaseEntity
    {
        public ShareData Data;
    }

    public class ShareData
    {
        public int ShareResult { get; set; }

        public int LotteryNumber { get; set; }

        public int IsShare { get; set; }

        /// <summary>
        /// 已中奖集合
        /// </summary>
        public List<PrizeInfoEntity> PrizeInfoList { get; set; }
    }
}
