using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect : Photon.PunBehaviour
{
	string _gameVersion = "1";

	// Use this for initialization
	void Start ()
	{
		PhotonNetwork.ConnectUsingSettings(_gameVersion);
		PhotonNetwork.automaticallySyncScene = true;
	}
	
	// Update is called once per frame
	void OnGUI()
	{
		string label = PhotonNetwork.connectionStateDetailed.ToString();
		GUILayout.Label(label);
	}

	void OnJoinedLobby()
	{
		Debug.Log("I have joined the lobby");
	}
}
