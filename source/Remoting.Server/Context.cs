using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Remoting.Server
{
	public class Context
	{
		#region Fields

		private FormMain form;

		#endregion Fields

		#region Constructors

		public Context(FormMain form)
		{
			this.form = form;
		}

		#endregion Constructors

		#region Methods

		public void SetLog(string text)
		{
			form.LogTextBox.AppendText(text);
            form.LogTextBox.AppendText(Environment.NewLine);
		}

		#endregion Methods
	}
}