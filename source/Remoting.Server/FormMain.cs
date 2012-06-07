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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Remoting.Service;

namespace Remoting.Server
{
	public partial class FormMain : Form
	{
		#region Constructors

		public FormMain()
		{
			InitializeComponent();
			InitializeServer();
		}

		#endregion Constructors

		#region Properties

		public TextBox LogTextBox
		{
			get
			{
				return this.tbLog;
			}
		}

		#endregion Properties

		#region Methods

		private void InitializeServer()
		{
            RemoteMessage remoteMessage = new RemoteMessage();
			remoteMessage.MessageReceived +=
				new EventHandler<MessageReceivedEventArgs>(MessageReceivedHandler);

			Server server = new Server();
			server.Create(remoteMessage);
		}

		private void MessageReceivedHandler(object sender, MessageReceivedEventArgs e)
		{
			tbLog.AppendText("MessageReceived");
			tbLog.AppendText(Environment.NewLine);
		}

		#endregion Methods
	}
}