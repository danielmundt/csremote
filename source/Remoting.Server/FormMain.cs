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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Windows.Forms;

using Remoting.Server;
using Remoting.Server.Events;
using Remoting.Service;

namespace Remoting.Server
{
	public partial class FormMain : Form
	{
		#region Fields

		private RemoteService remoteMessage;

		#endregion Fields

		#region Constructors

		public FormMain()
		{
			InitializeComponent();
			InitializeServer();
		}

		#endregion Constructors

		#region Delegates

		delegate void SetTextCallback(string text);

		#endregion Delegates

		#region Methods

		private void InitializeServer()
		{
			// set channel properties
			IDictionary props = new Hashtable();
			props["port"] = 9001;
			props["name"] = "server";
			// props["machineName"] = "localhost";

			// create custom formatter
			BinaryServerFormatterSinkProvider sinkProvider = new BinaryServerFormatterSinkProvider();
			sinkProvider.TypeFilterLevel = TypeFilterLevel.Full;

			// create and register the server channel
			TcpServerChannel serverChannel = new TcpServerChannel(props, sinkProvider);
			ChannelServices.RegisterChannel(serverChannel, false);

			remoteMessage = new RemoteService();
			remoteMessage.ClientAdded += new EventHandler<ClientAddedEventArgs>(remoteMessage_ClientAdded);
			remoteMessage.MessageReceived += new EventHandler<MessageReceivedEventArgs>(remoteMessage_MessageReceived);

			// publish a specific object instance
			RemotingServices.Marshal(remoteMessage, "service.rem");
		}

		void remoteMessage_ClientAdded(object sender, ClientAddedEventArgs e)
		{
			SetText(string.Format("Client ID registered: {0}{1}",
				e.Proxy.Sink, Environment.NewLine));
		}

		void remoteMessage_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			SetText(string.Format("Message arrived: {0}{1}",
				e.UserObject, Environment.NewLine));

			// echo message to subscribed client
			remoteMessage.DispatchEvent(e.Sink, e.UserObject);
		}

		private void SetText(string text)
		{
			// thread-safe call
			if (tbLog.InvokeRequired)
			{
				SetTextCallback d = new SetTextCallback(SetText);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				tbLog.AppendText(text);
			}
		}

		#endregion Methods
	}
}