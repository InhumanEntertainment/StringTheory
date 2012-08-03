using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
    static public GameCamera Instance;

    public float ScreenSize = 5;
    public float AspectRatio;
    public int Width, Height;
    public int TargetWidth = 1536;
    public int TargetHeight = 2048;

    public int GridX = 10;
    public int GridY = 10;
    public bool HexGrid = false;
    public float BaseScale = 1;

    //=====================================================================================================================================//
    void Awake()
    {
        // Singleton //
        if (GameCamera.Instance == null)
        {
            GameCamera.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

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

        /*if (!HexGrid)
        {
            // Draw Grid //
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
        }
        else
        {
            // Draw Hex //
            if (GridY > 0)
            {
                float stepY = 1f / GridY * ScreenSize * 2;

                for (float y = -ScreenSize; y <= ScreenSize; y += stepY)
                {
                    Gizmos.DrawLine(new Vector3(-ScreenSize, y, 0), new Vector3(ScreenSize, y, 0));
                }
            }

            if (GridY > 0 && GridX > 0)
            {
                float stepX = 1f / GridX * ScreenSize * 2;
                float stepY = 1f / GridY * ScreenSize * 2;
                bool offset = false;

                for (float y = ScreenSize; y > -ScreenSize; y -= stepY)
                {
                    offset = !offset;
                    for (float x = -ScreenSize; x <= ScreenSize; x += stepX)
                    {
                        float offsetX = offset ? 0 : stepX * 0.5f;
                        //print(offsetX);

                        Gizmos.DrawLine(new Vector3(x + offsetX, y, 0), new Vector3(x + offsetX, y - stepY, 0));
                    }
                }
            }
        }


        

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(-ScreenSize, -ScreenSize, 0), new Vector3(ScreenSize, -ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(-ScreenSize, ScreenSize, 0), new Vector3(ScreenSize, ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(-ScreenSize, -ScreenSize, 0), new Vector3(-ScreenSize, ScreenSize, 0));
        Gizmos.DrawLine(new Vector3(ScreenSize, -ScreenSize, 0), new Vector3(ScreenSize, ScreenSize, 0));    
         * */
    }

#if UNITY_EDITOR
    //=====================================================================================================================================//
    [UnityEditor.MenuItem("CONTEXT/GameCamera/Snap Bases")]
    static void SnapBases(UnityEditor.MenuCommand command)
    {
        GameCamera context = (GameCamera)command.context;
        Object[] objects = GameObject.FindObjectsOfType(typeof(ColorBase));

        for (int i = 0; i < objects.Length; i++)
        {
            Transform xform = ((ColorBase)objects[i]).transform;

            float stepX = 1f / (context.GridX) * context.ScreenSize * 2;
            float stepY = 1f / (context.GridY) * context.ScreenSize * 2;
            float offsetX = ((context.GridX % 2) == 1) ? 0 : 0.5f * stepX;
            float offsetY = ((context.GridY % 2) == 1) ? 0 : 0.5f * stepY;

            float x = Mathf.Round((xform.position.x - offsetX) / stepX);
            float y = Mathf.Round((xform.position.y - offsetY) / stepY);

            Vector3 newPosition = new Vector3(x * stepX + offsetX, y * stepY + offsetY, xform.position.z);
            xform.position = newPosition;
        }
    }

    //=====================================================================================================================================//
    [UnityEditor.MenuItem("CONTEXT/GameCamera/Resize Bases")]
    static void ResizeBases(UnityEditor.MenuCommand command)
    {
        GameCamera camera = (GameCamera)command.context;
        Object[] objects = GameObject.FindObjectsOfType(typeof(ColorBase));

        for (int i = 0; i < objects.Length; i++)
        {
            Transform xform = ((ColorBase)objects[i]).transform;

            float stepX = 1f / (camera.GridX) * camera.ScreenSize * 2;
            float stepY = 1f / (camera.GridY) * camera.ScreenSize * 2;

            float scale = Mathf.Min(stepX, stepY) * camera.BaseScale;
            xform.localScale = new Vector3(scale, scale, scale);
        }
    }
    #endif
}
