using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remote.MessageBox
{
	public class MessageReceivedEventArgs : EventArgs
	{
		#region Fields

		private object userObject;

		#endregion Fields

		#region Constructors

		public MessageReceivedEventArgs(object userObject)
		{
			this.userObject = userObject;
		}

		#endregion Constructors

		#region Properties

		public object UserState
		{
			get
			{
				return userObject;
			}
		}

		#endregion Properties
	}
}