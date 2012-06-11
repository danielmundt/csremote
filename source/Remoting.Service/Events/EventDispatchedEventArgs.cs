using System;
using System.Collections.Generic;
using System.Text;

namespace Remoting.Service.Events
{
	[Serializable]
	public class EventDispatchedEventArgs : EventArgs
	{
		#region Fields

		private String sink;
		private Object userObject;

		#endregion Fields

		#region Constructors

        public EventDispatchedEventArgs(String sink, Object userObject)
		{
			this.sink = sink;
			this.userObject = userObject;
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

		public Object UserObject
		{
			get
			{
				return userObject;
			}
		}

		#endregion Properties
	}
}