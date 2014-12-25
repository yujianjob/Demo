using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Yujian.WebService.Interface
{
    public partial class Data : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            string content = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Request["Action"]))
                {
                    string action = Request["Action"].ToString().Trim();
                    switch (action)
                    {
                        case "GetCurrentData":  
                            content = new InterfaceHandler().GetCurrentData();
                            break;
                        case "PreDrawLottery":
                            content = new InterfaceHandler().PreDrawLottery();
                            break;
                        case "CompleteDrawLottery":
                            content = new InterfaceHandler().CompleteDrawLottery();
                            break;
                        case "Share":
                            content = new InterfaceHandler().Share();
                            break;
                        case "SubmitInfo":
                            content = new InterfaceHandler().SubmitInfo();
                            break;
                        default:
                            throw new Exception("未定义的接口:" + action);
                    }
                }
            }
            catch (Exception ex)
            {
                content = ex.Message;
            }

            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(content);
            Response.End();
        }
    }
}