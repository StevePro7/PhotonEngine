﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace SteveProStudios.AnTutorialTest
{
	public class GameManager : Photon.PunBehaviour
	{

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public override void OnLeftRoom()
		{
			SceneManager.LoadScene(0);
		}

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		private void Update()
		{
		}


		/// <summary>
		/// Called when a Photon Player got connected. We need to then load a bigger scene.
		/// </summary>
		/// <param name="other">Other.</param>
		public override void OnPhotonPlayerConnected(PhotonPlayer other)
		{
			Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting

			if (PhotonNetwork.isMasterClient)
			{
				Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
				LoadArena();
			}
		}

		/// <summary>
		/// Called when a Photon Player got disconnected. We need to load a smaller scene.
		/// </summary>
		/// <param name="other">Other.</param>
		public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
		{
			Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects

			if (PhotonNetwork.isMasterClient)
			{
				Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected

				LoadArena();
			}
		}



		private void LoadArena()
		{
			if (!PhotonNetwork.isMasterClient)
			{
				Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}

			Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
			PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.room.PlayerCount);
		}

	}
}
