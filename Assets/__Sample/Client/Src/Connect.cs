using UnityEngine;
using System.Collections;
using socket.io;
using System;

namespace Sample {

    /// <summary>
    /// The basic sample to show how to connect to a server
    /// </summary>
    public class Connect : MonoBehaviour {

        void Start() {

            var serverUrl = "http://node0.local:8080";
            var socket = Socket.Connect(serverUrl);

            var myName = 77;
			
            socket.On(SystemEvents.connect, () => {
                Debug.Log("Socket.io Connected: ID = " + myName);
                socket.Emit("connected", myName.ToString() ); // send name
            });
			
            socket.On("publish", (string data) => {
                Debug.Log("Socket.io Data: " + data);
            });
			
            socket.On(SystemEvents.reconnect, (int reconnectAttempt) => {
                Debug.Log("Socket.io Reconnected: Attempt = " + reconnectAttempt);
                socket.Emit("connected", myName.ToString() ); // send name again ...
            });

            socket.On(SystemEvents.disconnect, () => {
                Debug.Log("Socket.io Disconnected: ID = " + myName);
				Socket.Reconnect(socket); // reconnect
            });

			socket.On(SystemEvents.connectTimeOut , () => {
				Debug.Log("Socket.io Connection Timeout: serverUrl = " + serverUrl);
				Socket.Reconnect(socket); // reconnect
			});

			socket.On(SystemEvents.connectError , (Exception e) => {
				Debug.Log("Socket.io Connection Error: " + e.ToString());
				Socket.Reconnect(socket); // reconnect
			});
        }

    }

}
