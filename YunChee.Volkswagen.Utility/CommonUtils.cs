using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Yunchee.Volkswagen.Utility.Log;

namespace Yunchee.Volkswagen.Utility
{
    public class CommonUtils
    {
        public static string Host = ConfigurationManager.AppSettings["Host"].ToString();
        #region 获取时间

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDateTime()
        {
            return GetDateTime(DateTime.Now);
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion

        #region 转换日期为时间戳

        /// <summary>
        /// 转换日期为时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        #endregion

        #region 过滤关键字, 防止SQL攻击

        /// <summary>
        /// 过滤关键字, 防止SQL攻击
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static string FilterKeyword(string str)
        {
            if (str == null)
            {
                return "";
            }
            else
            {
                //删除脚本
                str = Regex.Replace(str, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

                //删除HTML
                str = Regex.Replace(str, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"-->", "", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"<!--.*", "", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                str = Regex.Replace(str, @"&#(\d+);", "", RegexOptions.IgnoreCase);

                //删除与数据库相关的词
                //str = Regex.Replace(str, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "select", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "insert", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "delete from", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "count''", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "drop table", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "truncate", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "asc", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "char", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "exec master", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "and", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "net user", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "or", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "net", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "-", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "delete", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "drop", "", RegexOptions.IgnoreCase);
                //str = Regex.Replace(str, "script", "", RegexOptions.IgnoreCase);

                //特殊的字符
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                str = str.Replace("*", "");
                str = str.Replace("-", "");
                str = str.Replace("?", "");
                str = str.Replace("'", "''");
                str = str.Replace(",", "");
                str = str.Replace("/", "");
                str = str.Replace(";", "");
                str = str.Replace("*/", "");
                str = str.Replace("\r\n", "");
                str = HttpContext.Current.Server.HtmlEncode(str).Trim();

                return str;
            }
        }

        #endregion

        #region 提交表单数据

        /// <summary>
        /// 提交表单数据
        /// </summary>
        /// <param name="uri">提交数据的URI</param>
        /// <param name="method">GET, POST</param>
        /// <param name="content">提交内容</param>
        /// <returns></returns>
        public static string GetRemoteData(string uri, string method, string content)
        {
            string respData = "";
            method = method.ToUpper();
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            req.KeepAlive = false;
            req.Method = method.ToUpper();
            req.Credentials = System.Net.CredentialCache.DefaultCredentials;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            if (method == "POST")
            {
                //byte[] buffer = Encoding.ASCII.GetBytes(content);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
                req.ContentLength = buffer.Length;
                req.ContentType = "application/x-www-form-urlencoded";
                Stream postStream = req.GetRequestStream();
                postStream.Write(buffer, 0, buffer.Length);
                postStream.Close();
            }
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
            StreamReader loResponseStream = new StreamReader(resp.GetResponseStream(), enc);
            respData = loResponseStream.ReadToEnd();
            loResponseStream.Close();
            resp.Close();
            return respData;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #endregion

        #region 写入微信日志

        /// <summary>
        /// 写入微信日志
        /// </summary>
        /// <param name="message">消息文本</param>
        /// <param name="weixinId">公众账号ID</param>
        public static void WriteLogWeixin(string message, string weixinId = "weixin")
        {
            string dirPath = HttpContext.Current.Server.MapPath("~/logs/weixin/");
            dirPath += weixinId + "/";

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            string date = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString();
            string filename = dirPath + date + ".txt";

            StreamWriter sr = null;

            try
            {
                if (!System.IO.File.Exists(filename))
                {
                    sr = System.IO.File.CreateText(filename);
                }
                else
                {
                    sr = System.IO.File.AppendText(filename);
                }

                sr.WriteLine("\n" + DateTime.Now.ToLocalTime().ToString() + " :  " + message);
            }
            catch
            {

            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        #endregion

        #region 将HTTP请求参数转换为字符串

        /// <summary>
        /// 将HTTP请求参数转换为字符串
        /// </summary>
        /// <param name="httpContext">HTTP上下文</param>
        /// <returns></returns>
        public static string ConvertHttpContextToString(HttpContext httpContext)
        {
            Loggers.Debug(new DebugLogInfo { Message = "将HTTP请求参数转换为字符串" });

            System.IO.Stream stream = httpContext.Request.InputStream;
            byte[] bt = new byte[stream.Length];
            stream.Read(bt, 0, (int)stream.Length);
            string postStr = System.Text.Encoding.UTF8.GetString(bt);

            Loggers.Debug(new DebugLogInfo { Message = "postStr:  " + postStr });

            return postStr;
        }

        #endregion

        #region 获取客户端IP地址（无视代理）

        /// <summary>
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }

            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion

        #region 下载和删除图片

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="image_url"></param>
        /// <param name="weixin_id"></param>
        /// <returns></returns>
        public static string UpLoadHeadImg(string image_url, string weixin_id = "weixin")
        {
            string uploadPath = "upload";
            FileStream writer = null;
            WebRequest request = null;
            WebResponse response = null;
            Stream reader = null;
            try
            {
                string dirPath = HttpContext.Current.Server.MapPath("~/" + uploadPath + "/");
                dirPath += weixin_id + "/images/";

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                request = WebRequest.Create(image_url);
                response = request.GetResponse();
                reader = response.GetResponseStream();

                string date = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                writer = new FileStream(dirPath + date + ".jpg", FileMode.OpenOrCreate, FileAccess.Write);

                byte[] buff = new byte[512];
                int c = 0; //实际读取的字节数
                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    writer.Write(buff, 0, c);
                }
                return (Host.EndsWith("/") ? Host : Host.Substring(0, Host.LastIndexOf("/"))) + uploadPath + "/" + weixin_id + "/images/" + date + ".jpg";
            }
            catch (Exception e)
            {

            }
            finally
            {
                writer.Close();
                writer.Dispose();
                reader.Close();
                reader.Dispose();
                response.Close();
            }
            return string.Empty;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="image_url"></param>
        /// <returns></returns>
        public static string DeleteHeadImg(string image_url)
        {
            try
            {
                string dirPath = HttpContext.Current.Server.MapPath("~" + image_url);

                if (File.Exists(dirPath))
                    File.Delete(dirPath);
            }
            catch (Exception e)
            {

            }
            finally
            {
            }
            return string.Empty;
        }

        #endregion

        #region 生成兑换码
        /// <summary>
        /// 生成兑换码
        /// </summary>
        /// <param name="n">想生成的几位兑换码</param>
        public static string RandomNum(int n) //
        {
            //定义一个包括数字、大写英文字母和小写英文字母的字符串
            //string strchar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string strchar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            //将strchar字符串转化为数组
            //String.Split 方法返回包含此实例中的子字符串（由指定Char数组的元素分隔）的 String 数组。
            string[] VcArray = strchar.Split(',');
            string VNum = "";
            //记录上次随机数值，尽量避免产生几个一样的随机数          
            int temp = -1;
            //采用一个简单的算法以保证生成随机数的不同
            Random rd = new Random(Guid.NewGuid().GetHashCode());//以GUID作为因子，保证生成的随机字符永远不重复 

            // Random rd= new Random(); //不能写成这样，数目小的60条左右没问题，多了就会有很多重复

            for (int i = 1; i < n + 1; i++)
            {
                if (temp != -1)
                {
                    //unchecked 关键字用于取消整型算术运算和转换的溢出检查。
                    //DateTime.Ticks 属性获取表示此实例的日期和时间的刻度数。
                    rd = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                //Random.Next 方法返回一个小于所指定最大值的非负随机数。
                int t = rd.Next(35);
                if (temp != -1 && temp == t)
                {
                    return RandomNum(n);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;//返回生成的随机数
        }

        #endregion
    }
}
