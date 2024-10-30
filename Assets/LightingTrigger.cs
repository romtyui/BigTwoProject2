using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class LightingTrigger : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com5", 115200);
    private Thread serialThread;
    public int WaveVector;
    private string Newdate;
    private string Tconfirm;

    public Behaviour lightingcode;

    public bool triggerLighting = false;  // 用於主執行緒更新狀態的旗標

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

    void FixedUpdate()
    {
        // 在主執行緒中檢查旗標，並根據需要啟動 lightingcode
        if (triggerLighting)
        {
            StartCoroutine(TriggerLightingEffect());
            triggerLighting = false;  // 重置旗標
           // lightingcode.enabled = false;
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
                Tconfirm = sp.ReadLine();
                Newdate = sp.ReadLine();
                int.TryParse(Newdate, out WaveVector);
                
                Debug.Log("Tconfirm:" + Tconfirm);
                Debug.Log("Newdata:" + WaveVector);

                // 檢查條件是否滿足，然後設定旗標
                if (Tconfirm == "T" && WaveVector == 0 )
                {
                    triggerLighting = true;
                }
            }
            Thread.Sleep(10); // 控制讀取頻率，避免過度占用CPU
        }
    }

    private IEnumerator TriggerLightingEffect()
    {
        lightingcode.enabled = true;
        yield return new WaitForSeconds(0.5f);  // 例如延遲0.1秒
        lightingcode.enabled = false;
    }
}
