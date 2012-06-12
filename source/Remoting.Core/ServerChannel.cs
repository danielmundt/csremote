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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Remoting.Core
{
	public class ServerChannel : IChannel
	{
		#region Methods

		public IRemoteService Initialize()
		{
			// set channel properties
			IDictionary props = new Hashtable();
			props["port"] = 9001;
			props["name"] = "server";

			// create custom formatter
			BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
			provider.TypeFilterLevel = TypeFilterLevel.Full;

			// create and register the server channel
			TcpServerChannel serverChannel = new TcpServerChannel(props, provider);
			ChannelServices.RegisterChannel(serverChannel, false);

			return new RemoteService();
		}

		public void RegisterService(RemoteService service)
		{
			// publish a specific object instance
			ObjRef objRef = RemotingServices.Marshal(service, "service.rem");
			Console.WriteLine("An instance of RemoteService type is published at {0}.", objRef.URI);
		}

		public void UnregisterService(RemoteService service)
		{
			// unpublish a specific object instance
			RemotingServices.Disconnect(service);
		}

		#endregion Methods
	}
}