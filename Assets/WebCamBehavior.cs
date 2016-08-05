﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;

using ZXing;
using ZXing.QrCode;

public class WebCamBehavior : MonoBehaviour
{
    public WebCamTexture webcamTexture;
    public int Width = 1920;
    public int Height = 1080;
    public int FPS = 30;
    public static bool inited = false;

    public Color32[] color32;
    public Texture2D encodedTexture;

    public Thread qrThread;
    public Rect screenRect;
    public bool isQuit;

    public string lastResult;

    void OnEnable()
    {
        Debug.Log("OnEnable()");
        if (this.webcamTexture != null)
        {
            this.webcamTexture.Play();
            this.Width = this.webcamTexture.width;
            this.Height = this.webcamTexture.height;
        }
    }

    void OnDisable()
    {
        Debug.Log("OnDisable()");
        if (this.webcamTexture != null)
        {
            this.webcamTexture.Stop();
            this.webcamTexture = null;
            WebCamBehavior.inited = false;
        }
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy()");
        this.qrThread.Abort();
        if (this.webcamTexture != null)
        {
            this.webcamTexture.Stop();
            this.webcamTexture = null;
            WebCamBehavior.inited = false;
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit()");
        this.isQuit = true;
    }

    // Use this for initialization
    void Start ()
    {
        WebCamBehavior.inited = false;
        Debug.Log("WebCamBehavior Start()");

        qrThread = new Thread(this.DecodeQR);
        qrThread.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!WebCamBehavior.inited && MsgText.instance!=null)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length > 0)
            {
                this.webcamTexture = new WebCamTexture(devices[0].name);
                GetComponent<Renderer>().material.mainTexture = this.webcamTexture;
                this.webcamTexture.Play();
                this.Width = this.webcamTexture.width;
                this.Height = this.webcamTexture.height;
                MsgText.instance.msg = "カメラ・キャプチャ中";
            }
            else
            {
                Debug.Log("カメラが見つかりませんでした。");
                MsgText.instance.msg = "カメラが見つかりませんでした。";
            }
            WebCamBehavior.inited = true;
        }  else
        {
            if (this.color32 == null)
            {
                this.color32 = this.webcamTexture.GetPixels32();
            }
        }
     
    }


    public void OnClick()
    {
        Debug.Log("Button Clicked!!");
        MsgText.instance.msg = "Button Clicked!!";
    }

    void DecodeQR()
    {
        var barcodeReader = new BarcodeReader { AutoRotate = false };

        while (true)
        {
            if (this.isQuit)
                break;

            try
            {
                // decode the current frame
                var result = barcodeReader.Decode(this.color32, this.Width, this.Height);
                if (result != null)
                {
                    this.lastResult = result.Text;
                    var tmp_msg = "Decoded: " + result.Text;
                    Debug.Log(tmp_msg);
                    MsgText.instance.msg = tmp_msg;
                }

                Thread.Sleep(200);
            }
            catch
            {
                // catch error ??
            }
        }
    }
}
