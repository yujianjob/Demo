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
using System.Text;
using Yunchee.Volkswagen.Common.Enum;
using Yunchee.Volkswagen.Utility.ExtensionMethod;

namespace Yunchee.Volkswagen.Utility.DataAccess
{
    /// <summary>
    /// 数据访问的基类
    /// <remarks>
    /// <para>1.所有的数据访问类必须直接或间接的继承自该基类</para>
    /// <para>2.该基类会以日志的形式记录所执行的SQL命令</para>
    /// </remarks>
    /// </summary>
    public abstract class BaseDAO<T> where T : BasicUserInfo
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public BaseDAO(T pUserInfo, IConnectionStringManager pConnectionStringManager)
        {
            this.CurrentUserInfo = pUserInfo;
            //创建SQL Helper
            var connectionString = pConnectionStringManager.GetConnectionStringBy(pUserInfo);
            var sqlHelper = new DefaultSQLHelper(connectionString);
            sqlHelper.CurrentUserInfo = pUserInfo;
            this.SQLHelper = sqlHelper;
        }
        #endregion

        /// <summary>
        /// 数据访问助手
        /// </summary>
        protected ISQLHelper SQLHelper { get; set; }

        /// <summary>
        /// 当前的用户信息
        /// </summary>
        protected T CurrentUserInfo { get; private set; }

        /// <summary>
        /// 获取服务器当前时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetSqlServerTime()
        {
            string sql = " SELECT GETDATE() ";
            var timestr = Convert.ToDateTime(this.SQLHelper.ExecuteScalar(sql));
            return timestr;
        }

        ///<summary>
        /// 返回为某个会话和用域中的指定表生成的最新标识值
        ///</summary>
        ///<returns>表名</returns>
        public int GetIdentCurrent(string tableName)
        {
            var sql = new StringBuilder();
            sql.AppendFormat(" SELECT IDENT_CURRENT('{0}') ", tableName);

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }

        /// <summary>
        /// 获取指定名称的序列号最新值
        /// </summary>
        /// <param name="sequence">序列号名称</param>
        /// <param name="count">更新数量</param>
        /// <returns></returns>
        public int GetSequenceNumber(E_SequenceNumber sequence, int count = 1)
        {
            var name = sequence.ToString();
            var sql = new StringBuilder();

            // 建立临时表，并查找指定序列号的值
            sql.AppendFormat(" DECLARE @T TABLE ( id INT ); ");
            sql.AppendFormat(" INSERT INTO @T ");
            sql.AppendFormat(" SELECT MAX(Value) FROM dbo.SequenceNumber WHERE Name = '{0}'; ", name);
            // 如果不存在指定的序列号，插入一条新数据
            sql.AppendFormat(" INSERT INTO SequenceNumber(Name, Value) ");
            sql.AppendFormat(" SELECT '{0}', 0 FROM @T WHERE id IS NULL OR id = 0; ", name);
            // 增加指定数量的排序并返回最新值
            sql.AppendFormat(" UPDATE SequenceNumber SET Value = ISNULL(Value, 0) + {1} WHERE Name = '{0}'; ", name, count);
            sql.AppendFormat(" SELECT Value FROM SequenceNumber WHERE Name = '{0}'; ", name);

            return this.SQLHelper.ExecuteScalar(sql.ToString()).ToInt();
        }
    }
}
