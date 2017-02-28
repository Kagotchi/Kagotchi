using UnityEngine;
using System.Collections;
using System;

public class csGuiBar : MonoBehaviour {

    public float barValue; //current progress
    public Vector2 pos = new Vector2(5, 10);
    public Vector2 size = new Vector2(150, 20);
    public String emptyTex = "Food";
    public String fullTex;
    public Color barColor = Color.yellow;

    void OnGUI()
    {
        //draw the background:
        GUI.contentColor = barColor;
        GUI.backgroundColor = barColor;
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
            
            GUI.Box(new Rect(0, 0, size.x, size.y), emptyTex);
            //draw the filled-in part:
            GUI.BeginGroup(new Rect(0, 0, size.x * barValue, size.y));
                GUI.Box(new Rect(0, 0, size.x, size.y), fullTex);
            GUI.EndGroup();
        GUI.EndGroup();
    }

    public void SetValue(float value)
    {
        barValue = value;
    }

    void Update()
    {
        //for this example, the bar display is linked to the current time,
        //however you would set this value based on your desired display
        //eg, the loading progress, the player's health, or whatever.
        //        barDisplay = MyControlScript.staticHealth;
    }
}
