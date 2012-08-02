using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour 
{
    public bool EditMode = false;
    public bool Snapping = false;
    public int GridX = 10;
    public int GridY = 10;
    public bool HexGrid = false;
    public float BaseScale = 1;
    public float ScreenSize = 5;

    //=====================================================================================================================================//
    void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;

        if (!HexGrid)
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
    }
}
