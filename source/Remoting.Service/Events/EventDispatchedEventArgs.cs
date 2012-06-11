using System;
using System.Collections.Generic;
using System.Text;

namespace Remoting.Service.Events
{
	[Serializable]
	public class EventDispatchedEventArgs : EventArgs
	{
		#region Fields

		private Object data;
		private String sink;

		#endregion Fields

		#region Constructors

		public EventDispatchedEventArgs(String sink, Object data)
		{
			this.sink = sink;
			this.data = data;
		}

		#endregion Constructors

		#region Properties

		public String Sink
		{
			get
			{
				return sink;
			}
		}

		public Object Data
		{
			get
			{
				return data;
			}
		}

		#endregion Properties
	}
}