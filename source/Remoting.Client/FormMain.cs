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
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Remoting.Service;

namespace Remoting.Client
{
	public partial class FormMain : Form
	{
		#region Fields

		private RemoteMessage remoteMessage;
		private CallbackSink _CallbackSink = null;

		#endregion Fields

		#region Constructors

		public FormMain()
		{
			InitializeComponent();
			InitializeClient();
		}

		#endregion Constructors

		#region Delegates

		delegate void SetTextCallback(string text);

		#endregion Delegates

		#region Methods

		public void SendMessage()
		{
			try
			{
				if (remoteMessage != null)
				{
					remoteMessage.PushMessage(tbClientId.Text, "Hello World");
				}
			}
			catch (RemotingException ex)
			{
				MessageBox.Show(this, ex.Message, "Error");
			}
		}

		private void btnRegister_Click(object sender, EventArgs e)
		{
			// register client
			// remoteMessage = new RemoteMessage();
			//remoteMessage.MessageReceived +=
			//    new EventHandler<MessageReceivedEventArgs>(remoteMessage_MessageReceived);
			if (remoteMessage != null)
			{
				// remoteMessage.MessageReceived +=
				//    new EventHandler<MessageReceivedEventArgs>(remoteMessage_MessageReceived);
				remoteMessage.Register(tbClientId.Text,
					new delCommsInfo(_CallbackSink.HandleToClient));
			}
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			SendMessage();
		}

		void CallbackSink_OnHostToClient(Object obj)
		{
			int i = 0;
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			tbClientId.Text = Guid.NewGuid().ToString("N");
		}

		private void InitializeClient()
		{
			_CallbackSink = new CallbackSink();
			_CallbackSink.OnHostToClient += new delCommsInfo(CallbackSink_OnHostToClient);

			// create and register the channel
			IpcClientChannel clientChannel = new IpcClientChannel();
			ChannelServices.RegisterChannel(clientChannel, false);

			// create transparent proxy to server component
			remoteMessage = (RemoteMessage)Activator.GetObject(
				typeof(RemoteMessage), "ipc://remote/service");
		}

		private void remoteMessage_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			SetText("MessageReceived: " + (string)e.UserObject);
			SetText(Environment.NewLine);
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