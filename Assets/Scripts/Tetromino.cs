using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tetromino : MonoBehaviour {

    float fall = 0.5f;
    public float fallSpeed = 1;

	public bool enableRotation = true;
	public bool disableRotation = false;
    public Scene scene; // used to check which scene it's on so we can adjust speed
	public AudioClip moveEffect; // sound for when the mino moves
	public AudioClip landEffect; // pop sound when the mino lands in to place

	private AudioSource audioSource;

	// Use this for initialization
	void Start () 
	{
        scene = SceneManager.GetActiveScene();
		audioSource = GetComponent<AudioSource> ();
	    if(scene.name == "Level2")
        {
            fall = 0.3f;
        }
	}

	// Update is called once per frame
	void Update () {
        CheckInput();
	}

    void CheckInput()
    {
		// right directional movement with grid detection
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			transform.position += new Vector3 (1, 0, 0);
			if (!CheckIsValidPosition ()) {
				transform.position += new Vector3 (-1, 0, 0);
			}
            else
            {
				MovementSound ();
                FindObjectOfType<Game>().UpdateGrid(this);
            }
		} 

		// left directional movement with grid detection, if the step would put it out of the grid reverse the rotation
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			transform.position += new Vector3 (-1, 0, 0);
			if (!CheckIsValidPosition ()) {
				transform.position += new Vector3 (1, 0, 0);
			}
            else
            {
				MovementSound ();
                FindObjectOfType<Game>().UpdateGrid(this);
            }
        } 

		// rotation algortithim making sure that rotation on any point of the minos will ensure the piece will be within the grid
		else if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			if (enableRotation) 
			{
				if (disableRotation) 
				{
					if (transform.rotation.eulerAngles.z >= 90) 
					{
						transform.Rotate (0, 0, -90);
					} 
					else 
					{
						transform.Rotate (0, 0, 90);
					}
				} 
				else 
				{
					transform.Rotate (0, 0, 90);
				}

				if (!CheckIsValidPosition ()) 
				{
					if (disableRotation) 
					{
						if (transform.rotation.eulerAngles.z >= 90)
						{
							transform.Rotate (0, 0, -90);
						} 
						else 
						{
							transform.Rotate (0, 0, 90);
						}
					} 
					else 
					{
						transform.Rotate (0, 0, -90);
					}
				}
                else
                {
					MovementSound ();
                    FindObjectOfType<Game>().UpdateGrid(this);
                }
            }
		}

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time >= fallSpeed)
        {
            transform.position += new Vector3(0, -1, 0);
            fallSpeed = Time.time + fall;

			if (!CheckIsValidPosition())
			{
                transform.position += new Vector3(0, 1, 0);
                if (this.tag == "bomb")
                {
                    FindObjectOfType<Game>().Bomb(this);
                }
				LandingSound ();
                enabled = false;
                FindObjectOfType<Game>().GenerateNext();
                FindObjectOfType<Game>().DeleteRow();
                if(FindObjectOfType<Game>().CheckAboveGrid(this))
                {
                    FindObjectOfType<Game>().GameOver();
                }
			}
            else
            {
				MovementSound ();
                FindObjectOfType<Game>().UpdateGrid(this);
            }
        }

		else if (Input.GetKeyDown(KeyCode.Space))
		{
            while (CheckIsValidPosition())
            {
                transform.position += new Vector3(0, -1, 0);                                   
            }

			if (!CheckIsValidPosition ()) 
			{
				transform.position += new Vector3 (0, 1, 0);
				FindObjectOfType<Game> ().UpdateGrid (this);
				if (this.tag == "bomb") 
				{
					FindObjectOfType<Game> ().Bomb (this);
				}

				LandingSound ();
				enabled = false;
				FindObjectOfType<Game> ().GenerateNext ();
				FindObjectOfType<Game> ().DeleteRow ();
				if (FindObjectOfType<Game> ().CheckAboveGrid (this)) 
				{
					FindObjectOfType<Game> ().GameOver ();
				}
			} 

			else 
			{
				FindObjectOfType<Game> ().UpdateGrid (this);
			}
        }
    }

	/// <summary>
	/// plays a small audio sound whenever the block is rotated our moved
	/// </summary>
	void MovementSound()
	{
		audioSource.PlayOneShot (moveEffect);
	}
	void LandingSound()
	{
		audioSource.PlayOneShot (landEffect);
	}

    bool CheckIsValidPosition()
    {
        foreach(Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }
            if(FindObjectOfType<Game>().GetTransformAtGrid(pos) != null && FindObjectOfType<Game>().GetTransformAtGrid(pos).parent != transform)
            {
                return false;
            }
            
        }
        return true;
    }
}


