using System;

namespace ConsistentHashing
{
    public class Server
    {
        public string IpAddress { get; set; }

        public Server(string ipAddress)
        {
            this.IpAddress = ipAddress;
        }
    }
}
