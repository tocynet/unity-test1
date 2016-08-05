using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreText : MonoBehaviour {

    public string msg = "";
    public int score = 0;
    public static ScoreText instance;

    // Use this for initialization
    void Start()
    {
        this.msg = "Score: 0";
        this.score = 0;
        if (instance == null)
        {
            instance = this;
        }
        // DontDestroyOnLoad(this);
        Debug.Log("ScoreText class");
    }
	
	// Update is called once per frame
	void Update () {
        this.msg = "Score: " + this.score.ToString();
        this.GetComponent<Text>().text = this.msg;
    }
}
