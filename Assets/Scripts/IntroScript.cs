using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject panel;
    private bool end = false;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (end)
            {
                SceneManager.LoadScene("Launcher");
            }
            else
            {
                panel.SetActive(true);
                end = true;
                videoPlayer.Stop();
            }
        }
    }

    void EndReached(VideoPlayer vp)
    {
        panel.SetActive(true);
        end = true;
    }
}
