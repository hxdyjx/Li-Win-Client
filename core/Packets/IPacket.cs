using ClientWindows.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientWindows.Core.Packets
{
    public class IPacket
    {
        void Execute(Client client);
    }
}
