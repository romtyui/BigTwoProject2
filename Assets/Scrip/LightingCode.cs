using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class LightingCode : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com3", 115200);
    public float WaveVector, Vector, timer, oringnaltimer;
    private string Newdate;

    public Transform ground;
    // Start is called before the first frame update
    float groundwidth, groundlength;
    Light thunderLight;
    public Color colorlight;
    [Header("閃電停留最大時間")]
    public float maxThunderDur = 0.5f;
    float thunderDuration;
    [Header("結束停留最大時間")]
    public float maxThunderBreakDur = 0.5f;
    float thunderBreakDuration;
    [Header("下一循環等待時間 ")]
    public float maxThunderRestDur;
    float thunderRestDur;
    int serialThunderTime;
    [Header("閃電次數")]
    public int maxSerialThunderTime = 5;

    [Header("最大亮度")]
    public float IntensityMaxOne = 6f;
    public float IntensityMaxTwo = 10f;
    float Max;
    [Header("最小亮度")]
    public float IntensityMinOne = 1.5f;
    public float IntensityMinTwo = 3f;
    float Min;

    private void Awake()
    {
        thunderLight = GetComponent<Light>();
        thunderLight.color = colorlight;
    }
    void Start()
    {
        
        sp.Open();
    }
    void FixedUpdate()
    {
        thunderLight.color = colorlight;
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
            StartCoroutine(Thunder());
        }
    }


    IEnumerator Thunder()
    {
        while (true)
        {
            serialThunderTime = Random.Range(0, maxSerialThunderTime+1);
            for (int i = 0;i < serialThunderTime; i++) 
            {
                thunderDuration =Random.Range(0,maxThunderDur);
                Max = Random.Range(IntensityMinOne, IntensityMaxTwo);
                thunderLight.intensity = Max;
                yield return new WaitForSeconds(thunderBreakDuration);
                Min = Random.Range(IntensityMinOne,IntensityMaxTwo);
                thunderLight.intensity = Min;
                thunderBreakDuration = Random.Range(0, maxThunderBreakDur);
                yield return new WaitForSeconds(thunderBreakDuration);
            }
            thunderRestDur = Random.Range(0,maxThunderRestDur);
            yield return new WaitForSeconds(maxThunderRestDur);
        }
    }
}
