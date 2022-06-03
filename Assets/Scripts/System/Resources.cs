using Assets.Scripts.DamagedObject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public static class Resources
{
    //public static List<string> AllResources = new List<string>();

    public static List<GameObject> SelectedUnits = new List<GameObject>();

    public static float MainHeroGold = 155;
    public static float MainHeroExperience = 0;

    public static float MainHeroHP;
    public static float MainHeroSpeed;
    public static float MainHeroDamage;
    public static float MainHeroArmor;
    public static float MainHeroDistance;
    public static float MainHeroSpeedAttack;


    //public static List<object> MainHeroItems;

    public static GameObject MainHeroPrefab;
    public static Texture MainHeroImage;


    public static float volume = 0; //Громкость
    public static int quality = 0; //Качество
    public static bool isFullscreen = true; //Полноэкранный режим
    public static AudioMixer audioMixer; //Регулятор громкости
    public static Dropdown resolutionDropdown; //Список с разрешениями для игры
    public static Resolution[] resolutions; //Список доступных разрешений
    public static int currResolutionIndex = 0; //Текущее разрешение
    public static bool MiniMapState = true;
    public static bool FPSState = false;


    public static float FriendlyUnitHpBoost = 0;
    public static float FriendlyUnitDamageBoost = 0;


    public static HashSet<DamagedObject> AllObjects = new HashSet<DamagedObject>();

    public static int Timer = 0;
    public static int DeadTime = 0;

    public static int BaracEnemyCount = 2;
    public static int BaracFriendCount = 1;

    public static int DeathCount;


    public static void ReCreate()
    {

        SelectedUnits.Clear();

        MainHeroGold = 100;
        MainHeroExperience = 0;

        MainHeroHP = 0; ;
        MainHeroSpeed = 0;
        MainHeroDamage = 0;
        MainHeroArmor = 0;
        MainHeroDistance = 0;
        MainHeroSpeedAttack = 0;

        //public static List<object> MainHeroItems;

        MainHeroPrefab = null;
        MainHeroImage = null;

        FriendlyUnitHpBoost = 0;

        AllObjects.Clear();

        Timer = 0;
        DeadTime = 0;

        BaracEnemyCount = 2;
        BaracFriendCount = 1;

        DeathCount = 0;
    }

}