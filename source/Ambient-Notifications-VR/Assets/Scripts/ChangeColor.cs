using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {
    
    private string[] colorHex = new string[] {"#00FF00", "#11FF00", "#22FF00", "#33FF00", "#44FF00", "#55FF00", "#66FF00", "#77FF00", "#88FF00", "#99FF00", "#AAFF00", "#BBFF00", "#CCFF00", "#DDFF00", "#EEFF00", "#FFFF00", "#FFEE00", "#FFDD00", "#FFCC00", "#FFBB00", "#FFAA00", "#FF9900", "#FF8800", "#FF7700", "#FF6600", "#FF5500", "#FF4400", "#FF3300", "#FF2200", "#FF1100", "#FF0000"};
    private Color[] colors;
    public Light[] pointLights; 

	// Use this for initialization
	void Start () {
       
        colors = new Color[colorHex.Length];

        for(int i = 0; i < colorHex.Length; i++)
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
        
        foreach(Light item in pointLights)
        {
            item.color = cTemp;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
