using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using Unity.VisualScripting;
using UnityEngine;

public class SummerThunder : MonoBehaviour
{
    // Start is called before the first frame update
    public SerialPort sp = new SerialPort("com3", 115200);
    //public float[] num;

    public float WaveVector, Vector, timer, oringnaltimer;
    private string Newdate;
    private float lastWaveVector;
    private float NewRot;
    private float OrginalRot;
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
                WaveVector = int.Parse(Newdate);
            }
        }
        catch
        {

        }
        finally
        {
            timer += Time.deltaTime;
        }
       if(WaveVector != 0)
       {
            //triggerAnime
       }
    }
}
