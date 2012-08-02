using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
    public float ScreenSize = 5;
    public float AspectRatio;
    public int Width, Height;
    public int TargetWidth = 1536;
    public int TargetHeight = 2048;

    public int GridX = 10;
    public int GridY = 10;

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
        Gizmos.color = Color.grey;
        if (GridX > 0)
        {
            float stepX = 1f / GridX * ScreenSize * 2;

            for (float x = -ScreenSize; x <= ScreenSize; x += stepX)
            {
                Gizmos.DrawLine(new Vector3(x, -ScreenSize, 0), new Vector3(x, ScreenSize, 0));
            }
        }

        if (GridY > 0)
        {
            float stepY = 1f / GridY * ScreenSize * 2;

            for (float y = -ScreenSize; y <= ScreenSize; y += stepY)
            {
                Gizmos.DrawLine(new Vector3(-ScreenSize, y, 0), new Vector3(ScreenSize, y, 0));
            }
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(-ScreenSize, -ScreenSize, 0), new Vector3(ScreenSize, -ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(-ScreenSize, ScreenSize, 0), new Vector3(ScreenSize, ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(-ScreenSize, -ScreenSize, 0), new Vector3(-ScreenSize, ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(ScreenSize, -ScreenSize, 0), new Vector3(ScreenSize, ScreenSize, 0));       
    }
}
