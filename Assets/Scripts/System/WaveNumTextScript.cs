using UnityEngine;
using UnityEngine.UI;

public class WaveNumTextScript : MonoBehaviour
{

    [SerializeField]
    public WaveManager waveManager;

    [SerializeField]
    public Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = waveManager.waveCount.ToString();
    }
}
