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
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Remoting.Service;
using Remoting.Service.Enums;

namespace Remoting.Client
{
	class Client
	{
		#region Delegates

		public delegate int AsyncCommandDelegate(Command command);

		#endregion Delegates

		#region Events

		public event EventHandler<AsyncCompletedEventArgs> CommandCompleted;

		#endregion Events

		#region Methods

		public void RegisterChannel()
		{
			HttpChannel httpChannel = new HttpChannel();
			ChannelServices.RegisterChannel(httpChannel, false);
		}

		public void SendCommand(Command command)
		{
			ICommand remoteObject = (ICommand)Activator.GetObject(typeof(ICommand),
				string.Format("http://localhost:{0}/{1}", Constants.ServerHttpPort, Constants.CommandServiceUri));
			if (remoteObject != null)
			{
				AsyncCallback remoteCallback = new AsyncCallback(RemoteCallback);
				AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(null);
				AsyncCommandDelegate remoteDelegate = new AsyncCommandDelegate(remoteObject.SendCommand);
				IAsyncResult result = remoteDelegate.BeginInvoke(command, remoteCallback, asyncOperation);
			}
		}

		private void OnCommandCompleted(AsyncCompletedEventArgs e)
		{
			if (CommandCompleted != null)
			{
				CommandCompleted(this, e);
			}
		}

		private void RemoteCallback(IAsyncResult asyncResult)
		{
			try
			{
				// asynchronously execute remote command
				AsyncCommandDelegate remoteDelegate =
					(AsyncCommandDelegate)((AsyncResult)asyncResult).AsyncDelegate;
				object userState = remoteDelegate.EndInvoke(asyncResult);
				AsyncCompletedEventArgs completedArgs = new AsyncCompletedEventArgs(null, false, userState);

				// raise the completed event
				AsyncOperation asyncOperation = (AsyncOperation)asyncResult.AsyncState;
				asyncOperation.PostOperationCompleted(delegate(object e)
					{ OnCommandCompleted((AsyncCompletedEventArgs)e); }, completedArgs);
			}
			catch (System.Net.WebException exception)
			{
				Console.WriteLine("Error: " + exception.Message);
			}
		}

		#endregion Methods
	}
}