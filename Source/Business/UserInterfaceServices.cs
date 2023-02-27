namespace Covidien.CGRS.PcAgentInterfaceBusiness
{
    public abstract class UserInterfaceServices
    {
        /// <summary>
        /// The delegate to be invoked when a connection has occurred.  Pass "null" to clear it.
        /// Recommend setting this AFTER the initial connection has been performed.  Its value
        /// is when we try to automatically reconnect after having encountered a disconnection.
        /// </summary>
        public UserInterfaceDelegates.ConnectCallback ConnectCallback { get; set; }

        /// <summary>
        /// The delegate to be invoked when a connection has occurred.  Pass "null" to clear it.
        /// </summary>
        public UserInterfaceDelegates.DisconnectCallback DisconnectCallback { get; set; }

        /// <summary>
        /// The delegate to be invoked when a connection has occurred.  Pass "null" to clear it.
        /// </summary>
        public UserInterfaceDelegates.InternalMessageCallback InternalMessageCallback { get; set; }

    }
}
