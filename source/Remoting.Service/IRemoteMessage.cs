using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remoting.Service
{
	public interface IRemoteMessage
	{
		#region Methods

		void Send(object o);

		#endregion Methods
	}
}