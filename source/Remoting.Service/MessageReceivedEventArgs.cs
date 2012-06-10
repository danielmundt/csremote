using System;
using System.Collections.Generic;
using System.Text;

namespace Remoting.Service
{
    public class MessageReceivedEventArgs : EventArgs
    {
        private object userObject;

        public MessageReceivedEventArgs(Object userObject)
        {
            this.userObject = userObject;
        }

        public Object UserObject
        {
            get
            {
                return userObject;
            }
        }
    }
}
