using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
    public float ScreenSize = 5;
    public float AspectRatio;
    public int Width, Height;
    public int TargetWidth = 1536;
    public int TargetHeight = 2048;

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
        float aspect = (float)TargetWidth / TargetHeight;

        if (camera.aspect < aspect)
        {
            camera.orthographicSize = ScreenSize / camera.aspect;      
        }
        else
        {
            camera.orthographicSize = ScreenSize / aspect;
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
