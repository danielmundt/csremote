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
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Windows.Forms;

namespace Remoting.Server
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            CreateServer();
        }

        private void CreateServer()
        {
            try
            {
                TcpChannel tcpChannel = new TcpChannel(9000);
                ChannelServices.RegisterChannel(tcpChannel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(
                  typeof(Remoting.Interface.ICommand),
                  "RemotingExample/Command.rem", WellKnownObjectMode.SingleCall);

                Command command = new Command();
                RemotingServices.Marshal(command, "RemotingExample/Command.rem");
            }
            catch (SocketException)
            {
            }
        }
    }
}
