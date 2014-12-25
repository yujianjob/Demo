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

namespace Yunchee.Volkswagen.Utility.Entity
{
    /// <summary>
    /// 持久化实体的异常 
    /// </summary>
    public class PersistenceEntityException:YunCheeException
    {
        /// <summary>
        /// 持久化实体的异常
        /// </summary>
        /// <param name="pErrorMessage">错误信息</param>
        public PersistenceEntityException(string pErrorMessage) : base(ERROR_TYPES.INFRASTRUCTURE, INFRASTRUCTURE_ERROR_CODES.PERSISTENCE_STATUS_ERROR, pErrorMessage) { }

        /// <summary>
        /// 持久化实体的异常
        /// </summary>
        /// <param name="pErrorMessageTemplate">错误信息模板</param>
        /// <param name="pMessageParams">错误信息模板参数</param>
        public PersistenceEntityException(string pErrorMessageTemplate,params string[] pMessageParams) : base(ERROR_TYPES.INFRASTRUCTURE, INFRASTRUCTURE_ERROR_CODES.PERSISTENCE_STATUS_ERROR,pErrorMessageTemplate,pMessageParams) { }
    }

    /// <summary>
    /// 错误类别
    /// <remarks>
    /// <para>0-99      基础框架类的错误</para>
    /// <para>100-199   认证类的错误</para>
    /// </remarks>
    /// </summary>
    public static class ERROR_TYPES
    {
        /// <summary>
        /// 基础框架类的错误
        /// <remarks>
        /// <para>此类错误的错误码以'01'开头</para>
        /// </remarks>
        /// </summary>
        public readonly static int INFRASTRUCTURE = 1;

        /// <summary>
        /// 认证类的错误
        /// <remarks>
        /// <para>此类错误的错误码以'02'开头</para>
        /// </remarks>
        /// </summary>
        public readonly static int AUTHENTICATE = 2;
    }

    /// <summary>
    /// 基础框架的错误码
    /// <remarks>
    /// <para>错误码的规则为:错误码类别(两位)+'-'+错误码值(三位)</para>
    /// <para>错误码的值范围定义为:</para>
    /// <para>Utility           0 - 99</para>
    /// <para>Cache             100 - 199</para>
    /// <para>DataAccess        200 - 299</para>
    /// <para>Entity            300 - 399</para>
    /// <para>ExtensionMethod   400 - 499</para>
    /// <para>Log               500 - 599</para>
    /// </remarks>
    /// </summary>
    public static class INFRASTRUCTURE_ERROR_CODES
    {
        /// <summary>
        /// 持久化状态错误
        /// </summary>
        public const string PERSISTENCE_STATUS_ERROR = "01300";
    }
}
