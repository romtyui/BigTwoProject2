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

    public bool triggerLighting = false;  // �Ω�D�������s���A���X��

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
        // �b�D��������ˬd�X�СA�îھڻݭn�Ұ� lightingcode
        if (triggerLighting)
        {
            StartCoroutine(TriggerLightingEffect());
            triggerLighting = false;  // ���m�X��
           // lightingcode.enabled = false;
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
                Tconfirm = sp.ReadLine();
                Newdate = sp.ReadLine();
                int.TryParse(Newdate, out WaveVector);
                
                Debug.Log("Tconfirm:" + Tconfirm);
                Debug.Log("Newdata:" + WaveVector);

                // �ˬd����O�_�����A�M��]�w�X��
                if (Tconfirm == "T" && WaveVector == 0 )
                {
                    triggerLighting = true;
                }
            }
            Thread.Sleep(10); // ����Ū���W�v�A�קK�L�ץe��CPU
        }
    }

    private IEnumerator TriggerLightingEffect()
    {
        lightingcode.enabled = true;
        yield return new WaitForSeconds(0.5f);  // �Ҧp����0.1��
        lightingcode.enabled = false;
    }
}
