using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour 
{
    public UIAtlas Atlas;
    public UIAtlas SDAtlas;
    public UIAtlas HDAtlas;
    
    //=======================================================================================================================================================================//
    void Awake()
    {
        UILabel label = GameObject.Find("Debugger").GetComponent<UILabel>();
        label.text = Screen.width + " : " + Screen.height;

        // Retina Ipad //
        if (Screen.width == 1536)
        {
            Atlas.replacement = HDAtlas;
        }

        Destroy(this.gameObject);
	}
}
