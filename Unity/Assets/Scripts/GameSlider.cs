using UnityEngine;
using System.Collections;

public class GameSlider : MonoBehaviour
{
    public Vector3 Target = Vector3.zero;
    public Vector3 TouchPosition, StartPosition, TargetPosition, DownPosition;
    public Camera Camera;
    public float Multiplier = 1;

    public bool PauseMenu = false;

    //============================================================================================================================================//
    void Start()
    {
        StartPosition = transform.localPosition;
        Multiplier = 2048f / Screen.height;
    }

    //============================================================================================================================================//
    bool Touched = false;
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        //mouse.z = 0;


        if (Input.GetMouseButtonDown(0))
	    {
            Touched = false;
            TouchPosition = mouse;
            DownPosition = transform.localPosition;

            Ray ray = Camera.ScreenPointToRay(new Vector3(TouchPosition.x, TouchPosition.y, 0));
            RaycastHit hit;

            if (gameObject.collider.Raycast(ray, out hit, 50.0f))
            {
                Touched = true;
            }
            else
                Touched = false;		    
	    }
        else if (Input.GetMouseButton(0))
        {
            if (Touched)
            {
                float distance = TouchPosition.y - mouse.y;
                Vector3 temp = new Vector3(0, distance, 0);
                transform.localPosition = DownPosition - temp * Multiplier;

                if (transform.localPosition.y > TargetPosition.y)
                   transform.localPosition = new Vector3(transform.localPosition.x, TargetPosition.y, transform.localPosition.z);
                else if (transform.localPosition.y < StartPosition.y)
                   transform.localPosition = new Vector3(transform.localPosition.x, StartPosition.y, transform.localPosition.z);
            }
        }

        float targetDistance = Mathf.Abs(transform.localPosition.y - TargetPosition.y);
        float startDistance = Mathf.Abs(transform.localPosition.y - StartPosition.y); 
        
        if (Input.GetMouseButtonUp(0))
        {
            if (Touched)
            {
                Touched = false;

                float tapDistance = Mathf.Abs(TouchPosition.y - mouse.y);
                if (tapDistance < 20)
                {
                    // Toggle Target //
                    if (Target == TargetPosition)
                        Target = StartPosition;
                    else
                        Target = TargetPosition;
                }
                else
                {
                    if (targetDistance < startDistance)
                    {
                        Target = TargetPosition;
                        //transform.localPosition = TargetPosition;
                    }
                    else
                    {
                        Target = StartPosition;
                        //transform.localPosition = StartPosition;
                    }
                }
            }
        }

        if (!Touched)
        {
            // Fade Between Targets //
            transform.localPosition = transform.localPosition * 0.8f + 0.2f * Target;
        }

        if (PauseMenu)
        {
            //Game.Log(startDistance);
            if (startDistance < 10)
            {
                if (Game.Instance.Paused)
                {
                    Game.Instance.Resume();
                    Game.Log("Resume");
                }
            }
            else
            {
                if (!Game.Instance.Paused)
                {
                    Game.Instance.Pause();
                    Game.Log("Pause");
                }
            }
        }
	}
}
