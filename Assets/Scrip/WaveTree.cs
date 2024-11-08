using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using Unity.VisualScripting;
using UnityEngine;

public class WaveTree : MonoBehaviour
{
    // Start is called before the first frame update
    public SerialPort sp = new SerialPort("com3", 115200);
    //public float[] num;

    public float WaveVector,Vector,timer,oringnaltimer;
    public static string Newdate;
    private float lastWaveVector;
    private float NewRot;
    private float OrginalRot;
    //public string[] WORD ,newdata;//1.角度2.x軸3.Y軸4.Z軸
    // Start is called before the first frame update

    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {
                WaveVector = float.Parse(Newdate);
                 //timer += Time.deltaTime;
     Debug.Log(WaveVector);
        if (oringnaltimer != timer)
        {
            if (WaveVector != null)
            {
                if (WaveVector != lastWaveVector)
                {
                    // 获取当前的Z轴旋转角度
                    NewRot = this.transform.localRotation.z;

                    // 检查当前旋转角度是否超出范围
                    if (NewRot > 33)
                    {
                        NewRot = 33;
                    }
                    else if (NewRot < 13)
                    {
                        NewRot = 13;
                    }

                    // 如果当前的WaveVector会导致超出13到33的范围，则不应用它
                    float nextRot = NewRot + WaveVector;
                    if (nextRot > 33)
                    {
                        nextRot = 33;
                    }
                    else if (nextRot < 13)
                    {
                        nextRot = 13;
                    }

                    // 应用旋转
                    this.transform.localEulerAngles = new Vector3(
                        this.transform.localEulerAngles.x,
                        this.transform.localEulerAngles.y,
                        nextRot
                    );

                    // 保存当前状态
                    OrginalRot = nextRot;
                    lastWaveVector = WaveVector;
                }
            }
            oringnaltimer = timer;
        }

    }
}
