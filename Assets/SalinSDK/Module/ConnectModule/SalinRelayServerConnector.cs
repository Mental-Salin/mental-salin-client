using UnityEngine;

namespace SalinSDK
{
    public class SalinRelayServerConnector : IMessageServerConnectable
    {
        private bool isConnected = false;
        public bool IsConnected()
        {
            return isConnected;
        }

        public MessageServer Connect(string appToken, string userToken)
        {
            RelayServer relayServer = new RelayServer();
            relayServer.Connect(appToken, userToken , (connect) => { isConnected = connect; });

            return relayServer;
        }
    }

}
