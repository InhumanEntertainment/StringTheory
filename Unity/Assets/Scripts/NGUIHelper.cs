using UnityEngine;
using System.Collections;

public class NGUIHelper : MonoBehaviour 
{
    public int Width = 1536;
    public int Height = 2048;
    public bool ResizeToWidth = false;

    //============================================================================================================================================//
    void Update()
    {
        UIRoot root = GetComponent<UIRoot>();

        if (ResizeToWidth)
            root.manualHeight = (int)(Width / ((float)Screen.width / Screen.height));
        else
            root.manualHeight = Height;
	}
}
