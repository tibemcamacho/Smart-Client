using System.Net;

namespace MediaSuite.Common.Rtsp
{
    public class RtspPlayerConfiguration
    {
        /// <summary>
        ///     Password for authentication. Leave null for none.
        /// </summary>
        public string Password;

        /// <summary>
        ///     Proxy or Endpoint to route packets through. Leave null for none.
        /// </summary>
        public EndPoint Proxy;

        /// <summary>
        ///     RTSP Uri
        /// </summary>
        public string ResourceUri;

        /// <summary>
        ///     Username for authentication. Leave null for none.
        /// </summary>
        public string Username;
    }
}