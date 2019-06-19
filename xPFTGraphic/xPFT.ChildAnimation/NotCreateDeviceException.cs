using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPFT.Exceptions.ChildAnimation
{
    public class NotCreateDeviceException : Exception
    {
        #region Properties

        string message = "";
        /// <summary>
        /// The Message of Exception
        /// </summary>
        public override string Message
        {
            get
            {
                return message;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of Exception class
        /// </summary>
        /// <param name="Message"></param>
        public NotCreateDeviceException(string Message)
            : base(Message)
        {
            this.message = Message;
        }

        #endregion
    }
}
