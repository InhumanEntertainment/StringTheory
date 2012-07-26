using UnityEngine;
using System.Collections;

public class NGUIHelper : MonoBehaviour 
{
    public int Width = 1536;
    public int Height = 2048;
    public bool FitToScreen = false;
    public enum FitStyle {Horizontal, Vertical, Both};
    public FitStyle FitMode = FitStyle.Vertical;

    //============================================================================================================================================//
    void Update()
    {
        UIRoot root = GetComponent<UIRoot>();

        if (FitMode != FitStyle.Vertical)
        {
            float targetAspect = (float)Width / Height;
            float aspect = (float)Screen.width / Screen.height;

            if (aspect < targetAspect || FitMode == FitStyle.Horizontal)
            {
                root.manualHeight = (int)(Width / aspect);
            }
            else if (FitMode == FitStyle.Both)
            {
                root.manualHeight = Height;
            }
        }
        else
            root.manualHeight = Height;
	}
}
