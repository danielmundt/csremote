using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remoting.Service
{
	public class RemoteMessage : MarshalByRefObject
	{
		#region Events

		public event EventHandler<MessageReceivedEventArgs> MessageReceived;

		#endregion Events

		#region Methods

		public override object InitializeLifetimeService()
		{
			// indicate that this lease never expires
			return null;
		}

		public void Send(object o)
		{
			OnMessageReceived(new MessageReceivedEventArgs(o));
		}

		private void OnMessageReceived(MessageReceivedEventArgs e)
		{
			if (MessageReceived != null)
			{
				MessageReceived(this, e);
			}
		}

		#endregion Methods
	}
}