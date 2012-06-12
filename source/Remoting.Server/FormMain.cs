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
using System.Data;
using System.Drawing;
using System.Runtime.Remoting;
using System.Text;
using System.Windows.Forms;

using Remoting.Core;
using Remoting.Core.Events;

namespace Remoting.Server
{
	public partial class FormMain : Form
	{
		#region Fields

		private RemoteService service;

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
			ServerChannel channel = (ServerChannel)ChannelFactory.GetChannel(
				ChannelFactory.Type.Server);
			if (channel != null)
			{
				service = channel.Initialize() as RemoteService;
				if (service != null)
				{
					service.ClientAdded += new EventHandler<ClientAddedEventArgs>(service_ClientAdded);
					service.MessageReceived += new EventHandler<MessageReceivedEventArgs>(service_MessageReceived);
					channel.RegisterService(service);
				}
			}
		}

		void service_ClientAdded(object sender, ClientAddedEventArgs e)
		{
			SetText(string.Format("Client ID registered: {0}{1}",
				e.Proxy.Sink, Environment.NewLine));
		}

		void service_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			SetText(string.Format("Message arrived: {0}{1}",
				e.Data, Environment.NewLine));

			// echo message to subscribed client
			service.DispatchEvent(e.Sink, e.Data);
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