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
using System.Threading;
using System.Windows.Forms;

using Remoting.Core;
using Remoting.Core.Channels;
using Remoting.Core.Events;
using Remoting.Core.Exceptions;

namespace Remoting.Client
{
	public partial class FormMain : Form
	{
		#region Fields

		private IRemoteService service;

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

		private void btnSend_Click(object sender, EventArgs e)
		{
			DispatchCall();
		}

		private void DispatchCall()
		{
            try
            {
                EventProxy proxy = new EventProxy(tbClientId.Text);
                proxy.EventDispatched += new EventHandler<EventDispatchedEventArgs>(proxy_EventDispatched);
                service.DispatchCall(proxy, "Hello World");
            }
            catch (RemotingException ex)
            {
                MessageBox.Show(this, ex.Message, "Error");
            }
            catch (SinkNotFoundException ex)
            {
                MessageBox.Show(this, ex.Message, "Error");
            }
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			tbClientId.Text = Guid.NewGuid().ToString("N");
		}

		private void InitializeClient()
		{
			ClientChannel channel = (ClientChannel)ChannelFactory.GetChannel(
				ChannelFactory.Type.Client);
			if (channel != null)
			{
				service = channel.Initialize();
			}
		}

		private void proxy_EventDispatched(object sender, EventDispatchedEventArgs e)
		{
			SetText(string.Format("EventDispatched: {0}{1}",
				(string)e.Data, Environment.NewLine));
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