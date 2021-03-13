using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;
using System.Collections;


namespace socket.io {

    /// <summary>
    /// Streams out received packets as observable
    /// </summary>
    public class WebSocketTrigger : ObservableTriggerBase {

        // [Serializable]
        // public class PongData
        // {
        //     public int serverProcessMsTime = 0;

        //     public override string ToString()
        //     {
        //         return "serverProcessMsTime=" + serverProcessMsTime.ToString();
        //     }
        // }

        // private float pingStartTime = 0.0f;
        // private float clientToServerLatencySec = 0.0f;  // client -> server 까지 도착까지 걸린 지연 시간

        /// <summary>
        /// Observes received packets and also starts Ping-Pong routine
        /// </summary>
        /// <returns></returns>
        public UniRx.IObservable<string> OnRecvAsObservable() {
            if (_cancelPingPong == null) {
                _cancelPingPong = gameObject.UpdateAsObservable()
                    .Sample(TimeSpan.FromSeconds(10f))
                    .Subscribe(_ => {
                        
                        // pingStartTime = Time.time;

                        WebSocket.Send(Packet.Ping);
                        //Debug.LogFormat("socket.io => {0} ping~", WebSocket.Url.ToString());
                    });
            }

            if (_onRecv == null)
                _onRecv = new Subject<string>();

            return _onRecv;
        }
        
        protected override void RaiseOnCompletedOnDestroy() {
            if (_cancelPingPong != null) {
                _cancelPingPong.Dispose();
                _cancelPingPong = null;
            }

            if (_onRecv != null) {
                _onRecv.OnCompleted();
                _onRecv = null;
            }

            if (!IsConnected)
                WebSocket.Close();
        }

        IDisposable _cancelPingPong;
        Subject<string> _onRecv;

        /// <summary>
        /// WebSocket object ref
        /// </summary>
        public WebSocketWrapper WebSocket { get; set; }

        /// <summary>
        /// Holds the last error on WebSocket
        /// </summary>
        public string LastWebSocketError { get; private set; }

        public bool IsConnected {
            get { return WebSocket != null && WebSocket.IsConnected; }
        }

        public bool IsProbed { get; set; }

        public bool IsUpgraded { get; set; }
        

        void Update() {
            LastWebSocketError = WebSocket.GetLastError();

            if (!string.IsNullOrEmpty(LastWebSocketError)) {
                CheckAndHandleWebSocketDisconnect();
                Debug.LogError(LastWebSocketError);
            }

            if (IsConnected)
                ReceiveWebSocketData();
        }

        void ReceiveWebSocketData() {
            var recvData = WebSocket.Recv();

            if (string.IsNullOrEmpty(recvData))
                return;

            if (recvData == Packet.ProbeAnswer) {
                IsProbed = true;
                Debug.LogFormat("socket.io => {0} probed~", WebSocket.Url.ToString());
            }
            else if (recvData == Packet.Pong) {
                // Debug.Log("pong.recvData=" + recvData.ToString());
                Debug.LogFormat("socket.io => {0} pong~", WebSocket.Url.ToString());
            }
            // else if (recvData[0] == Packet.Pong[0]) {
            //     Debug.Log("pong(custom).recvData=" + recvData.ToString());
                
            //     string dataString = recvData.Substring(1, recvData.Length-1);
            //     Debug.Log("pong(custom).dataString=" + dataString.ToString());

            //     Debug.LogFormat("socket.io => {0} pong~ : time {1}", WebSocket.Url.ToString(), Time.time - this.pingStartTime);

            //     PongData pongData = JsonUtility.FromJson<PongData>(dataString);
            //     Debug.Log("pong(custom).pongData=" + pongData.ToString());

            //     // Client -> Server 도착 시간 = [(PingPong 도착 시간 - Ping 요청 시간) - (Pong Server 보낸시간 - Ping Server 도착시간)] * 0.5f
            //     float serverProcessSecTime = (float)pongData.serverProcessMsTime * 0.001f;
            //     this.clientToServerLatencySec = ((Time.time - this.pingStartTime) - serverProcessSecTime) * 0.5f;

            //     Debug.LogFormat("pong(custom).clientToServerLatencySec=" + this.clientToServerLatencySec.ToString());
            // }
            else {
                if (_onRecv != null)
                    // Debug.Log("data.recvData=" + recvData.ToString());
                    _onRecv.OnNext(recvData);
            }
        }
        
        void CheckAndHandleWebSocketDisconnect() {
            if (IsConnected)
                return;

            if (_onRecv != null) {
                _cancelPingPong.Dispose();
                _cancelPingPong = null;
                _onRecv.Dispose();
                _onRecv = null;
                IsProbed = false;
                IsUpgraded = false;

                var sockets = gameObject.GetComponentsInChildren<Socket>();
                foreach (var s in sockets) {
                    if (s.OnDisconnect != null)
                        s.OnDisconnect();
                }
            }
            
            if (SocketManager.Instance.Reconnection) {
                var sockets = gameObject.GetComponentsInChildren<Socket>();
                foreach (var s in sockets)
                    SocketManager.Instance.Reconnect(s, 1);
            }
        }
        
    }

}