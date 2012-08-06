using UnityEngine;
using System.Collections;

public class Dimensions : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    public bool FlipX;
    public bool FlipY;
    public bool SwapXY;

    public Vector3 Offset;
    public float Rotation;

    //============================================================================================================================================//
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 Player1Pos = mousePos;
        Vector3 Player2Pos = mousePos;

        if (SwapXY)
        {
            float temp = Player2Pos.x;
            Player2Pos.x = Player2Pos.y;
            Player2Pos.y = -temp;
        }
        if (FlipX)
        {
            Player2Pos.x = -Player2Pos.x;
        }
        if (FlipY)
        {
            Player2Pos.y = -Player2Pos.y;
        }

        float radian = Rotation * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * Player2Pos.x + Mathf.Sin(radian) * Player2Pos.y;
        float y = -Mathf.Sin(radian) * Player2Pos.x + Mathf.Cos(radian) * Player2Pos.y;

        Player2Pos = new Vector3(x, y, 0);
        Player2Pos += Offset;

        Player1.transform.position = Player1Pos;
        Player2.transform.position = Player2Pos;
	}
}
