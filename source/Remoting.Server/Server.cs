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
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;

using Remoting.Service;

namespace Remoting.Server
{
	class Server
	{
		#region Methods

		public void Create(MarshalByRefObject refObject)
		{
			// create and register the server channel
			IpcChannel serverChannel = new IpcChannel("remote");
			ChannelServices.RegisterChannel(serverChannel, false);

			// show the name of the channel
			Console.WriteLine("The name of the channel is {0}.",
				serverChannel.ChannelName);

			// show the priority of the channel
			Console.WriteLine("The priority of the channel is {0}.",
				serverChannel.ChannelPriority);

			// show the URIs associated with the channel
			ChannelDataStore channelData = (ChannelDataStore)serverChannel.ChannelData;
			foreach (string uri in channelData.ChannelUris)
			{
				Console.WriteLine("The channel URI is {0}.", uri);
			}

			// expose an object for remote calls
			RemotingConfiguration.RegisterWellKnownServiceType(
				typeof(Command), "command", WellKnownObjectMode.Singleton);
			RemotingServices.Marshal(refObject, "command");
		}

		#endregion Methods
	}
}