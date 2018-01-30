using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SteveProStudios.AnTutorialTest
{
	public class GameManager : MonoBehaviour
	{

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public void OnLeftRoom()
		{
			SceneManager.LoadScene(0);
		}

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}
	}
}
