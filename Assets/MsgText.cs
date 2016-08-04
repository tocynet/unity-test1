using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MsgText : MonoBehaviour {

    public string msg = "";
    public static MsgText instance;

    // Use this for initialization
    void Start()
    {
        this.msg = "";
        if (instance == null)
        {
            instance = this;
        }
        // DontDestroyOnLoad(this);
        Debug.Log("MsgText class");
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Text>().text = this.msg;
    }

}
