using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteveProStudios.AnTutorialTest
{
	public class Launcher : Photon.PunBehaviour
	{
		/// <summary>
		/// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
		/// </summary>
		string _gameVersion = "1";

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		private void Awake()
		{
			// We don't join the lobby.  There is no need to join a lobby to get the list of rooms
			PhotonNetwork.autoJoinLobby = false;

			// This makes sure we can use LoadLevel on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.automaticallySyncScene = true;
		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase.
		/// </summary>
		private void Start()
		{
			Connect();
		}

		/// <summary>
		/// Start the connection process. 
		/// - If already connected, we attempt joining a random room
		/// - if not yet connected, Connect this application instance to Photon Cloud Network
		/// </summary>
		private void Connect()
		{
			// Check if connected or not: join if we are connected else initiate the connection to the server
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				PhotonNetwork.ConnectUsingSettings(_gameVersion);
			}
		}

		/// <summary>
		/// Called after the connection to the master is established and authenticated but only when PhotonNetwork.autoJoinLobby is false.
		/// </summary>
		public override void OnConnectedToMaster()
		{
			Debug.LogWarning("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");

			// The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()  
			PhotonNetwork.JoinRandomRoom();
		}

		public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
			Debug.LogError("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
		}

		/// <summary>
		/// Called after disconnecting from the Photon server.
		/// </summary>
		/// <remarks>
		/// In some cases, other callbacks are called before OnDisconnectedFromPhoton is called.
		/// Examples: OnConnectionFail() and OnFailedToConnectToPhoton().
		/// </remarks>
		public override void OnDisconnectedFromPhoton()
		{
			Debug.LogError("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
		}

		/// <summary>
		/// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
		/// </summary>
		/// <remarks>
		/// This method is commonly used to instantiate player characters.
		/// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
		///
		/// When this is called, you can usually already access the existing players in the room via PhotonNetwork.playerList.
		/// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
		/// enough players are in the room to start playing.
		/// </remarks>
		public override void OnJoinedRoom()
		{
			Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() was called by PUN.  Now this client is in a room");
		}

	}
}