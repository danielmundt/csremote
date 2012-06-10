using System;
using System.Collections.Generic;
using System.Text;

namespace Remoting.Service
{
    public interface IRemoteService
    {
        event EventHandler<ClientAddedEventArgs> ClientAdded;
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        void PublishMessage(ClientSink clientSink, Object obj);
        void PublishEvent(string clientId, Object obj);
    }
}
