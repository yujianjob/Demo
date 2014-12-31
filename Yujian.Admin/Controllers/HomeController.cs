using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YuJian.WeiXin.Entity;
using YuJian.WeiXin.BLL;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.DataAccess.Query;
using Webdiyer.WebControls.Mvc;
using System.Text;
using System.IO;
namespace Yujian.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult BaiDu()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string userPwd)
        {
            if (userName == "admin" && userPwd == "tianruipwd")
            {
                Session["Login"] = "true";

                return RedirectToAction("Index");
            }
            return View();
        }
        /// <summary>
        /// 呵呵jjj
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int? id = 1)
        {
            if (Session["Login"] == null)
            {
                return View("Login");
            }
            ViewBag.Message = "用户信息列表";

            List<CustomerEntity> userList = new List<CustomerEntity>();
            int pageIndex = id ?? 1;
            //var list=new CustomerBLL(new BasicUserInfo()).PagedQuery(null, new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Asc } }
            //, 10, id.Value);
            //userList = list.Entities.ToList();
            userList = new CustomerBLL(new BasicUserInfo()).GetAll().OrderByDescending(s=>s.CreateTime).ToList();
            PagedList<CustomerEntity> mPage = userList.AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = userList.Count;
            mPage.CurrentPageIndex = (int)(id ?? 1);
            return View(mPage);
        }

        
        public FileResult ExportExcel()
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            var lstTitle = new List<string> { "标识", "昵称", "性别" , "关注时间" };
            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");

            var userList = new CustomerBLL(new BasicUserInfo()).GetAll().OrderByDescending(s => s.CreateTime).ToList();
            for (int i = 0; i < userList.Count; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].WxOpenId);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].WxNickName);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].WxSex=="1"?"男":"女");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].CreateTime);
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");

            //第一种:使用FileContentResult
            //byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            //return File(fileContents, "application/ms-excel", "fileContents.xls");

            //第二种:使用FileStreamResult
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            var fileStream = new MemoryStream(fileContents);
            return File(fileStream, "application/ms-excel", "用户信息.xls");

            //第三种:使用FilePathResult
            //服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }

        public FileResult ExportExcel1()
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            var lstTitle = new List<string> { "标识", "IP地址", "访问时间"};
            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");

            var userList = new VisitIPBLL(new BasicUserInfo()).GetAll().OrderByDescending(s => s.CreateTime).ToList();
            for (int i = 0; i < userList.Count; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].Openid);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].IPAddress);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].CreateTime);
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");

            //第一种:使用FileContentResult
            //byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            //return File(fileContents, "application/ms-excel", "fileContents.xls");

            //第二种:使用FileStreamResult
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            var fileStream = new MemoryStream(fileContents);
            return File(fileStream, "application/ms-excel", "用户IP访问信息.xls");

            //第三种:使用FilePathResult
            //服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }

        public ActionResult About(int? id = 1)
        {
            if (Session["Login"] == null)
            {
                return View("Login");
            }
            ViewBag.Message = "用户访问IP列表";

            List<VisitIPEntity> ipList = new List<VisitIPEntity>();
            int pageIndex = id ?? 1;
            //var list=new CustomerBLL(new BasicUserInfo()).PagedQuery(null, new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Asc } }
            //, 10, id.Value);
            //userList = list.Entities.ToList();
            ipList = new VisitIPBLL(new BasicUserInfo()).GetAll().OrderByDescending(s => s.CreateTime).ToList();
            PagedList<VisitIPEntity> mPage = ipList.AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = ipList.Count;
            mPage.CurrentPageIndex = (int)(id ?? 1);
            return View(mPage);
        }

        public ActionResult Lottery(int? id = 1)
        {
            if (Session["Login"] == null)
            {
                return View("Login");
            }
            ViewBag.Message = "中奖用户列表";

            List<CustomerPrizeMappingEntity> ipList = new List<CustomerPrizeMappingEntity>();
            int pageIndex = id ?? 1;

            ipList = DataTableToObject.ConvertToList<CustomerPrizeMappingEntity>(new CustomerPrizeMappingBLL(new BasicUserInfo()).GetLottery().Tables[0])
                .OrderByDescending(s => s.CreateTime).ToList();
            PagedList<CustomerPrizeMappingEntity> mPage = ipList.AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = ipList.Count;
            mPage.CurrentPageIndex = (int)(id ?? 1);
            return View(mPage);
        }

        public ActionResult DelteLottery(int? id = 1)
        {
            var entity = new CustomerPrizeMappingBLL(new BasicUserInfo()).GetByID(id);
            new CustomerPrizeMappingBLL(new BasicUserInfo()).Delete(entity);
            return RedirectToAction("Lottery"); 
        }

        public FileResult ExportExcel2()
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            var lstTitle = new List<string> { "标识", "奖品", "中奖时间" };
            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");

            var userList = DataTableToObject.ConvertToList<CustomerPrizeMappingEntity>(new CustomerPrizeMappingBLL(new BasicUserInfo()).GetLottery().Tables[0])
                .OrderByDescending(s => s.CreateTime).ToList();
            for (int i = 0; i < userList.Count; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].Openid);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].PrizeID == 1 ? "B.L.C.多元修护霜" : userList[i].PrizeID == 2 ? "B.L.C.多元修护精华水" : "B.L.C.多元修护乳");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].CreateTime);
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");

            //第一种:使用FileContentResult
            //byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            //return File(fileContents, "application/ms-excel", "fileContents.xls");

            //第二种:使用FileStreamResult
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            var fileStream = new MemoryStream(fileContents);
            return File(fileStream, "application/ms-excel", "中奖用户列表.xls");

            //第三种:使用FilePathResult
            //服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }

        public ActionResult DrawAll(int? id = 1)
        {
            if (Session["Login"] == null)
            {
                return View("Login");
            }
            ViewBag.Message = "礼盒用户列表";

            List<UserInfoEntity> ipList = new List<UserInfoEntity>();
            int pageIndex = id ?? 1;
            //var list=new CustomerBLL(new BasicUserInfo()).PagedQuery(null, new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Asc } }
            //, 10, id.Value);
            //userList = list.Entities.ToList();
            ipList = new UserInfoBLL(new BasicUserInfo()).GetAll().OrderByDescending(s => s.CreateTime).ToList();
            PagedList<UserInfoEntity> mPage = ipList.AsQueryable().ToPagedList(pageIndex, 10);
            mPage.TotalItemCount = ipList.Count;
            mPage.CurrentPageIndex = (int)(id ?? 1);
            return View(mPage);
        }

        public FileResult ExportExcel3()
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            var lstTitle = new List<string> { "标识", "姓名", "地址", "联系电话", "提交时间" };
            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");

            var userList = new UserInfoBLL(new BasicUserInfo()).GetAll().OrderByDescending(s => s.CreateTime).ToList();
            for (int i = 0; i < userList.Count; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].Openid);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].Name);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].Address);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].Phone);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", userList[i].CreateTime);
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");

            //第一种:使用FileContentResult
            //byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            //return File(fileContents, "application/ms-excel", "fileContents.xls");

            //第二种:使用FileStreamResult
            byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            var fileStream = new MemoryStream(fileContents);
            return File(fileStream, "application/ms-excel", "礼盒用户列表.xls");

            //第三种:使用FilePathResult
            //服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }
    }
}
