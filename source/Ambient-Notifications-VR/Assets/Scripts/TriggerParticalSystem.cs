using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticalSystem : MonoBehaviour {

	public ParticleSystem ParticleSystem1;
	public ParticleSystem ParticleSystem2;

	private bool isTimer = false;
	private float timer = 0.0f;

    // Use this for initialization
    void Start () {
		ParticleSystem1.Stop();
		ParticleSystem2.Stop();
	}
	
	// Update is called once per frame
	void Update () {

		if (isTimer)
		{
			timer += Time.deltaTime;
            // Timer for 10 secs
            if ((timer % 60) > 10)
            { 
                ParticleSystem1.Stop();
                ParticleSystem2.Stop();
                isTimer = false;
            }
    	}
		
	}

	public void play(){
		if (!isTimer){
			if(!ParticleSystem1.isPlaying){
				ParticleSystem1.Play();
				ParticleSystem2.Play();
				timer = 0.0f;
				isTimer = true;
			}
		}
	}
}
