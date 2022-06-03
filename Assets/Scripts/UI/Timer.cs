using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Text _timeText;

    [SerializeField]
    public AudioClip[] clips;

    private AudioSource _audio;

    private float _step=0;
    private float _value=0;
    private float _start;
    private float _end;
    private bool isFirst = true;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        StartCoroutine(UpdateTime());
    }

    public void Update()
    {
        _value += _step * Time.deltaTime;
        _audio.volume = Mathf.Lerp(_start, _end, _value);

        if (_audio.time > 5 && isFirst)
        {
            _start = 1f;
            _end = 0;
            _value = 0f;
            isFirst = false;
        }
    }

    public IEnumerator UpdateTime()
    {
        while (true)
        {
            SetTime();

            Resources.DeadTime -= 1;

            yield return new WaitForSeconds(1);
        }
    }

    private void SetTime()
    {
        var _timePassed = Resources.Timer;

        float minutes = _timePassed / 60;
        int seconds = _timePassed % 60;

        var minutesString = $"{(minutes < 10 ? "0" : "")}{minutes}";
        var secondsString = $"{(seconds < 10 ? "0" : "")}{seconds}";

        _timeText.text = minutesString + ":" + secondsString;

        Resources.Timer -= 1;

        if (seconds == 5)
        {
            var clip = clips[Random.Range(0, clips.Length - 1)];
            _audio.clip = clip;
            
            _start = 0; 
            _end = 1f;   
            _value = 0f;
            _step = 1f / 5;
            
            _audio.volume = _step;
            _audio.Play();
            isFirst = true;
        }
    }
}
