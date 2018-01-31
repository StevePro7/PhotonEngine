﻿using UnityEngine;

namespace SteveProStudios.AnTutorialTest
{
	public class PlayerManager : Photon.PunBehaviour
	{
		[Tooltip("The Beams GameObject to control")]
		public GameObject Beams;

		[Tooltip("The current Health of our player")]
		public float Health = 1f;

		// True, when the user is firing
		private bool IsFiring;

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		private void Awake()
		{
			if (Beams == null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> Beams Reference.", this);
			}
			else
			{
				Beams.SetActive(false);
			}
		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase.
		/// </summary>
		private void Start()
		{
			CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
			if (_cameraWork != null)
			{
				if (photonView.isMine)
				{
					_cameraWork.OnStartFollowing();
				}
			}
			else
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
			}
		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// </summary>
		void Update()
		{
			// we only process Inputs and check health if we are the local player
			if (photonView.isMine)
			{
				ProcessInputs();

				if (Health <= 0f)
				{
					GameManager.Instance.LeaveRoom();
				}
			}

			// trigger Beans active state
			if (Beams != null && IsFiring != Beams.GetActive())
			{
				Beams.SetActive(IsFiring);
			}
		}

		/// <summary>
		/// MonoBehaviour method called when the Collider 'other' enters the trigger.
		/// Affect Health of the Player if the collider is a beam
		/// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
		/// One could move the collider further away to prevent this or check if the beam belongs to the player.
		/// </summary>
		private void OnTriggerEnter(Collider other)
		{
			if (!photonView.isMine)
			{
				return;
			}

			// We are only interested in Beamers
			// we should be using tags but for the sake of distribution, let's simply check by name.
			if (!other.name.Contains("Beam"))
			{
				return;
			}

			Health -= 0.1f;
		}

		/// <summary>
		/// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
		/// We're going to affect health while the beams are interesting the player
		/// </summary>
		/// <param name="other">Other.</param>
		private void OnTriggerStay(Collider other)
		{
			// we don't do anything if we are not the local player
			if (!photonView.isMine)
			{
				return;
			}

			// We are only interested in Beamers
			// we should be using tags but for the sake of distribution, let's simply check by name.
			if (!other.name.Contains("Beam"))
			{
				return;
			}

			// we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
			Health -= 0.1f * Time.deltaTime;
		}

		/// <summary>
		/// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
		/// </summary>
		private void ProcessInputs()
		{
			if (Input.GetButtonDown("Fire1"))
			{
				if (!IsFiring)
				{
					IsFiring = true;
				}
			}
			if (Input.GetButtonUp("Fire1"))
			{
				if (IsFiring)
				{
					IsFiring = false;
				}
			}
		}
	}

}