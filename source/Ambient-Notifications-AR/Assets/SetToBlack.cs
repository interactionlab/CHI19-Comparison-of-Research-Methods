using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental;


public class SetToBlack : MonoBehaviour {

    private Material m_mat;

	// Use this for initialization
	void Start () {
        m_mat = GetComponent<Renderer>().material;
        m_mat.color = Color.black;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
