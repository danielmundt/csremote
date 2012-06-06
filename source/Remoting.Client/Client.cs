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
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Remoting.Interface;
using Remoting.Interface.Enums;

namespace Remoting.Client
{
	class Client
	{
		#region Delegates

		delegate bool RemoteAsyncDelegate(Command command);

		#endregion Delegates

		#region Methods

		public void RegisterChannel()
		{
			HttpChannel httpChannel = new HttpChannel();
			ChannelServices.RegisterChannel(httpChannel, false);
		}

		public void SendCommand(Command command)
		{
			ICommand remoteObject = (ICommand)Activator.GetObject(typeof(ICommand),
				string.Format("http://localhost:{0}/{1}", Constants.HttpPort, Constants.ObjectUri));
			if (remoteObject != null)
			{
				AsyncCallback remoteCallback = new AsyncCallback(this.RemoteCallback);
				RemoteAsyncDelegate remoteDelegate = new RemoteAsyncDelegate(remoteObject.SendCommand);
				IAsyncResult result = remoteDelegate.BeginInvoke(command, remoteCallback, null);
			}
		}

		void RemoteCallback(IAsyncResult result)
		{
			RemoteAsyncDelegate remoteDelegate = (RemoteAsyncDelegate)((AsyncResult)result).AsyncDelegate;
			Console.WriteLine(string.Format("Async result: {0}", remoteDelegate.EndInvoke(result)));
		}

		#endregion Methods
	}
}