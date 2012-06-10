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

namespace Remoting.Service
{
	[Serializable]
	public class ClientSink
	{
		private string name;
        private EventCallback callback;

		public ClientSink(string name, EventCallback callback)
		{
			this.name = name;
            this.callback = callback;
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

        public EventCallback Callback
        {
            get
            {
                return callback;
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

            return ((name == other.Name) && (callback == other.Callback));
		}

		public bool Equals(ClientSink other)
		{
			// If parameter is null return false:
			if ((object)other == null)
			{
				return false;
			}

			// Return true if the fields match:
            return ((name == other.Name) && (callback == other.Callback));
		}

		public override int GetHashCode()
		{
			return (name.GetHashCode() ^ callback.GetHashCode());
		}

        public void PublishEvent(Object obj)
        {
            if (callback != null)
            {
                callback(obj);
            }
        }
	}
}