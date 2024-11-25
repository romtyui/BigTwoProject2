using System.Collections;
using System.Threading;
using UnityEngine;
using System.IO.Ports;

public class Arduino : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com3", 38400);
    private Thread serialThread;
    private float Pos;
    private int Olddate;
    private int transpos;
    private bool triggerCamera;
    private bool isRotating = false; // 是否正在旋轉

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
        if (triggerCamera && !isRotating) // 如果觸發旋轉且未在旋轉中
        {
            StartCoroutine(roundCamera());
            triggerCamera = false; // 重置觸發標誌
        }
    }

    private void ReadSerialData()
    {
        while (true)
        {
            if (sp.IsOpen)
            {
                try
                {
                    string rawData = sp.ReadLine();
                    if (float.TryParse(rawData, out float newAngle))
                    {
                        Pos = newAngle;
                        transpos = Mathf.RoundToInt(Pos);
                        triggerCamera = true; // 設置觸發標誌
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Serial Read Error: " + ex.Message);
                }
            }
            Thread.Sleep(10);
        }
    }

    private IEnumerator roundCamera()
    {
        if (transpos != Olddate && !isRotating) // 如果角度變化且未在旋轉中
        {
            // 計算目標角度
            int newQuadrant = Mathf.FloorToInt((transpos + 180f) / 90f) % 4;
            int targetAngle = newQuadrant * 90;

            // 開始平滑旋轉
            yield return SmoothRotateToAngle(targetAngle);

            Olddate = transpos; // 更新上一個角度
        }
    }

    private IEnumerator SmoothRotateToAngle(float targetAngle)
    {
        isRotating = true; // 開始旋轉
        float duration = 1.0f; // 平滑旋轉所需時間（秒）
        float elapsedTime = 0f; // 已經過的時間
        Quaternion startRotation = transform.rotation; // 初始旋轉角度
        Quaternion endRotation = Quaternion.Euler(0, targetAngle, 0); // 目標旋轉角度

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // 更新已經過的時間
            float t = elapsedTime / duration; // 時間比例
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t); // 平滑旋轉
            yield return null; // 等待下一幀
        }

        transform.rotation = endRotation; // 確保最終旋轉到目標角度
        isRotating = false; // 結束旋轉
    }

    private void OnDestroy()
    {
        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Abort();
        }
        if (sp.IsOpen)
        {
            sp.Close();
        }
    }
}
