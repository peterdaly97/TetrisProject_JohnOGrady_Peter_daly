using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	string[] names = {"Prefabs/Tetromino_T", "Prefabs/Tetromino_I", "Prefabs/Tetromino_J", "Prefabs/Tetromino_bs", "Prefabs/Tetromino_s", "Prefabs/Tetromino_Sq"};
    public static int gridHeight= 20;
    public static int gridWidth = 10;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

	// Use this for initialization
	void Start () 
	{
		GenerateNext();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    public bool CheckAboveGrid(Tetromino tetro)
    {
        for(int i = 0; i < gridWidth; ++i)
        {
            foreach (Transform mino in tetro.transform)
            {
                Vector2 pos = Round(mino.position);
                if(pos.y > gridHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsRowFull(int y)
    {
        for(int i = 0; i < gridWidth; ++i)
        {
            if(grid[i,y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void DeleteMino(int y)
    {
        for (int i = 0; i < gridWidth; ++i)
        {
            Destroy(grid[i, y].gameObject);
            grid[i, y] = null;
        }
    }

    public void MoveRowsDown(int y)
    {
        for (int i = 0; i < gridWidth; ++i)
        {
            if(grid[i,y] != null)
            {
                grid[i, y - 1] = grid[i, y];
                grid[i, y] = null;
                grid[i, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for(int j = y; j < gridHeight; ++j)
        {
            MoveRowsDown(j);
        }
    }

    public void DeleteRow()
    {
        for (int i = 0; i < gridHeight; ++i)
        {
            if(IsRowFull(i))
            {
                DeleteMino(i);
                MoveAllRowsDown(i + 1);
                --i;
            }
        }
    }

    public void UpdateGrid(Tetromino tetro)
    {
        for(int i = 0; i < gridHeight; i++)
        {
            for(int j = 0; j < gridWidth; j++)
            {
                if(grid[j,i] != null && grid[j,i].parent == tetro.transform)
                {
                    grid[j, i] = null;
                }
            }
        }
        foreach (Transform mino in tetro.transform)
        {
            Vector2 pos = Round(mino.position);
            if(pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform GetTransformAtGrid(Vector2 pos)
    {
        if(pos.y > gridHeight - 1)
        {
            return null;
        }
        return grid[(int)pos.x, (int)pos.y];
    }

	public void GenerateNext()
	{
		GameObject instance = (GameObject)Instantiate(Resources.Load(names[Random.Range(0, 5)], typeof(GameObject)), new Vector3(5.0f, 22.0f, -3.0f), Quaternion.identity);
	}

    public bool CheckIsInsideGrid(Vector2 position)
    {
        return ((int)position.x >= 0 && (int)position.x < gridWidth && (int)position.y >= 0);
    }

    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
