using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
