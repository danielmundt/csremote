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
using System.Runtime.Remoting;
using System.Text;

using Remoting.Core.Events;

namespace Remoting.Core
{
	public class RemoteService : MarshalByRefObject, IRemoteService, IDisposable
	{
		#region Fields

		private bool disposed = false;
		private List<EventProxy> eventProxies = new List<EventProxy>();

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

		// called from client to publish a messsage
		public void DispatchCall(EventProxy proxy, Object data)
		{
			// register client and notify listeners
			if (!eventProxies.Contains(proxy))
			{
				eventProxies.Add(proxy);
				OnClientAdded(new ClientAddedEventArgs(proxy));
			}
			OnMessageReceived(new MessageReceivedEventArgs(proxy.Sink, data));
		}

		// called from server/client to send client an event
		public void DispatchEvent(String sink, Object data)
		{
			foreach (EventProxy proxy in eventProxies)
			{
				if ((proxy.Sink == sink) || (proxy.Sink == String.Empty))
				{
					proxy.DispatchEvent(new EventDispatchedEventArgs(proxy.Sink, data));
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
			foreach (var tmp in NestedMarshalByRefObjects)
			{
				RemotingServices.Disconnect(tmp);
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
			if (MessageReceived != null)
			{
				// asynchronous event dispatching
				MessageReceived.BeginInvoke(this, e, null, null);
			}
		}

		#endregion Methods
	}
}