using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental;


public class ChangeColor : MonoBehaviour
{

    private string[] colorHex = new string[] { "#00FF00", "#11FF00", "#22FF00", "#33FF00", "#44FF00", "#55FF00", "#66FF00", "#77FF00", "#88FF00", "#99FF00", "#AAFF00", "#BBFF00", "#CCFF00", "#DDFF00", "#EEFF00", "#FFFF00", "#FFEE00", "#FFDD00", "#FFCC00", "#FFBB00", "#FFAA00", "#FF9900", "#FF8800", "#FF7700", "#FF6600", "#FF5500", "#FF4400", "#FF3300", "#FF2200", "#FF1100", "#FF0000" };
    private Color[] colors;

    //public FoggyLight[] foggyLights;
          // Use this for initialization
    void Start()
    {

        colors = new Color[colorHex.Length];

        for (int i = colorHex.Length -1 ; i >= 0 ; i--)
        {
            Color clr = new Color();
            ColorUtility.TryParseHtmlString(colorHex[i], out clr);
            colors[i] = clr;
        }
    }

    public void setColor(int _percent)
    {
        Color cTemp;
        if (_percent == -1)
        {
            cTemp = Color.black;
            
        }
        else
        {
            if (_percent > 99) _percent = 99;
            if (_percent < 0) _percent = 0;
            int i = (int)(_percent / 100.0f * colorHex.Length);
            cTemp = colors[i];
        }

        GameObject parent = this.gameObject;
        
        //Change light conditions
        for (int j = 0; j < parent.transform.childCount; j++)
        {
            Transform child = this.gameObject.transform.GetChild(j);
            
            if(child!= null)
            {
                Debug.Log("Child: " + child.name);
                for (int i = 0; i < child.childCount; i++)
                {
                    GameObject grandChild = child.GetChild(i).gameObject;
                    //Debug.Log(grandChild.name);
                    //if (grandChild.name.Equals("Sphere"))
                    //{
                    //    grandChild.GetComponent<Renderer>().material.color = cTemp;
                    //    Debug.Log("Changed color for: " + grandChild.name);
                    //}
                    //else 
                    if (grandChild.name.Equals("Point Light"))
                    {
                        grandChild.GetComponent<Light>().color = cTemp;
                        Debug.Log("Changed color for: Point Light" + i);
                    }
                }  
            }
        }
        /*
        GameObject parent = this.gameObject;
        if (parent != null)
        {
            
            for (int j=0; j< parent.transform.childCount; j++)
            {
                Transform child = this.gameObject.transform.GetChild(j);
                if (child != null)
                {
                    Debug.Log("Child: " + child.name);
                    if (child.name.Equals("FoggyLights"))
                    {
                        for (int i = 0; i < child.childCount; i++)
                        {
                            GameObject grandChild = child.GetChild(i).gameObject;
                            Debug.Log(grandChild.name);
                            if (grandChild.name.Equals("FoggyLight"))
                            {
                                grandChild.GetComponent<Renderer>().sharedMaterial.SetColor("PointLightColor",cTemp);
                                Debug.Log("Changed color for: FoggyLight" + i);
                                grandChild.GetComponent<Light>().color = cTemp;
                                Debug.Log("Changed color for attached light at: FoggyLight" + i);
                            }
                        }
                    }
                    
                }
            }
        }*/
   
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
