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
    //public string[] WORD ,newdata;//1.����2.x�b3.Y�b4.Z�b
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
        // �b�D��������ˬd�X�СA�îھڻݭn�Ұ� lightingcode
        if (triggerCamera)
        {
            StartCoroutine(roundCamera());
            triggerCamera = false;  // ���m�X��
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

                // �ˬd����O�_�����A�M��]�w�X��
                Debug.Log(transpos);

                if(Newdate != null)
                {
                    triggerCamera = true;
                }
            }
            Thread.Sleep(10); // ����Ū���W�v�A�קK�L�ץe��CPU
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
        yield return new WaitForSeconds(0.5f);  // �Ҧp����0.1��
    }
}
    

