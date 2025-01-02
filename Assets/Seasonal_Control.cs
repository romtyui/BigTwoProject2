using CosineKitty;
using nminhhoangit.SunCalculator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Seasonal_Control : MonoBehaviour
{
    public string now_time;
    public int now_hour;
    public int now_minute,last_minute;
    public float timer;
    private float last_timer;

    [Header("渲染區")]
    public SunCalculator sunCalculator;
    public bool timeButton;
    public Volume rendering_obj;  // 引用场景中的 Volume 组件
    public ColorAdjustments colorAdjustments__obj;
    public float brightness;
    public enum SeasonState { Spring, Summer, Autumn , Winter };
    [Header("當前季節")]
    public SeasonState state;
    [Header("季節變化物件")]
    public Terrain terrain; // 將你的 Terrain 拖入此處
    public TerrainLayer[] currentLayers; // 新的 Terrain Layer
    public Texture2D[] seasonal_textures;
    public GameObject[] trees;
    public Material seasonal_M,grass_M;
    private float count;
    [Header("果實生成紀錄器")]
    public bool Isgenarate;
    public int fruit_numbers;
    [Header("測試用")]
    public bool test;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fruit_numbers >= 100)
        {
            Isgenarate = false;
            fruit_numbers = 0;
        }
        timer = Time.time;
        if (timer > (last_timer + 1))
        {
            last_timer = timer;
            sunCalculator.m_Hour += 1;
            if (sunCalculator.m_Hour <= 6)
            {
                brightness += 0.5f;
            }
            else if (sunCalculator.m_Hour >= 20)
            {
                
                brightness -= 0.8f;
            }
            else if(sunCalculator.m_Hour > 6 && sunCalculator.m_Hour < 20)
            {
                brightness = 0f;
            }

            if (rendering_obj.profile.TryGet<ColorAdjustments>(out colorAdjustments__obj))
            {
                // 在这里修改 Intensity
                colorAdjustments__obj.postExposure.value = brightness;
            }
            if (sunCalculator.m_Hour > 23) 
            {
                sunCalculator.m_Hour  = 0;
            }
        }
        now_time = DateTime.Now.ToString();
        now_hour = DateTime.Now.Hour;
        now_minute = DateTime.Now.Minute;
        if (now_hour >= 13 && now_hour < 14 &&test ==false)
        {
            state = SeasonState.Spring;
        }
        else if (now_hour >= 14 && now_hour < 15 && test == false)
        {
            state = SeasonState.Summer;
        }
        else if (now_hour >= 15 && now_hour < 16 && test == false)
        {
            state = SeasonState.Autumn;
        }
        else if (now_hour >= 16 && now_hour <= 17 && test == false)
        {
            state = SeasonState.Winter;
        }
        seasonalControler();
        if (timeButton) 
        {
            sunCalculator.m_Hour = now_hour;
        }
        if (now_minute != last_minute) 
        {
            count += 0.002f;
            seasonal_M.SetFloat("_Falling", count);
            Isgenarate = true;
            StartCoroutine(Waittime(0.1f));
            
        }
    }
    void seasonalControler() 
    {
        switch (state) 
        {
            case SeasonState.Spring:
                terrain.GetComponent<Terrain>().terrainData.terrainLayers[0].diffuseTexture = seasonal_textures[0];
                grass_M.SetFloat("_alpha", 0.5f);

                //floor.GetComponent<MeshRenderer>().materials[0].SetTexture("_Albedo", seasonal_textures[0]);
                break;
            case SeasonState.Summer:
                terrain.GetComponent<Terrain>().terrainData.terrainLayers[0].diffuseTexture = seasonal_textures[1];
                grass_M.SetFloat("_alpha", 0.5f);

                //floor.GetComponent<MeshRenderer>().materials[0].SetTexture("_Albedo", seasonal_textures[1]);
                break;
            case SeasonState.Autumn:
                terrain.GetComponent<Terrain>().terrainData.terrainLayers[0].diffuseTexture = seasonal_textures[2];
                grass_M.SetFloat("_alpha", 0.5f);

                //floor.GetComponent<MeshRenderer>().materials[0].SetTexture("_Albedo", seasonal_textures[2]);
                break;
            case SeasonState.Winter:
                terrain.GetComponent<Terrain>().terrainData.terrainLayers[0].diffuseTexture = seasonal_textures[3];
                grass_M.SetFloat("_alpha", 0.5f);

                //terrain.terrainData.terrainLayers = new TerrainLayer[] { currentLayers[3] };
                //floor.GetComponent<MeshRenderer>().materials[0].SetTexture("_Albedo", seasonal_textures[3]);
                grass_M.SetFloat("_alpha", 2.0f);
                break;
        }
    }
    private IEnumerator Waittime(float x)
    {


        // 等待指定的时间（比如 2 秒）
        yield return new WaitForSeconds(x);
        last_minute = now_minute;
    }
}
