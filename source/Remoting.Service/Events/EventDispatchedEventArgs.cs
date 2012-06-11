using System;
using System.Collections.Generic;
using System.Text;

namespace Remoting.Service.Events
{
	[Serializable]
	public class EventDispatchedEventArgs : EventArgs
	{
		#region Fields

		private String name;
		private Object userObject;

		#endregion Fields

		#region Constructors

		public EventDispatchedEventArgs(String name, Object userObject)
		{
			this.name = name;
			this.userObject = userObject;
		}

		#endregion Constructors

		#region Properties

		public string Name
		{
			get
			{
				return name;
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