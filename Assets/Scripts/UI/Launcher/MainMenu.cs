using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.AI;
using Assets.Scripts.Units;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Scroll scr;

    [SerializeField]
    private GameObject settings;

    [SerializeField]
    private GameObject Scroll;

    public AudioMixer audioMixer; //Регулятор громкости
    public Dropdown resolutionDropdown;

    public Texture[] Images;

    private void Start()
    {
        settings.gameObject.SetActive(false);
        Resources.audioMixer = audioMixer;
        Resources.resolutionDropdown = resolutionDropdown;
        resolutionDropdown.ClearOptions(); //Удаление старых пунктов
        Resources.resolutions = Screen.resolutions; //Получение доступных разрешений
        var options = new List<string>(); //Создание списка со строковыми значениями

        for (int i = 0; i < Resources.resolutions.Length; i++) //Поочерёдная работа с каждым разрешением
        {
            string option = Resources.resolutions[i].width + " x " + Resources.resolutions[i].height; //Создание строки для списка
            options.Add(option); //Добавление строки в список

            if (Resources.resolutions[i].Equals(Screen.currentResolution)) //Если текущее разрешение равно проверяемому
                Resources.currResolutionIndex = i; //То получается его индекс
        }

        resolutionDropdown.AddOptions(options); //Добавление элементов в выпадающий список
        resolutionDropdown.value = Resources.currResolutionIndex; //Выделение пункта с текущим разрешением
        resolutionDropdown.RefreshShownValue(); //Обновление отображаемого значения
    }

    public void Play()
    {
        Resources.MainHeroPrefab = scr.obj[scr.currentNum];
        Resources.MainHeroImage = Images[scr.currentNum];
        Resources.DeadTime = -1;

        var mainHero = Resources.MainHeroPrefab;

        var dO = mainHero.GetComponent<PlayerDamagedObject>();
        Resources.MainHeroHP = dO.MaxHealth;
        Resources.MainHeroArmor = dO.Armor;

        var pC = mainHero.GetComponent<MainCharacterUnit>();
        Resources.MainHeroDamage = pC.Damage;
        Resources.MainHeroDistance = pC.AttackRadius;
        Resources.MainHeroSpeedAttack = pC.ShootDelay;

        Resources.MainHeroSpeed = mainHero.GetComponent<NavMeshAgent>().speed;

        SceneManager.LoadScene("Game");
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
            //ChangeStateOfSettings();
    }

    public void ChangeStateOfSettings()
    {
        settings.gameObject.SetActive(!settings.gameObject.active);
        Scroll.gameObject.SetActive(!Scroll.gameObject.active);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ChangeVolume(float val) //Изменение звука
    {
        Resources.volume = val;
    }

    public void ChangeResolution(int index) //Изменение разрешения
    {
        Resources.currResolutionIndex = index;
    }

    public void ChangeFullscreenMode(bool val) //Включение или отключение полноэкранного режима
    {
        Resources.isFullscreen = val;
    }

    public void ChangeMiniMapState(bool val) //Включение или отключение полноэкранного режима
    {
        Resources.MiniMapState = val;
    }

    public void ChangeFPSState(bool val) //Включение или отключение полноэкранного режима
    {
        Resources.FPSState = val;
    }

    public void ChangeQuality(int index) //Изменение качества
    {
        Resources.quality = index;
    }

    public void SaveSettings()
    {
        QualitySettings.SetQualityLevel(Resources.quality); //Изменение качества

        Screen.fullScreen = Resources.isFullscreen; //Включение или отключение полноэкранного режима

        Screen.SetResolution(Screen.resolutions[Resources.currResolutionIndex].width,
                            Screen.resolutions[Resources.currResolutionIndex].height,
                            Resources.isFullscreen); //Изменения разрешения

        audioMixer.SetFloat("MasterVolume", Resources.volume); //Изменение уровня громкости
    }
}
