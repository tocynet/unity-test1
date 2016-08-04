using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour {

    public static GlobalController instance;

	// Use this for initialization
	void Start () {
	    if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
        Debug.Log("GlobalController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
