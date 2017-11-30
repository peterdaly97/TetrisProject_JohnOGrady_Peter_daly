using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCode : MonoBehaviour {
	
	public ParticleSystem explosion_effect;

	// Use this for initialization
	void Start () 
	{
	}

	void OnDestroy()
	{
		ParticleSystem explosion = Instantiate (explosion_effect) as ParticleSystem;
		explosion.transform.position = transform.position;
		explosion.Play();
		Destroy(explosion.gameObject, explosion.main.duration);
		Destroy(gameObject);
	}

	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
