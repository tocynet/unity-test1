using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WebCamBehavior : MonoBehaviour
{
    public int Width = 1920;
    public int Height = 1080;
    public int FPS = 30;
    public static bool _inited = false;


    // Use this for initialization
    void Start ()
    {
        _inited = false;
        Debug.Log("WebCamBehavior Start()");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!_inited && MsgText.instance!=null)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length > 0)
            {
                var webcamTexture = new WebCamTexture(devices[0].name);
                GetComponent<Renderer>().material.mainTexture = webcamTexture;
                webcamTexture.Play();
                MsgText.instance.msg = "カメラ・キャプチャ中";
            }
            else
            {
                Debug.Log("カメラが見つかりませんでした。");
                MsgText.instance.msg = "カメラが見つかりませんでした。";
            }
            _inited = true;
        }
    }


    public void OnClick()
    {
        Debug.Log("Button Clicked!!");
        MsgText.instance.msg = "Button Clicked!!";
    }


}
