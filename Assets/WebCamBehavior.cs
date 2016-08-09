using UnityEngine;
using UnityEngine.UI;
using System;
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

    public Color32[] color32 = null;
    public bool isCaptured = false;
    public Texture2D encodedTexture;

    public Thread qrThread;
    public Rect screenRect;
    public bool isQuit = false;

    public BarcodeReader barcodeReader;
    public string lastResult = null;

    public int score = 0;

    public Quaternion baseRotation;

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

        this.qrThread = new Thread(this.decodeThreadLoop);
        this.qrThread.Start();

        this.score = 0;
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
                this.baseRotation = this.transform.rotation;
            }
            else
            {
                Debug.Log("カメラが見つかりませんでした。");
                MsgText.instance.msg = "カメラが見つかりませんでした。";
            }
            WebCamBehavior.inited = true;
        }  else
        {
            // MsgText.instance.msg = "prepare capture.";
            // if (this.color32 == null)
            if (!this.isCaptured)
            {
                MsgText.instance.msg = "capturing.";
                this.color32 = this.webcamTexture.GetPixels32();
                var tmp_msg = "Camera Captured...";
                // Debug.Log(tmp_msg);
                MsgText.instance.msg = tmp_msg;
                this.resetResult();
                this.isCaptured = true;
            }
        }
        if (this.webcamTexture != null)
        {
            this.transform.rotation = this.baseRotation * Quaternion.AngleAxis(this.webcamTexture.videoRotationAngle, Vector3.up);
        }     
    }


    public void OnClickNext()
    {
        Debug.Log("Next Button Clicked!!");
        // MsgText.instance.msg = "Button Clicked!!";
        if (this.lastResult != null)
        {
            if (this.lastResult.Length > 5)
            {
                this.lastResult = this.lastResult.Substring(8);
            }
        }
        int tmp;
        if(Int32.TryParse(this.lastResult, out tmp))
        {
            this.score += tmp;
            ScoreText.instance.score = this.score;
        }
        this.resetCapturing();
        this.resetResult();
    }

    public void OnClickReset()
    {
        Debug.Log("Reset Button Clicked!!");
        // MsgText.instance.msg = "Button Clicked!!";
        this.score = 0;
        ScoreText.instance.score = 0;
        this.resetCapturing();
        this.resetResult();
    }

    public void resetCapturing()
    {
        this.color32 = null;
        this.isCaptured = false;
    }

    public void resetResult()
    {
        this.lastResult = null;
    }

    void decodeThreadLoop()
    {
        this.barcodeReader = new BarcodeReader {
            AutoRotate = true,
            Options = new ZXing.Common.DecodingOptions { TryHarder=false }
        };

        Debug.Log("barcodeReader inited");
        while (true)
        {
            if (this.isQuit)
                break;

            this.decodeBarcode();

            Thread.Sleep(200);
        }
    }

    void decodeBarcode()
    {
        var tmp_msg = "";
        var has_error = false;

        try
        {
            // decode the current frame
            if (this.isCaptured && this.lastResult == null)
            {

                var result = this.barcodeReader.Decode(this.color32, this.Width, this.Height);
                if (result != null)
                {
                    this.lastResult = result.Text;
                    var t_msg = "Decoded: " + result.Text;
                    Debug.Log(t_msg);
                    MsgText.instance.msg = t_msg;
                }
                else
                {
                    // Debug.Log("no result");
                    this.resetCapturing();
                }
            }
            else
            {
                // Debug.Log("not set color32");
            }
        }
        catch (InvalidOperationException ex)
        {
            // catch error ??
            tmp_msg = "not capturing ??\n" + ex.ToString();
        }
        catch (ArgumentNullException ex)
        {
            tmp_msg = "not capturing ??\n" + ex.ToString();
        }
        finally
        {
            if (has_error)
            {
                MsgText.instance.msg = tmp_msg;
            }
        }
        // this.resetCapturing();
    }
}
