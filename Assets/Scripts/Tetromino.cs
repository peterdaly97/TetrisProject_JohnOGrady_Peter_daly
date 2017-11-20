﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    float fall = 0;
    public float fallSpeed = 1;

	public bool enableRotation = true;
	public bool disableRotation = false;

	// Use this for initialization
	void Start () 
	{
	    
	}

	// Update is called once per frame
	void Update () {
        CheckInput();
	}

    void CheckInput()
    {

		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			transform.position += new Vector3 (1, 0, 0);
			if (!CheckIsValidPosition ()) {
				transform.position += new Vector3 (-1, 0, 0);
			}
		} 

		else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			transform.position += new Vector3 (-1, 0, 0);
			if (!CheckIsValidPosition ()) {
				transform.position += new Vector3 (1, 0, 0);
			}
		} 

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
			}
		}

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            transform.position += new Vector3(0, -1, 0);
            fall = Time.time;

			if (!CheckIsValidPosition())
			{
				transform.position += new Vector3(0, 1, 0);
			}
        }
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
            
        }
        return true;
    }
}