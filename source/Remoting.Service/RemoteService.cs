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

namespace Remoting.Service
{
    public delegate void ClientAddedEvent(ClientSink clientSink, Object obj);
    public delegate void MessageArrivedEvent(Object obj);
    
    public class RemoteService : MarshalByRefObject
	{
        private List<ClientSink> clients = new List<ClientSink>();

        public event ClientAddedEvent ClientAdded;
        public event MessageArrivedEvent MessageArrived;

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
                OnClientAdded(clientSink, obj);
            }
            OnMessageArrived(obj);

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

        private void OnClientAdded(ClientSink clientSink, Object obj)
		{
			if (ClientAdded != null)
			{
				ClientAdded(clientSink, obj);
			}
		}

        private void OnMessageArrived(Object obj)
        {
            if (MessageArrived != null)
            {
                MessageArrived(obj);
            }
        }
	}
}