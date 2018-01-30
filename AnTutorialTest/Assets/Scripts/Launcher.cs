using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteveProStudios.AnTutorialTest
{
	public class Launcher : MonoBehaviour
	{
		/// <summary>
		/// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
		/// </summary>
		string _gameVersion = "1";

		private void Awake()
		{
			// We don't join the lobby.  There is no need to join a lobby to get the list of rooms
			PhotonNetwork.autoJoinLobby = false;

			// This makes sure we can use LoadLevel on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.automaticallySyncScene = true;
		}

		private void Start()
		{
			Connect();
		}

		// If already connected then attempt to join a random room
		// If not yet connected then connect this application instance to Photon Cloud Network
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
	}
}