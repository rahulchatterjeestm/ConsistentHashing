using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsistentHashing
{
    public class ConsistentHash
    {
        private SortedDictionary<uint, Server> hashRing;
        private readonly int numberOfReplicas;

        public ConsistentHash(int numberOfReplicas, List<Server> servers)
        {
            this.numberOfReplicas = numberOfReplicas;
            hashRing = new SortedDictionary<uint, Server>();

            if(servers != null)
                foreach (Server s in servers)
                {
                    this.AddServerToRing(s);
                }
        }

        private void AddServerToRing(Server server)
        {
            for (int i =0; i<numberOfReplicas; i++)
            {
                string serverIdentity = String.Concat(server.IpAddress, ":", i);
                uint hashKey = FnvHash.To32BitFnvHash(serverIdentity);
                this.hashRing.Add(hashKey, server);
            }
        }

        private void RemoveServerFromRing(Server server)
        {
            for (int i = 0; i < numberOfReplicas; i++)
            {
                string serverIdentity = String.Concat(server.IpAddress, ":", i);
                uint hashKey = FnvHash.To32BitFnvHash(serverIdentity);
                this.hashRing.Remove(hashKey);
            }
        }

        public Server GetServerForKey(String key)
        {
            Server serverHoldingKey;

            if( this.hashRing.Count == 0)
            {
                return null;
            }

            uint hashKey = FnvHash.To32BitFnvHash(key);

            if(this.hashRing.ContainsKey(hashKey))
            {
                serverHoldingKey = this.hashRing[hashKey];
            }
            else
            {
                uint[] sortedKeys = this.hashRing.Keys.ToArray();

                uint firstServerKey = sortedKeys.FirstOrDefault(x => x >= hashKey);

                serverHoldingKey = this.hashRing[firstServerKey];
            }
            return serverHoldingKey;
        }
    }
}
