using UnityEngine;
using System.Collections;

public class WebCamBehavior : MonoBehaviour
{
    public int Width = 1920;
    public int Height = 1080;
    public int FPS = 30;

    public void OnClick()
    {
        Debug.Log("Button Clicked!!");
    }

    // Use this for initialization
    void Start ()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            var webcamTexture = new WebCamTexture(devices[0].name);
            GetComponent<Renderer>().material.mainTexture = webcamTexture;
            webcamTexture.Play();
        } else
        {
            Debug.Log("カメラが見つかりませんでした。");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}


}
