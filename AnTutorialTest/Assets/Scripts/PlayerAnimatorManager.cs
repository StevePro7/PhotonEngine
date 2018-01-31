using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
	public float DirectionDampTime = 0.25f;

	private Animator animator;

	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity during initialization phase.
	/// </summary>
	void Start ()
	{
		animator = GetComponent<Animator>();
		if (!animator)
		{
			Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// failSafe is missing Animator component on GameObject
		if (!animator)
		{
			return;
		}

		// deal with Jumping
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		// only allow jumping if we are running.
		if (stateInfo.IsName("Base Layer.Run"))
		{
			// When using trigger parameter
			if (Input.GetButtonDown("Fire2"))
			{
				animator.SetTrigger("Jump");
			}
		}

		// deal with movement
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		// prevent negative Speed.
		if (v < 0)
		{
			v = 0;
		}

		// set the Animator Parameters
		animator.SetFloat("Speed", h * h + v * v);
		animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);
	}
}
