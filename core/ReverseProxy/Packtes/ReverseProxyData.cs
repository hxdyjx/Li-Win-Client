using ClientWindows.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientWindows.Core.ReverseProxy.Packtes
{
    internal class ReverseProxyData
    {
        public int ConnectedId { get; set; }        
        public byte[] Data { get; set; }

        public ReverseProxyData() { }
        public ReverseProxyData(int connectionId, byte[] data)
        {
            this.ConnectedId = connectionId;
            this.Data = data;
        }
        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
