using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
	// single script to launch the scene corresponding to go to the start of the game
    public void LoadGame()
    {
        SceneManager.LoadScene("startGame");
    }
}
