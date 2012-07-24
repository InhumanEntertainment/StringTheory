/*using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GameCamera))]
public class GameCameraEditor : Editor
{
    GameCamera Cam;

    //======================================================================================================================================//
    void OnSceneGUI()
    {
        //SetupCamera();

        //if (GUI.changed)
        //EditorUtility.SetDirty(target);
        //SetupCamera();
        SceneView.RepaintAll();
    }

	//======================================================================================================================================//
    public override void OnInspectorGUI()
    {

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Width: " + Width);
            EditorGUILayout.LabelField("Height: " + Height);
        EditorGUILayout.EndHorizontal();

        DrawDefaultInspector();
        EditorUtility.SetDirty(target);
    }

    float Width, Height;

    //===================================================================================================================================================================//
    void SetupCamera()
    {
        //Camera Cam = ((Camera)target);
        GameCamera gameCam = (GameCamera)target;
        Debug.Log(gameCam.camera.aspect);
        float aspect = (float)Screen.width / Screen.height;
        Width = gameCam.GetComponent<Camera>().pixelWidth;
        Height = gameCam.camera.pixelHeight;

        if (gameCam.camera.aspect < 1)
        {
            gameCam.camera.orthographicSize = gameCam.ScreenSize / gameCam.camera.aspect;
        }
        else
        {
            gameCam.camera.orthographicSize = gameCam.ScreenSize;
        }

        EditorUtility.SetDirty(gameCam.camera);
    }
}
*/