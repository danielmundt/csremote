#region Header

// Copyright (C) 2012 Daniel Schubert
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
// AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;

using Remoting.Service;
using Remoting.Service.Events;

namespace Remoting.Server
{
    public class RemoteService : MarshalByRefObject, IRemoteService
	{
        private List<ClientSink> clients = new List<ClientSink>();

        public event EventHandler<ClientAddedEventArgs> ClientAdded;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

		public override object InitializeLifetimeService()
		{
			// indicate that this lease never expires
			return null;
		}

		// called from client to publish a messsage
        public void PublishMessage(ClientSink clientSink, Object obj)
		{
            // register client and notify listeners
            if (!clients.Contains(clientSink))
            {
                clients.Add(clientSink);
                OnClientAdded(new ClientAddedEventArgs(clientSink));
            }
            // OnMessageArrived(obj);
            OnMessageReceived(new MessageReceivedEventArgs(obj));

            // echo message to subscribed client
            PublishEvent(clientSink.Name, obj);
		}

        // called from service server to send client an event
        public void PublishEvent(string clientId, Object obj)
        {
            foreach (ClientSink clientSink in clients)
            {
                if ((clientSink.Name == clientId) || (clientSink.Name == string.Empty))
                {
                    clientSink.PublishEvent(obj);
                }
            }
        }

        private void OnClientAdded(ClientAddedEventArgs e)
		{
			if (ClientAdded != null)
			{
				ClientAdded(this, e);
			}
		}

        private void OnMessageReceived(MessageReceivedEventArgs e)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, e);
            }
        }
	}
}