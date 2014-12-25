using System;
using System.Messaging;
using Yunchee.Volkswagen.Utility.Log;
using Yunchee.Volkswagen.Utility.Msmq.Base;
using Yunchee.Volkswagen.Utility.Msmq.Builder;

namespace Yunchee.Volkswagen.Utility.Msmq
{
    /// <summary>
    /// 日志类消息队列
    /// </summary>
    public class LogMsmq : BaseMSMQ<BaseLogInfo>
    {
        public LogMsmq()
            : base()
        { }
        public LogMsmq(Action<Message> ac)
            : base(ac)
        { }

        public override System.Messaging.MessageQueue CreateMsq()
        {
            return MsmqBulder.Create("TestLog");
        }

        public override System.Messaging.MessageQueue CreateNotifyMsq()
        {
            return MsmqBulder.Create("TestLogNotify");
        }
    }
}
