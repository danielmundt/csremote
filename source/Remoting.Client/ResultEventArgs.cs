using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remoting.Client
{
	public class ResultEventArgs : EventArgs
	{
		#region Fields

		private bool result;

		#endregion Fields

		#region Constructors

		public ResultEventArgs(bool result)
		{
			this.result = result;
		}

		#endregion Constructors

		#region Properties

		public bool Result
		{
			get
			{
				return result;
			}
			set
			{
				result = value;
			}
		}

		#endregion Properties
	}
}