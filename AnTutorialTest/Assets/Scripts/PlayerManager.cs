using UnityEngine;

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
			if (this.Beams == null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> Beams Reference.", this);
			}
			else
			{
				this.Beams.SetActive(false);
			}
		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// </summary>
		void Update()
		{
			ProcessInputs();

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