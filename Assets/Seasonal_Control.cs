using CosineKitty;
using nminhhoangit.SunCalculator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seasonal_Control : MonoBehaviour
{
    public string now_time;
    private int now_hour;
    private int now_minute,last_minute;
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
    // Start is called before the first frame update
    void Start()
    {
        last_timer = timer;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (now_hour >= 9 && now_hour < 11)
        {
            state = SeasonState.Spring;
        }
        else if (now_hour >= 11 && now_hour < 13)
        {
            state = SeasonState.Summer;
        }
        else if (now_hour >= 13 && now_hour < 14)
        {
            state = SeasonState.Autumn;
        }
        else if (now_hour >= 14 && now_hour <= 15)
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
            count += 0.016f;
            seasonal_M.SetFloat("_Falling", count);

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
