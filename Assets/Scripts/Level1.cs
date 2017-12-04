using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1 : MonoBehaviour 
{
	// single script to launch the scene corresponding to the first level of the game
	public void LoadGame()
	{
		SceneManager.LoadScene("Level");
	}
}
