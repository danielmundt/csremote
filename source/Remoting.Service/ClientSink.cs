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
using System.Text;

using Remoting.Service.Events;

namespace Remoting.Service
{
	public class ClientSink : MarshalByRefObject
	{
		#region Fields

		private String name;

		#endregion Fields

		#region Constructors

		public ClientSink(String name)
		{
			this.name = name;
		}

		#endregion Constructors

		#region Events

		public event EventHandler<EventDispatchedEventArgs> EventDispatched;

		#endregion Events

		#region Properties

		public EventHandler<EventDispatchedEventArgs> EventHandler
		{
			get
			{
				return EventDispatched;
			}
		}

		public String Name
		{
			get
			{
				return name;
			}
		}

		#endregion Properties

		#region Methods

		public void DispatchEvent(EventDispatchedEventArgs e)
		{
			if (EventDispatched != null)
			{
				// asynchronous event dispatching
				EventDispatched.BeginInvoke(this, e, null, null);
			}
		}

		public override bool Equals(Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			ClientSink other = obj as ClientSink;
			if ((Object)other == null)
			{
				return false;
			}
			// return true if the fields match:
			return ((name == other.Name) && (EventHandler == other.EventHandler));
		}

		public bool Equals(ClientSink other)
		{
			if ((object)other == null)
			{
				return false;
			}
			// return true if the fields match
			return ((name == other.Name) && (EventHandler == other.EventHandler));
		}

		public override int GetHashCode()
		{
			return (name.GetHashCode() ^ EventHandler.GetHashCode());
		}

		#endregion Methods
	}
}