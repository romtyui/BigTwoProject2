using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class LightingCode : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com3", 115200);
    public float WaveVector;
    private string Tconfirm;
    private bool startThunder = false;
    private bool isThunderRunning = false;
    private Thread serialThread;

    Light thunderLight;
    public Color colorlight;
    [Header("閃電停留最大時間")]
    public float maxThunderDur = 0.5f;
    float thunderDuration;
    [Header("結束停留最大時間")]
    public float maxThunderBreakDur = 0.5f;
    float thunderBreakDuration;
    [Header("下一循環等待時間")]
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

    private void Start()
    {
        // 開啟SerialPort並啟動SerialPort讀取線程
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

    private void OnDestroy()
    {
        // 確保安全關閉SerialPort和線程
        if (serialThread != null && serialThread.IsAlive)
            serialThread.Abort();

        if (sp.IsOpen)
            sp.Close();
    }

    private void ReadSerialData()
    {
        while (true)
        {
            if (sp.IsOpen)
            {
                try
                {
                    Tconfirm = sp.ReadLine();
                    string Newdate = sp.ReadLine();
                    WaveVector = int.Parse(Newdate);

                    // 在特定情況下觸發閃電效果
                    if (Tconfirm == "T" && WaveVector == 0)
                    {
                        startThunder = true;
                    }
                }
                catch { }
            }
            Thread.Sleep(100); // 控制讀取頻率，避免過度占用CPU
        }
    }

    private void FixedUpdate()
    {
        thunderLight.color = colorlight;

        // 只在需要時觸發閃電效果
        if (startThunder && !isThunderRunning)
        {
            startThunder = false; // 重置標記，避免重複觸發
            StartCoroutine(Thunder());
        }
    }

    IEnumerator Thunder()
    {
        isThunderRunning = true;
        serialThunderTime = Random.Range(1, maxSerialThunderTime + 1);

        for (int i = 0; i < serialThunderTime; i++)
        {
            thunderDuration = Random.Range(0, maxThunderDur);
            Max = Random.Range(IntensityMinOne, IntensityMaxTwo);
            thunderLight.intensity = Max;
            yield return new WaitForSeconds(thunderDuration);

            Min = Random.Range(IntensityMinOne, IntensityMaxTwo);
            thunderLight.intensity = Min;
            thunderBreakDuration = Random.Range(0, maxThunderBreakDur);
            yield return new WaitForSeconds(thunderBreakDuration);
        }

        thunderRestDur = Random.Range(0, maxThunderRestDur);
        yield return new WaitForSeconds(thunderRestDur);
        isThunderRunning = false;
    }
}
