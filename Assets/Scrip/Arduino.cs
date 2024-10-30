using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using Palmmedia.ReportGenerator.Core.Common;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Threading;

public class Arduino : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com5", 38400);
    private Thread serialThread;
    //public float[] num;
    private float Pos;
    public float time;
    private string Newdate;
    private int Olddate;
    public int WaveVector;
    private string Tconfirm;
    private int transpos;
    public bool triggerCamera;
    //public string[] WORD ,newdata;//1.角度2.x軸3.Y軸4.Z軸
    // Start is called before the first frame update

    void Start()
    {
        try
        {
            sp.Open();
            serialThread = new Thread(ReadSerialData);
            serialThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open Serial Port: " + e.Message);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 在主執行緒中檢查旗標，並根據需要啟動 lightingcode
        if (triggerCamera)
        {
            StartCoroutine(roundCamera());
            triggerCamera = false;  // 重置旗標
                                      // lightingcode.enabled = false;
        }
    }
    private void ReadSerialData()
    {
        while (true)
        {
            if (sp.IsOpen)
            {
                Newdate = sp.ReadLine();

                //Pos = float.Parse(Newdate);
                float.TryParse(Newdate, out Pos);
                transpos = (int)Pos;

               // int.TryParse(Newdate, out WaveVector);

                // 檢查條件是否滿足，然後設定旗標
                Debug.Log(transpos);

                if(Newdate != null)
                {
                    triggerCamera = true;
                }
            }
            Thread.Sleep(10); // 控制讀取頻率，避免過度占用CPU
        }
    }

    private IEnumerator roundCamera()
    {
        if (transpos != null)
        {
            if (transpos != Olddate)
            {
                if (transpos > 0 || transpos < 0)
                {
                    int gap = transpos - Olddate;
                    //this.transform.position = new Vector3(Pos, 0, 0);
                    // this.transform.Rotate(Vector3.right * this.transform.rotation.x * Pos);
                    //                    this.transform.rotation = this.transform.rotation +  Quaternion.Angle;
                    //if (time % 1.0f ==0)
                    //{
                    this.transform.Rotate(0, -gap / 3.5f, 0, Space.Self);
                    //}
                }
            }
            Olddate = transpos;
        }
        yield return new WaitForSeconds(0.5f);  // 例如延遲0.1秒
    }
}
    

