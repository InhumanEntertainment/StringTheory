using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
    public float ScreenSize = 5;
    public float AspectRatio;
    public int Width, Height;

    //=====================================================================================================================================//
    void Start()
    {
        SetupCamera();
    }

    //=====================================================================================================================================//
    void Update()
    {
        AspectRatio = camera.aspect;
        Width = Screen.width;
        Height = Screen.height;

        SetupCamera();
	}

    //===================================================================================================================================================================//
    void SetupCamera()
    {
        if (camera.aspect < 1)
        {
            camera.orthographicSize = ScreenSize / camera.aspect;      
        }
        else
        {
            camera.orthographicSize = ScreenSize;
        }
    }

    //=====================================================================================================================================//
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(-ScreenSize, -ScreenSize, 0), new Vector3(ScreenSize, -ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(-ScreenSize, ScreenSize, 0), new Vector3(ScreenSize, ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(-ScreenSize, -ScreenSize, 0), new Vector3(-ScreenSize, ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(ScreenSize, -ScreenSize, 0), new Vector3(ScreenSize, ScreenSize, 0));
        //SetupCamera();
    }
}
