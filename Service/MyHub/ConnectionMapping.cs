using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MyHub
{
    public class ConnectionMapping
    {
        private readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public int Count
        {
            get { return _connections.Count; }
        }
        
        public void Add(string key, string connectionId)
        {
            lock (_connections)
            {
                _connections.Add(key, connectionId);
            }
        }

        public string GetConnections(string key)
        {
            string connectionId;
            _connections.TryGetValue(key, out connectionId);
            return connectionId;
        }

        public void Remove(string key)
        {
            lock(_connections)
            {
                _connections.Remove(key);
            }
        }
    }
}
