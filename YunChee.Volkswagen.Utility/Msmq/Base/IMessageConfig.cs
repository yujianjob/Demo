using System.Messaging;

namespace Yunchee.Volkswagen.Utility.Msmq.Base
{
    public interface IMessageConfig
    {
        void Config(Message message);
    }
}
