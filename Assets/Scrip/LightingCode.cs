using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;

public class LightingCode : MonoBehaviour
{
    public Transform ground;
    // Start is called before the first frame update
    float groundwidth, groundlength;
    Light thunderLight;
    public Color colorlight;
    [Header("??光停留最大??")]
    public float maxThunderDur = 0.5f;
    float thunderDuration;
    [Header("?弱光停留最大??")]
    public float maxThunderBreakDur = 0.5f;
    float thunderBreakDuration;
    [Header("下一循?? Q ")]
    public float maxThunderRestDur;
    float thunderRestDur;
    int serialThunderTime;
    [Header("?光次?")]
    public int maxSerialThunderTime = 5;

    [Header("最大?度范?")]
    public float IntensityMaxOne = 6f;
    public float IntensityMaxTwo = 10f;
    float Max;
    [Header("最小?度范?")]
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
        StartCoroutine(Thunder());
    }
    void FixedUpdate()
    {
        thunderLight.color = colorlight;
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
