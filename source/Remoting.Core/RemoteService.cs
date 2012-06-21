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
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Text;

using Remoting.Core.Events;
using Remoting.Core.Exceptions;

namespace Remoting.Core
{
    public class RemoteService : MarshalByRefObject, IRemoteService, IDisposable
    {
        #region Fields

        private bool disposed = false;
        private Dictionary<string, EventProxy> proxies = new Dictionary<string, EventProxy>();

        #endregion Fields

        #region Constructors

        ~RemoteService()
        {
            Dispose(false);
        }

        #endregion Constructors

        #region Events

        public event EventHandler<ClientAddedEventArgs> ClientAdded;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        #endregion Events

        #region Properties

        protected virtual IEnumerable<MarshalByRefObject> NestedMarshalByRefObjects
        {
            get { yield break; }
        }

        #endregion Properties

        #region Methods

        private void RemoveDanglingRemoteObjects()
        {
            lock (this)
            {
                List<string> keys = new List<string>();

                // find keys to delete
                foreach (string key in proxies.Keys)
                {
                    try
                    {
                        // dummy read on proxy object to trigger exception
                        string sink = proxies[key].Sink;
                    }
                    catch (SocketException)
                    {
                        keys.Add(key);
                    }
                }
                // then modify dictionary
                foreach (string key in keys)
                {
                    Console.WriteLine("Removed key: {0}", key);
                    proxies.Remove(key);
                }
            }
        }

        // called from client to publish a messsage
        public void DispatchCall(EventProxy proxy, Object data)
        {
            lock (this)
            {
                /* if (!proxies.ContainsKey(proxy.Sink))
                {
                    OnClientAdded(new ClientAddedEventArgs(proxy.Sink));
                } */
                proxies[proxy.Sink] = proxy;
                OnMessageReceived(new MessageReceivedEventArgs(proxy.Sink, data));
            }
        }

        // called from server/client to send client an event
        public void DispatchEvent(String sink, Object data)
        {
            lock (this)
            {
                try
                {
                    if (proxies.ContainsKey(sink))
                    {
                        proxies[sink].DispatchEvent(new EventDispatchedEventArgs(sink, data));
                    }
                    else
                    {
                        throw new SinkNotFoundException();
                    }
                }
                catch (SocketException)
                {
                    proxies.Remove(sink);
                    throw;
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        public override sealed object InitializeLifetimeService()
        {
            // indicate that this lease never expires
            return null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            Disconnect();
            disposed = true;
        }

        private void Disconnect()
        {
            RemotingServices.Disconnect(this);
            foreach (MarshalByRefObject byRefObject in NestedMarshalByRefObjects)
            {
                RemotingServices.Disconnect(byRefObject);
            }
        }

        private void OnClientAdded(ClientAddedEventArgs e)
        {
            if (ClientAdded != null)
            {
                // asynchronous event dispatching
                ClientAdded.BeginInvoke(this, e, null, null);
            }
        }

        private void OnMessageReceived(MessageReceivedEventArgs e)
        {
            lock (this)
            {
                if (MessageReceived != null)
                {
                    // asynchronous event dispatching
                    MessageReceived.BeginInvoke(this, e, null, null);
                }
            }
        }

        #endregion Methods
    }
}