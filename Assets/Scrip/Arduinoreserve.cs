using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
public class Arduinoreserve : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com5", 38400);
    private Thread serialThread;
    public int WaveVector;
    public string Newdate;
    public string confirm;

    public LightingCode lightingcode;
    public GameObject rain;
    public GameObject raindot;
    private Material material;
    //public Behaviour throwrock;

    public bool triggerLighting = false;  // 用於主執行緒更新狀態的旗標


    // Start is called before the first frame update
    void Start()
    {
        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        material = renderer.material;

        material.SetFloat("_Ripple_Strengh", 0);
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
    void Update()
    {
        
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

    private void ReadSerialData()
    {
        while (true)
        {
            if (sp.IsOpen)
            {
                confirm = sp.ReadLine();
                Newdate = sp.ReadLine();
                int.TryParse(Newdate, out WaveVector);//把Newdate轉成int放到waveVrctor

                Debug.Log("confirm:" + confirm);
                Debug.Log("Newdata:" + WaveVector);

                // 檢查條件是否滿足，然後設定旗標
                if (confirm == "T")
                {
                    triggerLighting = true;
                }

                if(confirm =="R" && Newdate != null)
                {
                    rain.SetActive(true);
                    material.SetFloat("_Ripple_Strengh", 0.1f);
                    //WaveTree.Newdate = Newdate;
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
