using UnityEngine;
using UnityEngine.Audio;

public class SoundLevel : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        audioMixer.SetFloat("MasterVolume", Resources.volume); //Изменение уровня громкости
    }
}
