using System;
using System.Messaging;
using Yunchee.Volkswagen.Utility.Msmq.Base;

namespace Yunchee.Volkswagen.Utility.Msmq.Config
{
    public class MessageConfig : IMessageConfig
    {
        public virtual void Config(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
