using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[Tooltip("The Beams GameObject to control")]
	public GameObject Beams;

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
	void Update ()
	{
		ProcessInputs();

		// trigger Beans active state
		if (Beams != null && IsFiring != Beams.GetActive())
		{
			Beams.SetActive(IsFiring);
		}
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
