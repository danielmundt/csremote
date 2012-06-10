using System;
using System.Collections.Generic;
using System.Text;

namespace Remoting.Service
{
    [Serializable]
    public class ClientAddedEventArgs : EventArgs
    {
        private ClientSink clientSink;

        public ClientAddedEventArgs(ClientSink clientSink)
        {
            this.clientSink = clientSink;
        }

        public ClientSink Sink
        {
            get
            {
                return clientSink;
            }
        }
    }
}
