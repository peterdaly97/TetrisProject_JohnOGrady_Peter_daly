using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	string[] names = {"Prefabs/Tetromino_T", "Prefabs/Tetromino_I", "Prefabs/Tetromino_J", "Prefabs/Tetromino_bs", "Prefabs/Tetromino_s", "Prefabs/Tetromino_Sq", "Prefabs/Tetromino_L", "Prefabs/Bomb" };
    public static int gridHeight= 20;
    public static int gridWidth = 10;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
	public int[] scores = { 100, 200, 400, 800 };
	private int rowCount = 0;
	private int userScore = 0;
	public Text display_score;

    private GameObject nextTetro;
    private GameObject previewTetro;

    private bool gameStart = false;

    private Vector3 previewPos = new Vector3(-6.5f, 15, -3);
    public Scene scene;
    private int range = 7;
    

	// Use this for initialization
	void Start () 
	{
        scene = SceneManager.GetActiveScene();
        if (scene.name == "Level2")
        {
            range = 8;
        }
            GenerateNext();
	}
		
	public void UpdateScore()
	{
		if (rowCount > 0) 
		{
			userScore += scores [(rowCount - 1)];
			display_score.text = userScore.ToString ();
			rowCount = 0;
		}
	}

	// Update is called once per frame
	void Update () 
	{     
        UpdateScore ();	
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
		rowCount++;
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

    public void Bomb(Tetromino tetro)
    {
        Transform mino = tetro.transform;
        Vector2 pos = Round(mino.position);

        if(grid[(int)pos.x, (int)pos.y] != null)
        {
            Destroy(grid[(int)pos.x, (int)pos.y].gameObject);
        }

        if ((int)pos.x < gridWidth - 1)
        {
            if (grid[(int)pos.x + 1, (int)pos.y] != null)
            {
                Destroy(grid[(int)pos.x + 1, (int)pos.y].gameObject);
            }
        }

        if ((int)pos.x < gridWidth - 1 && (int)pos.y < gridHeight)
        {
            if (grid[(int)pos.x + 1, (int)pos.y + 1] != null)
            {
                Destroy(grid[(int)pos.x + 1, (int)pos.y + 1].gameObject);
            }
        }

        if ((int)pos.x < gridWidth - 1 && (int)pos.y > 0)
        {
            if (grid[(int)pos.x + 1, (int)pos.y - 1] != null)
            {
                Destroy(grid[(int)pos.x + 1, (int)pos.y - 1].gameObject);
            }
        }

        if ((int)pos.x > 0)
        {
            if (grid[(int)pos.x - 1, (int)pos.y] != null)
            {
                Destroy(grid[(int)pos.x - 1, (int)pos.y].gameObject);
            }
        }

        if ((int)pos.y < gridHeight)
        {
            if (grid[(int)pos.x, (int)pos.y + 1] != null)
            {
                Destroy(grid[(int)pos.x, (int)pos.y + 1].gameObject);
            }
        }

        if ((int)pos.y > 0)
        {
            if (grid[(int)pos.x, (int)pos.y - 1] != null)
            {
                Destroy(grid[(int)pos.x, (int)pos.y - 1].gameObject);
            }
        }

        if ((int)pos.x > 0 && (int)pos.y < gridHeight)
        {
            if (grid[(int)pos.x - 1, (int)pos.y + 1] != null)
            {
                Destroy(grid[(int)pos.x - 1, (int)pos.y + 1].gameObject);
            }
        }

        if ((int)pos.x > 0 && (int)pos.y > 0)
        {
            if (grid[(int)pos.x - 1, (int)pos.y - 1] != null)
            {
                Destroy(grid[(int)pos.x - 1, (int)pos.y - 1].gameObject);
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
        if(!gameStart)
        {
            gameStart = true;
            nextTetro = (GameObject)Instantiate(Resources.Load(names[Random.Range(0, range)], typeof(GameObject)), new Vector3(5.0f, 22.0f, -3.0f), Quaternion.identity);
            previewTetro = (GameObject)Instantiate(Resources.Load(names[Random.Range(0, range)], typeof(GameObject)), previewPos, Quaternion.identity);
            previewTetro.GetComponent<Tetromino>().enabled = false;
        }
        else
        {
            previewTetro.transform.localPosition = new Vector3(5.0f, 22.0f, -3.0f);
            nextTetro = previewTetro;
            nextTetro.GetComponent<Tetromino>().enabled = true;
            previewTetro = (GameObject)Instantiate(Resources.Load(names[Random.Range(0, range)], typeof(GameObject)), previewPos, Quaternion.identity);
            previewTetro.GetComponent<Tetromino>().enabled = false;
        }
		
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
