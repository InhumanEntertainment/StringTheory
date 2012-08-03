using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackSlider : MonoBehaviour 
{
    public Transform Slider;
    public Vector3 OffsetPosition = Vector3.zero;

    public int TargetIndex = 0;
    public float[] Targets;

    public float Position = 0;
    public float Velocity = 0;
    public float Drag = 0.95f;
    public float Speed = 0.1f;

    public float DownPosition = 0;
    public float PrevMousePosition;
    public float MouseVelocity = 0;
    public float SwipeAmount = 2f;
    public float MinimumSwipeDistance = 1f;
    public float StartPosition = 0;
    public Transform LabSheet;
    static GameObject sliderobject;

    //============================================================================================================================================//
    void Awake()
    {
        OffsetPosition = new Vector3(transform.position.x, 0, 0);

        TargetIndex = PlayerPrefs.GetInt("DNA_Area");
        print("Area = " + TargetIndex);
        Position = Targets[TargetIndex];

        // Slider //
        sliderobject = this.gameObject;
        SetSlider(false);
	}

    //============================================================================================================================================//
    public static void SetSlider(bool value)
    {
        //AreasSlider slider = sliderobject.GetComponent<AreasSlider>();
        //slider.enabled = value;
        print(sliderobject);
        NGUITools.SetActive(sliderobject, value);

        //sliderobject.transform.position = new Vector3(sliderobject.transform.position.x, value ? 64 : 1024, 0);
    }

    //============================================================================================================================================//
    float SetTarget()
    {
        for (int i = 0; i < Targets.Length; i++)
        {
            if (Mathf.Abs(Position - Targets[i]) < Mathf.Abs(Position - Targets[TargetIndex]))
            {
                TargetIndex = i;
                PlayerPrefs.SetInt("DNA_Area", TargetIndex);
            }
        }

        return Targets[TargetIndex];
    }

    //============================================================================================================================================//
    void Lab()
    {
        if (!Dragging)
        {
            if (TargetIndex != 1)
            {
                TargetIndex = 1;
                PlayerPrefs.SetInt("DNA_Area", TargetIndex);
            }
            else
            {
                // Swap Level Sheets //
                LabSheet.position = new Vector3(1536 / 1024f, 0, 0);

                Animation anim = GameObject.Find("CameraAnimation").GetComponent<Animation>();
                anim.Play("Frontend_Levels_Open");             
            }
        }        
    }

    void Caves()
    {
        if (!Dragging)
        {
            if (TargetIndex != 2)
            {
                TargetIndex = 2;
                PlayerPrefs.SetInt("DNA_Area", TargetIndex);
            }
            else
            {
                // Swap Level Sheets //
                LabSheet.position = new Vector3(1536 / 1024f, 2, 0);

                Animation anim = GameObject.Find("CameraAnimation").GetComponent<Animation>();
                anim.Play("Frontend_Levels_Open");              
            }
        }
    }

    void Organic()
    {
        if (!Dragging)
        {
            if (TargetIndex != 3)
            {
                TargetIndex = 3;
                PlayerPrefs.SetInt("DNA_Area", TargetIndex);
            }
            else
            {
                // Swap Level Sheets //
                LabSheet.position = new Vector3(1536 / 1024f, 4, 0);

                Animation anim = GameObject.Find("CameraAnimation").GetComponent<Animation>();
                anim.Play("Frontend_Levels_Open");              
            }
        }
    }

    //============================================================================================================================================//
    List<GameObject> Buttons = new List<GameObject>();
    bool Dragging = false;
    bool MouseDown = false;

    void Update()
    {
	    // If camera is in Area Mode //

        float MousePosition = GetMousePosition().x;
            
        if (Input.GetMouseButtonDown(0))
        {
            DownPosition = MousePosition;
            MouseVelocity = 0;
            StartPosition = Position;
            Dragging = false;
            MouseDown = true;

            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            float distance = MousePosition - DownPosition;
            // Swipe Gestures //
            if (MouseVelocity < -SwipeAmount && distance < -MinimumSwipeDistance)
            {
                TargetIndex = TargetIndex + 1 < Targets.Length ? TargetIndex + 1 : TargetIndex;
                PlayerPrefs.SetInt("DNA_Area", TargetIndex);
            }
            else if (MouseVelocity > SwipeAmount && distance > MinimumSwipeDistance)
            {
                TargetIndex = TargetIndex - 1 >= 0 ? TargetIndex - 1 : 0;
                PlayerPrefs.SetInt("DNA_Area", TargetIndex);
            }
            else if (Mathf.Abs(distance) > 0.1f)
            {
                // Set to Closest Target //
                SetTarget();
            }

            // Enable buttons on drag //            
            for (int i = 0; i < Buttons.Count; i++)
            {
                //Buttons[i].active = true;
                //Buttons[i].GetComponent<UIButton>().
            }

            MouseDown = false;
        }
        if (Input.GetMouseButton(0))
        {
            float vel = MousePosition - PrevMousePosition;
            MouseVelocity = MouseVelocity * 0.7f + vel * 0.3f;

            float Offset = MousePosition - DownPosition;
            Position = StartPosition + Offset / 1.5f;

            transform.position = OffsetPosition + Slider.position / 512 + new Vector3(Position, 64 / 512f, 0);

            // Disables buttons on drag //
            if (Mathf.Abs(Offset) > 0.1f)
            {
                Dragging = true;
                UIButton[] buttons = GetComponentsInChildren<UIButton>();

                for (int i = 0; i < buttons.Length; i++)
                {
                    Buttons.Add(buttons[i].gameObject);
                    //buttons[i].gameObject.active = false;
                }
            }
        }
        else
        {           
            //float Acceleration = Targets[TargetIndex] - Position;
            //Acceleration *= (Speed * Time.deltaTime);

            //Velocity += Acceleration;
            //Velocity *= Mathf.Max(0, 1 - (Drag * Time.deltaTime));
            /*int sign = Acceleration > 0 ? 1 : -1;
            float nextVelocity = Velocity + (Velocity > 0 ? (-Drag * Time.deltaTime) : (Drag * Time.deltaTime));
            int nextsign = nextVelocity > 0 ? 1 : -1;

            if (sign != nextsign)
            {
                Position = Targets[TargetIndex];
                Velocity = 0;
            }
            else
            {
                Position += nextVelocity;

            }*/
            //Position += Velocity;
            //transform.position = OffsetPosition + Slider.position / 512 + new Vector3(Position, 64 / 512f, 0);
        }

        
        PrevMousePosition = MousePosition;     
	}

    //============================================================================================================================================//
    void FixedUpdate()
    {
        if (!MouseDown)
        {
            float Acceleration = Targets[TargetIndex] - Position;
            Acceleration *= (Speed * Time.deltaTime);

            Velocity += Acceleration;
            Velocity *= Mathf.Max(0, 1 - (Drag * Time.fixedDeltaTime));

            Position += Velocity;
            transform.position = OffsetPosition + Slider.position / 512 + new Vector3(Position, 64 / 512f, 0);
        }

    }

    //============================================================================================================================================//
    Vector3 GetMousePosition()
    {
        Vector3 vec = Vector3.zero;
        Camera cam = GameObject.Find("Camera").camera;
        var w = cam.pixelWidth;
        var h = cam.pixelHeight;

        vec = new Vector3((Input.mousePosition.x / w - 0.5f) * cam.orthographicSize * cam.aspect * 2, (Input.mousePosition.y / h - 0.5f) * cam.orthographicSize * 2, 0);

        return vec;
    }
}
