using CosineKitty;
using nminhhoangit.SunCalculator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seasonal_Control : MonoBehaviour
{
    public string now_time;
    public int now_hour;
    public int now_minute,last_minute;
    public float timer;
    private float last_timer;

    public SunCalculator sunCalculator;
    public bool timeButton;
    public enum SeasonState { Spring, Summer, Autumn , Winter };
    [Header("當前季節")]
    public SeasonState state;
    [Header("季節變化物件")]
    public GameObject floor;
    public Texture2D[] seasonal_textures;
    public GameObject[] trees;
    public Material seasonal_M;
    private float count;
    [Header("果實生成紀錄器")]
    public bool Isgenarate;
    public int fruit_numbers;
    [Header("測試用")]
    public bool test;

    // Start is called before the first frame update
    void Start()
    {
        last_timer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (fruit_numbers >= 10)
        {
            Isgenarate = false;
            fruit_numbers = 0;
        }




        timer = Time.time;
        if (timer > (last_timer + 25)) 
        {
            last_timer = timer;
            sunCalculator.m_Hour += 1;
            if (sunCalculator.m_Hour > 23) 
            {
                sunCalculator.m_Hour  = 0;
            }
        }
        now_time = DateTime.Now.ToString();
        now_hour = DateTime.Now.Hour;
        now_minute = DateTime.Now.Minute;
        if (now_hour >= 9 && now_hour < 11 &&test ==false)
        {
            state = SeasonState.Spring;
        }
        else if (now_hour >= 11 && now_hour < 13 && test == false)
        {
            state = SeasonState.Summer;
        }
        else if (now_hour >= 13 && now_hour < 14 && test == false)
        {
            state = SeasonState.Autumn;
        }
        else if (now_hour >= 14 && now_hour <= 15 && test == false)
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
            last_minute = now_minute;
        }
    }
    void seasonalControler() 
    {
        switch (state) 
        {
            case SeasonState.Spring:
                floor.GetComponent<MeshRenderer>().materials[0].SetTexture("_Albedo", seasonal_textures[0]);
                break;
            case SeasonState.Summer:
                floor.GetComponent<MeshRenderer>().materials[0].SetTexture("_Albedo", seasonal_textures[1]);
                break;
            case SeasonState.Autumn:
                floor.GetComponent<MeshRenderer>().materials[0].SetTexture("_Albedo", seasonal_textures[2]);
                break;
            case SeasonState.Winter:
                floor.GetComponent<MeshRenderer>().materials[0].SetTexture("_Albedo", seasonal_textures[3]);
                break;
        }
    }
}
