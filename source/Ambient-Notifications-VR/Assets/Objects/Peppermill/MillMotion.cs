using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillMotion : MonoBehaviour {

    public ParticleSystem particleSystem;

    Animator animator;

    private bool isTimer = false;
    private float timer = 0.0f;

    private bool open = false;
    private bool milling = false;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        particleSystem.Stop();
    }
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("IsOpening", open);
        animator.SetBool("IsMilling", milling);

        if (isTimer)
        {
            timer += Time.deltaTime;
            // Timer for 10 secs
            if ((timer % 60) > 9)
            {
                particleSystem.Stop();
                isTimer = false;
            }
        }
    }

    public void toggleOpen() {
        open = !open;

        Debug.Log("MillMotion: " + open);
        if (open == true)// && particleSystem.isStopped)
        {
            particleSystem.Play();
            timer = 0.0f;
            isTimer = true;
        }
    }

    internal void toggleMilling()
    {
        milling = !milling;
    }
}