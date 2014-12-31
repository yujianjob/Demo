using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yunchee.Volkswagen.Entity.Interface.Base;



namespace YuJian.WeiXin.Entity.Interface.Response
{
    public class CompleteDrawLotteryReponse : BaseEntity
    {
        public CompleteDrawLotteryData Data;
    }

    public class CompleteDrawLotteryData
    {
        public int DrawLotteryResult{get;set;}

        public int LotteryNumber { get; set; }

        public int IsShare { get; set; }

        /// <summary>
        /// 已中奖集合
        /// </summary>
        public List<PrizeInfoEntity> PrizeInfoList{ get; set; }

        public int isEndDraw { get; set; }
    }

    public class PrizeInfoEntity
    {
        /// <summary>
        /// 奖品ID
        /// </summary>
        public int PrizeID { get; set; }
    }
    
}
