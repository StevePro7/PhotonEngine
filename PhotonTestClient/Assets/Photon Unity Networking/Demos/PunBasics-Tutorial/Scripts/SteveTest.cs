using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.Demos.DemoAnimator
{

	public class SteveTest : MonoBehaviour
	{
		[Tooltip("The UI Loader Anime")]
		public LoaderAnime loaderAnime;

		private string resultText = "default";

		void Start()
		{
		}

		void Update()
		{
		}

		private void OnGUI()
		{
			float xPos = 5.0f;
			float yPos = 5.0f;

			float width = Screen.width;
			float height = Screen.height / 2;

			GUI.skin.button.fontSize = 30;
			GUI.skin.button.fontStyle = FontStyle.Bold;

			if (GUI.Button(new Rect(xPos, yPos, width, height), "START"))
			{
				resultText = "adriana";
				if (loaderAnime != null)
				{
					loaderAnime.StartLoaderAnimation();
				}
			}

			if (GUI.Button(new Rect(xPos, yPos + height, width, height), "STOP"))
			{
				resultText = "stevepro";
				if (loaderAnime != null)
				{
					loaderAnime.StopLoaderAnimation();
				}
			}

			GUI.skin.label.fontSize = 50;
			GUI.Label(new Rect(xPos, yPos, width, height), resultText);
		}
	}

}