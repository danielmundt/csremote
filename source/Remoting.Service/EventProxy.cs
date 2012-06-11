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
	public class EventProxy : MarshalByRefObject
	{
		#region Fields

		private String sink;

		#endregion Fields

		#region Constructors

		public EventProxy(String sink)
		{
			this.sink = sink;
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

		public String Sink
		{
			get
			{
				return sink;
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

			EventProxy other = obj as EventProxy;
			if ((Object)other == null)
			{
				return false;
			}
			// return true if the fields match
			return ((sink == other.Sink) && (EventHandler == other.EventHandler));
		}

		public bool Equals(EventProxy other)
		{
			if ((object)other == null)
			{
				return false;
			}
			// return true if the fields match
			return ((sink == other.Sink) && (EventHandler == other.EventHandler));
		}

		public override int GetHashCode()
		{
			return (sink.GetHashCode() ^ EventHandler.GetHashCode());
		}

		public override object InitializeLifetimeService()
		{
			// indicate that this lease never expires
			return null;
		}

		#endregion Methods
	}
}
