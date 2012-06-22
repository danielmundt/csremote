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
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Remoting.Core.Exceptions
{
	[Serializable]
	public class SinkNotFoundException : Exception, ISerializable
	{
		#region Fields

		private string sink;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SinkNotFoundException"/> class.
		/// </summary>
		public SinkNotFoundException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SinkNotFoundException"/> class with
		/// a specified error message and a sink ID.
		/// </summary>
		public SinkNotFoundException(string message, string sink)
			: base(message)
		{
			this.sink = sink;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SinkNotFoundException"/> class with
		/// a specified error message.
		/// </summary>
		public SinkNotFoundException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SinkNotFoundException"/> class with
		/// a specified error message and a reference to the inner exception that is a cause
		/// of this exception.
		/// </summary>
		public SinkNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SinkNotFoundException"/> class with
		/// serialized data.
		/// </summary>
		protected SinkNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info != null)
			{
				sink = info.GetString("sink");
			}
		}

		#endregion Constructors

		#region Properties

		public string Sink
		{
			get
			{
				return sink;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// When overridden in a derived class, sets the <see cref="SinkNotFoundException"/>
		/// class with information about the exception. Performs a custom serialization.
		/// </summary>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			if (info != null)
			{
				info.AddValue("sink", sink);
			}
		}

		#endregion Methods
	}
}