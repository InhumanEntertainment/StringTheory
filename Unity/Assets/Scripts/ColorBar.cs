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

    public UILabel BestDistanceLabel;
    public UIFilledSprite BestMarker;

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
        // Reset Values //
        for (int i = 0; i < BarSegments.Length; i++)
        {
            BarSegments[i].transform.localScale = Vector3.zero;
        }

        if (true)
        {
            Strings = (ColorString[])GameObject.FindObjectsOfType(typeof(ColorString));

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

            // Set Bar Length Based on Best //
            float bestLength = Game.Instance.CurrentLevel.BestLength;
            if (bestLength > 0 && bestLength * 1.1f > BarLength)
            {
                BarLength = bestLength * 1.1f;
            }

            if (Strings.Length > 0 && !Game.Instance.LevelIsTransitioning)
            {
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
                CurrentDistanceLabel.text = TotalLength.ToString("N1") + "m";
                CurrentDistanceLabel.color = Strings[0].Color.ProgressColor;
            }
            else
            {
                CurrentDistanceLabel.transform.localPosition = new Vector3(-1000, 0, 0);         
            }
            
            // Update Best Marker //
            if (bestLength > 0 && !Game.Instance.LevelIsTransitioning)
            {
                BestDistanceLabel.text = bestLength.ToString("N1") + "m";
                float bestX = bestLength / BarLength * ScreenWidth - 8;
                BestMarker.transform.localPosition = new Vector3(bestX, 0, 0);
                bestX = Mathf.Clamp(bestX, 90f, ScreenWidth - 90f);
                BestDistanceLabel.transform.localPosition = new Vector3(bestX, -8, 0);
            }
            else
            {
                BestMarker.transform.localPosition = new Vector3(-1000, 0, 0);
                BestDistanceLabel.transform.localPosition = new Vector3(-1000, 0, 0);
            }
        }                      
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
