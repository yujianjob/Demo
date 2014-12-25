using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yunchee.Volkswagen.Entity.Interface.Base;

namespace YuJian.WeiXin.Entity.Interface.Response
{
    public class SubmitInfoResponse:BaseEntity
    {
        public SubmitInfoData Data;
    }

    public class SubmitInfoData
    {
        public int SubmitInfoResult { get; set; }
    }
}
