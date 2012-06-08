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
		public FormMain()
		{
			InitializeComponent();
			InitializeClient();
		}

		delegate void SetTextCallback(string text);

		public void SendMessage(string message)
		{
			try
			{
                // create transparent proxy to server component
                RemoteMessage remoteMessage = (RemoteMessage)Activator.GetObject(
                    typeof(RemoteMessage), "tcp://localhost:9001/serverExample.Rem");
                remoteMessage.PublishMessage(message, "Hello World");
                remoteMessage.MessageArrived += new MessageArrivedEvent(eventProxy.OnMessageArrived);
            }
			catch (RemotingException ex)
			{
				MessageBox.Show(this, ex.Message, "Error");
			}
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
            SendMessage(tbClientId.Text);
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			tbClientId.Text = Guid.NewGuid().ToString("N");
		}

        private EventProxy eventProxy;

		private void InitializeClient()
		{
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            IDictionary props = new Hashtable();
            props["name"] = "remotingClient";
            props["port"] = 0;

			// create and register the channel
            TcpChannel clientChannel = new TcpChannel(props, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(clientChannel, false);

            RemotingConfiguration.RegisterWellKnownClientType(
                new WellKnownClientTypeEntry(typeof(RemoteMessage),
                    "tcp://localhost:9000/serverExample.Rem"));

            // create event proxy
            eventProxy = new EventProxy();
            eventProxy.MessageArrived += new MessageArrivedEvent(eventProxy_MessageArrived);
		}

        void eventProxy_MessageArrived(object obj)
        {
            // SetText("MessageReceived: " + (string)obj);
            SetText("MessageReceived");
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
	}
}