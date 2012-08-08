using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBar : MonoBehaviour
{
    public GameObject BarSegment;
    public UILabel DistanceLabel;
    public UILabel CurrentDistanceLabel;
    public UILabel LastDistanceLabel;
    
    GameObject[] BarSegments = new GameObject[0];
    public float BarSegmentScale = 0.1f;
    public float BarSegmentHeight = 16f;
    public float BarMax = 45;    
    public float ScreenWidth = 1536;
    public float GlobalScale = 100;

    //============================================================================================================================================//
    void Update()
    {    
        ColorString[] strings = (ColorString[])GameObject.FindObjectsOfType(typeof(ColorString));

        // Find Total Length //
        float TotalLength = 0;
        for (int i = 0; i < strings.Length; i++)
        {
            TotalLength += strings[i].CurveLength / GlobalScale;
        }

        // Calculate Bar Ratio //
        float BarLength = BarMax;
        if (TotalLength > BarMax)
        {
            BarLength = TotalLength;
        }

        // Reset Values //
        for (int i = 0; i < BarSegments.Length; i++)
        {
            BarSegments[i].transform.localScale = Vector3.zero; 
        }

        // Update Segments //
        float Length = 0;
        for (int i = strings.Length - 1; i >= 0; i--)
        {            
            float x = Length / BarLength * ScreenWidth;
            float width = (strings[i].CurveLength / GlobalScale) / BarLength * ScreenWidth;

            BarSegments[i].transform.localPosition = new Vector3(x + width * 0.5f, BarSegmentHeight * 0.5f, 0);
            BarSegments[i].transform.localScale = new Vector3(width, BarSegmentHeight, BarSegmentHeight) * BarSegmentScale;

            BarSegments[i].renderer.material.color = strings[i].Color.ProgressColor;

            Length += strings[i].CurveLength / GlobalScale;
        }

        // Update Labels //        
        float labelX = Length / BarLength * ScreenWidth;
        labelX = Mathf.Clamp(labelX, 90f, ScreenWidth - 90f);

        CurrentDistanceLabel.transform.localPosition = new Vector3(labelX, -8, 0);
        CurrentDistanceLabel.text = TotalLength.ToString("N1") +"m";

        UILabel label = CurrentDistanceLabel.GetComponent<UILabel>();
        if (strings.Length > 0)
            label.color = strings[0].Color.ProgressColor;
        else
            label.color = Color.grey;
    
    }

    //============================================================================================================================================//
    public void ResetColorBar()
    {
        // Setup Segment Meshes //
        ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));
        int numColors = bases.Length / 2;

        for (int i = 0; i < BarSegments.Length; i++)
        {
            Destroy(BarSegments[i].gameObject);
        }     

        BarSegments = new GameObject[numColors];

        for (int i = 0; i < numColors; i++)
        {
            BarSegments[i] = (GameObject)Instantiate(BarSegment);
            BarSegments[i].name = "BarSegment_" + i;
            BarSegments[i].transform.parent = this.transform;
            BarSegments[i].transform.localScale = Vector3.zero;
        }
    }
}
