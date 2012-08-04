using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Replacer : EditorWindow 
{
    string myString = "Hello World";
    bool groupEnabled; bool myBool = true;
    float myFloat = 1.23f;

    List<Object> Replacements = new List<Object>();

    //============================================================================================================================================//
    [MenuItem("Inhuman/Replacer")]
    static void Init () 
    { 
        // Get existing open window or if none, make a new one:       
        Replacer window = (Replacer)EditorWindow.GetWindow(typeof(Replacer));
    }

    //============================================================================================================================================//
    void OnGUI() 
    {
        //EditorGUILayout.Foldout(true, "Selected Objects");
        GUILayout.Label ("Selected Objects", EditorStyles.boldLabel);          

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            GUILayout.Label(Selection.gameObjects[i].name);
        }
        //GUILayout.Box(null, GUIStyle.none

        //EditorGUILayout.Foldout(true, "Replacement Objects");
        GUILayout.Label("Replacement Objects", EditorStyles.boldLabel);
        if (GUILayout.Button("New"))
        {
            Replacements.Add(null);
        }

        for (int i = 0; i < Replacements.Count; i++)
        {
            GUILayout.BeginHorizontal();

            Replacements[i] = EditorGUILayout.ObjectField(Replacements[i], typeof(Object), true);
            if (GUILayout.Button("Replace"))
            {
                ReplaceAll(Replacements[i]);
            } 
            if (GUILayout.Button("X"))
            {
                Replacements.Remove(Replacements[i]);
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Random Colors"))
        {
            ReplaceRandom(Selection.transforms);
        }


        //EditorGUILayout.PropertyField(ColorA, true);

        //EditorGUILayout.Foldout(true, "Options");
        //GUILayout.Label("Options", EditorStyles.boldLabel);
        //GUILayout.BeginArea();
        //GUILayout.Toggle(true, "Position");
        //GUILayout.Toggle(true, "Rotation");
        //GUILayout.Toggle(true, "Scale");
        //GUILayout.Toggle(false, "Keep Original");
        //EditorGUILayout.EndVertical();
    }
    
    //============================================================================================================================================//
    void ReplaceRandom(Transform[] objects)
    {
        Dictionary<int, int> lookup = new Dictionary<int, int>();
        List<int> spriteIDs = new List<int>();
        List<int> indexes = new List<int>();

        //if (objects.Length > 0)
        //{
            Undo.RegisterSceneUndo("Randomly Replaced " + objects.Length + " Ojects with " + Replacements.Count + " New Objects");

            // Create list of current sprite id's //
            List<ColorBase> baseList = new List<ColorBase>();
            ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));

            // Get ID List //
            for (int i = 0; i < bases.Length; i++)
            {
                int id = bases[i].GetComponent<tk2dSprite>().spriteId;

                if (!spriteIDs.Contains(id))
                {
                    spriteIDs.Add(id);
                }               
            }

            // Create Index List //
            for (int i = 0; i < Replacements.Count; i++)
			{
			    indexes.Add(i);
			}

            // Assign random replacement index to each id //
            for (int i = 0; i < spriteIDs.Count; i++)
            {
                int randomIndex = Random.Range(0, indexes.Count);
                lookup.Add(spriteIDs[i], indexes[randomIndex]);
                Debug.Log(indexes[randomIndex] + " : " + indexes);
                indexes.RemoveAt(randomIndex);
                
            }

            // Replace Objects //
            for (int i = 0; i < bases.Length; i++)
            {
                int id = bases[i].GetComponent<tk2dSprite>().spriteId;

                Object replacement = Replacements[lookup[id]];
                ReplaceObject(bases[i].transform, replacement);
            }

        //}
    }

    //============================================================================================================================================//
    void ReplaceAll(Object replacement)
    {
        Transform[] selected = Selection.transforms;

        if (Replacements.Count > 0 && selected.Length > 0)
        {
            Undo.RegisterSceneUndo("Replaced " + selected.Length + " with " + replacement.name);

            for (int i = 0; i < selected.Length; i++)
            {
                ReplaceObject(selected[i], replacement);
            }
        }
    }

    //============================================================================================================================================//
    void ReplaceObject(Transform selected, Object replacement)
    {
        Transform parent = selected.parent;
        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(replacement);
        newObj.transform.localPosition = selected.position;
        newObj.transform.localRotation = selected.localRotation;
        newObj.transform.localScale = selected.lossyScale;
        newObj.transform.parent = parent;

        DestroyImmediate(selected.gameObject);
    }

    //============================================================================================================================================//
    void OnInspectorUpdate() 
    { 
        this.Repaint();
    }
}
