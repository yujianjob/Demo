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

using System;
using System.Linq;

namespace Yunchee.Volkswagen.Utility.Log
{
    /// <summary>
    /// 异常日志信息 
    /// </summary>
    [Serializable]
    public class ExceptionLogInfo:BaseLogInfo
    {
        #region 构造函数
        /// <summary>
        /// 异常日志信息 
        /// </summary>
        /// <param name="pClientID">客户ID</param>
        /// <param name="pUserID">用户ID</param>
        /// <param name="pException">异常信息</param>
        public ExceptionLogInfo(int pClientID, int pUserID, Exception pException)
        {
            this.ClientID = pClientID;
            this.UserID = pUserID;
            this.Init(pException);
        }
        /// <summary>
        /// 异常日志信息
        /// </summary>
        /// <param name="pUserInfo">当前用户信息</param>
        /// <param name="pException">异常信息</param>
        public ExceptionLogInfo(BasicUserInfo pUserInfo, Exception pException)
        {
            if (pUserInfo != null)
            {
                this.ClientID = pUserInfo.ClientID;
                this.UserID = pUserInfo.UserID;
            }
            this.Init(pException);
        }
        /// <summary>
        /// 异常日志信息
        /// </summary>
        /// <param name="pException">异常信息</param>
        public ExceptionLogInfo(Exception pException)
        {
            this.Init(pException);
        }

        public ExceptionLogInfo()
        { 

        }
        #endregion

        #region 属性集
        /// <summary>
        /// 是否Yunchee自定义的异常
        /// </summary>
        public bool IsYuncheeException { get; set; }

        /// <summary>
        /// 异常是否被最外层的框架代码捕获
        /// </summary>
        public bool IsCatchedByOuterFrameworkCode { get; set; }

        /// <summary>
        /// 最近20条执行的T-SQL语句
        /// </summary>
        public TSQL[] Last20ExecutedTSQLs { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pException"></param>
        protected void Init(Exception pException)
        {
            if (pException != null)
            {
                //自定义异常
                if (pException is YunCheeException)
                {
                    this.IsYuncheeException = true;
                    var temp = pException as YunCheeException;
                }
                else
                {
                    this.IsYuncheeException = false;
                }
                this.ErrorMessage = pException.Message;
                //跟踪堆栈
                this.StackTrances = SystemUtils.GetStackTracesFrom(pException);
                if (this.StackTrances != null && this.StackTrances.Length > 0)
                {
                    var thrownLocation = this.StackTrances[this.StackTrances.Length - 1];
                    this.IsCatchedByOuterFrameworkCode = thrownLocation.IsOuterFrameworkCode;
                    this.Location = thrownLocation.GetFullMethodName();
                    //
                    var tSQLs = SystemRuntimeInfo.GetInstace().GetTSQLBy(this.ClientID, this.UserID);
                    if (tSQLs != null && tSQLs.Length > 20)
                    {
                        tSQLs = tSQLs.Take(20).ToArray();
                    }
                    this.Last20ExecutedTSQLs = tSQLs;
                }
            }
        }
        #endregion
    }
}
