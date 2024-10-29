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
    [Header("�{�q���d�̤j�ɶ�")]
    public float maxThunderDur = 0.5f;
    float thunderDuration;
    [Header("�������d�̤j�ɶ�")]
    public float maxThunderBreakDur = 0.5f;
    float thunderBreakDuration;
    [Header("�U�@�`�����ݮɶ�")]
    public float maxThunderRestDur;
    float thunderRestDur;
    int serialThunderTime;
    [Header("�{�q����")]
    public int maxSerialThunderTime = 5;

    [Header("�̤j�G��")]
    public float IntensityMaxOne = 6f;
    public float IntensityMaxTwo = 10f;
    float Max;
    [Header("�̤p�G��")]
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
        // �}��SerialPort�ñҰ�SerialPortŪ���u�{
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
        // �T�O�w������SerialPort�M�u�{
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

                    // �b�S�w���p�UĲ�o�{�q�ĪG
                    if (Tconfirm == "T" && WaveVector == 0)
                    {
                        startThunder = true;
                    }
                }
                catch { }
            }
            Thread.Sleep(100); // ����Ū���W�v�A�קK�L�ץe��CPU
        }
    }

    private void FixedUpdate()
    {
        thunderLight.color = colorlight;

        // �u�b�ݭn��Ĳ�o�{�q�ĪG
        if (startThunder && !isThunderRunning)
        {
            startThunder = false; // ���m�аO�A�קK����Ĳ�o
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
