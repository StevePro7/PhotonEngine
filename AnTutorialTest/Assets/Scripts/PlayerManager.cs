#if UNITY_5 && (!UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 && !UNITY_5_3) || UNITY_2017
#define UNITY_MIN_5_4
#endif

using UnityEngine;

namespace SteveProStudios.AnTutorialTest
{
	public class PlayerManager : Photon.PunBehaviour, IPunObservable
	{
		[Tooltip("The Player's UI GameObject Prefab")]
		public GameObject PlayerUiPrefab;

		[Tooltip("The Beams GameObject to control")]
		public GameObject Beams;

		[Tooltip("The current Health of our player")]
		public float Health = 1f;

		[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
		public static GameObject LocalPlayerInstance;

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

			// #Important
			// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
			if (photonView.isMine)
			{
				PlayerManager.LocalPlayerInstance = this.gameObject;
			}

			// #Critical
			// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load
			DontDestroyOnLoad(this.gameObject);
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

			// Create the UI
			if (PlayerUiPrefab != null)
			{
				GameObject _uiGo = Instantiate(PlayerUiPrefab) as GameObject;
				_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
			}
			else
			{
				Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
			}

#if UNITY_MIN_5_4
			// Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
			{
				this.CalledOnLevelWasLoaded(scene.buildIndex);
			};
#endif
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
		/// MonoBehaviour method called after a new level of index 'level' was loaded.
		/// We recreate the Player UI because it was destroy when we switched level.
		/// Also reposition the player if outside the current arena.
		/// </summary>
		/// <param name="level">Level index loaded</param>
		private void CalledOnLevelWasLoaded(int level)
		{
			// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
			if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
			{
				transform.position = new Vector3(0f, 5f, 0f);
			}

			GameObject _uiGo = Instantiate(PlayerUiPrefab) as GameObject;
			_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
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

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				// We own this player: send the others our data
				stream.SendNext(IsFiring);
				stream.SendNext(Health);
			}
			else
			{
				// Network player, receive data
				IsFiring = (bool)stream.ReceiveNext();
				Health = (float)stream.ReceiveNext();
			}
		}
	}

}