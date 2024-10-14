using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class WaveTree : MonoBehaviour
{
    // Start is called before the first frame update
    public SerialPort sp = new SerialPort("com3", 115200);
    //public float[] num;

    public float WaveVector,Vector,timer,oringnaltimer;
    private string Newdate;
    private float lastWaveVector;
    //public string[] WORD ,newdata;//1.¨¤«×2.x¶b3.Y¶b4.Z¶b
    // Start is called before the first frame update

    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (sp.IsOpen)
            {
                Newdate = sp.ReadLine();
                WaveVector = float.Parse(Newdate);
            }
        }
        catch
        {

        }
        finally
        {
            timer += Time.deltaTime;
        }
       if(oringnaltimer != timer) 
        {
            if(WaveVector != null) 
            {
                
                 
                if (WaveVector != lastWaveVector)
                {
                    if(this.transform.localRotation.z >=13 && this.transform.localRotation.z <= 33)
                    {
                        this.transform.localEulerAngles += new Vector3(0, 0, WaveVector);
                    }
                    lastWaveVector = WaveVector;
                }   
            }
            oringnaltimer = timer;
        }
    }
}
