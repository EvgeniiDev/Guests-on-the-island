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

    public AudioMixer audioMixer; //��������� ���������
    public Dropdown resolutionDropdown;

    public Texture[] Images;

    private void Start()
    {
        settings.gameObject.SetActive(false);
        Resources.audioMixer = audioMixer;
        Resources.resolutionDropdown = resolutionDropdown;
        resolutionDropdown.ClearOptions(); //�������� ������ �������
        Resources.resolutions = Screen.resolutions; //��������� ��������� ����������
        var options = new List<string>(); //�������� ������ �� ���������� ����������

        for (int i = 0; i < Resources.resolutions.Length; i++) //���������� ������ � ������ �����������
        {
            string option = Resources.resolutions[i].width + " x " + Resources.resolutions[i].height; //�������� ������ ��� ������
            options.Add(option); //���������� ������ � ������

            if (Resources.resolutions[i].Equals(Screen.currentResolution)) //���� ������� ���������� ����� ������������
                Resources.currResolutionIndex = i; //�� ���������� ��� ������
        }

        resolutionDropdown.AddOptions(options); //���������� ��������� � ���������� ������
        resolutionDropdown.value = Resources.currResolutionIndex; //��������� ������ � ������� �����������
        resolutionDropdown.RefreshShownValue(); //���������� ������������� ��������
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

    public void ChangeVolume(float val) //��������� �����
    {
        Resources.volume = val;
    }

    public void ChangeResolution(int index) //��������� ����������
    {
        Resources.currResolutionIndex = index;
    }

    public void ChangeFullscreenMode(bool val) //��������� ��� ���������� �������������� ������
    {
        Resources.isFullscreen = val;
    }

    public void ChangeMiniMapState(bool val) //��������� ��� ���������� �������������� ������
    {
        Resources.MiniMapState = val;
    }

    public void ChangeFPSState(bool val) //��������� ��� ���������� �������������� ������
    {
        Resources.FPSState = val;
    }

    public void ChangeQuality(int index) //��������� ��������
    {
        Resources.quality = index;
    }

    public void SaveSettings()
    {
        QualitySettings.SetQualityLevel(Resources.quality); //��������� ��������

        Screen.fullScreen = Resources.isFullscreen; //��������� ��� ���������� �������������� ������

        Screen.SetResolution(Screen.resolutions[Resources.currResolutionIndex].width,
                            Screen.resolutions[Resources.currResolutionIndex].height,
                            Resources.isFullscreen); //��������� ����������

        audioMixer.SetFloat("MasterVolume", Resources.volume); //��������� ������ ���������
    }
}
