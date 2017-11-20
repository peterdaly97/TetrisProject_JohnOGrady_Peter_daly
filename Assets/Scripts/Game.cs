using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	string[] names = {"Prefabs/Tetromino_T", "Prefabs/Tetromino_I", "Prefabs/Tetromino_J", "Prefabs/Tetromino_bs", "Prefabs/Tetromino_s", "Prefabs/Tetromino_Sq"};
    public static int gridHeight= 22;
    public static int gridWidth = 10;

	// Use this for initialization
	void Start () 
	{
		generateNext();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void generateNext()
	{
		GameObject instance = (GameObject)Instantiate(Resources.Load(names[Random.Range(0, 5)], typeof(GameObject)), new Vector3(5.0f, 20.0f, -3.0f), Quaternion.identity);
	}

    public bool CheckIsInsideGrid(Vector2 position)
    {
        return ((int)position.x >= 0 && (int)position.x < gridWidth && (int)position.y >= 0);
    }

    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }
}
