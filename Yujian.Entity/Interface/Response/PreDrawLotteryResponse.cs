using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yunchee.Volkswagen.Entity.Interface.Base;

namespace Yujian.Entity.Interface.Response
{
    public class PreDrawLotteryResponse : BaseEntity
    {
        public PreDrawLotteryData Data;
    }

    public class PreDrawLotteryData
    {
        public int PreDrawLotteryResult { get; set; }

        /// <summary>
        /// 已中奖ID
        /// </summary>
        public int PrizeID { get; set; }

        public int MappingID { get; set; }

    }
}
