using System.Messaging;

namespace Yunchee.Volkswagen.Utility.Msmq.Base
{
    public interface IMSMQ<T>
    {
        void Send(T obj);

        void Listen();

        MessageEnumerator GetEnumerator();
    }
}
