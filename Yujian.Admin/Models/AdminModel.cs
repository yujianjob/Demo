using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YuJian.WeiXin.Entity;

namespace Yujian.Admin.Models
{
    public class AdminModel
    {
        public List<CustomerEntity> customerList { get; set; }

        public List<VisitIPEntity> visitIpList { get; set; }
    }
}