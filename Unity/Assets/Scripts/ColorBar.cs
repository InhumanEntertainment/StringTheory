using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBar : MonoBehaviour
{
    public static ColorBar Instance;
    public GameObject BarSegment;
    public UILabel DistanceLabel;
    public UILabel CurrentDistanceLabel;
    public UILabel LastDistanceLabel;

    UIFilledSprite[] BarSegments = new UIFilledSprite[0];
    public float BarSegmentScale = 1f;
    public float BarSegmentHeight = 16f;
    public float BarMax = 45;    
    public float ScreenWidth = 1536;
    public float GlobalScale = 100;

    public float TotalLength;

    ColorString[] Strings;

    //============================================================================================================================================//
    void Awake()
    {
        Instance = this;
        ResetColorBar();
    }

    //============================================================================================================================================//
    void Update()
    {        
        // Find Total Length //
        TotalLength = 0;
        for (int i = 0; i < Strings.Length; i++)
        {
            TotalLength += Strings[i].CurveLength / GlobalScale;
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
        for (int i = Strings.Length - 1; i >= 0; i--)
        {            
            float x = Length / BarLength * ScreenWidth;
            float width = (Strings[i].CurveLength / GlobalScale) / BarLength * ScreenWidth;

            BarSegments[i].transform.localPosition = new Vector3(x, 0, 0);
            BarSegments[i].transform.localScale = new Vector3(width, BarSegmentHeight, BarSegmentHeight) * BarSegmentScale;

            BarSegments[i].color = Strings[i].Color.ProgressColor;
            //BarSegments[i].renderer.material.color = strings[i].Color.ProgressColor;

            Length += Strings[i].CurveLength / GlobalScale;
        }

        // Update Labels //        
        float labelX = Length / BarLength * ScreenWidth;
        labelX = Mathf.Clamp(labelX, 90f, ScreenWidth - 90f);

        CurrentDistanceLabel.transform.localPosition = new Vector3(labelX, -8, 0);
        CurrentDistanceLabel.text = TotalLength.ToString("N1") +"m";

        if (Strings.Length > 0)
            CurrentDistanceLabel.color = Strings[0].Color.ProgressColor;
        else
            CurrentDistanceLabel.color = Color.grey;   
    }

    //============================================================================================================================================//
    public void ResetColorBar()
    {
        Strings = (ColorString[])GameObject.FindObjectsOfType(typeof(ColorString));

        // Setup Segment Meshes //
        ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));
        int numColors = bases.Length / 2;

        for (int i = 0; i < BarSegments.Length; i++)
        {
            Destroy(BarSegments[i].gameObject);
        }

        BarSegments = new UIFilledSprite[numColors];

        for (int i = 0; i < numColors; i++)
        {
            GameObject obj = (GameObject)Instantiate(BarSegment);
            BarSegments[i] = obj.GetComponent<UIFilledSprite>();
            BarSegments[i].name = "BarSegment_" + i;
            BarSegments[i].transform.parent = this.transform;
            BarSegments[i].transform.localScale = Vector3.zero;
        }
    }
}
