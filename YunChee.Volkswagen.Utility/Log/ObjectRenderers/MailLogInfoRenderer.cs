/*
 * Author		:
 * EMail		:
 * Company		:
 * Create On	:
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

using log4net.Appender;

namespace Yunchee.Volkswagen.Utility.Log
{
    /// <summary>
    /// 异常日志信息的呈现器 
    /// </summary>
    public class MailLogInfoAppender : SmtpAppender
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public MailLogInfoAppender()
        {
        }
        #endregion

        protected override void SendEmail(string messageBody)
        {
            string newmessageBody = messageBody + "...";
            //base.EnableSSL = true;
            var host = this.SmtpHost;
            //base.FilterEvent 
            base.SendEmail(newmessageBody);
        }
    }
}
