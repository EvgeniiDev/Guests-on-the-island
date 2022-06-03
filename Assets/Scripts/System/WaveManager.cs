using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    public Spawner[] spawners;

    [SerializeField]
    private int WaveTimePeriod;

    [SerializeField]
    public int FirstSpawnDelayInSeconds;

    [SerializeField]
    public float amountOfUnits;

    public float unitAttackDelay;
    public int waveCount = 0;

    public void Start()
    {
        Resources.Timer = FirstSpawnDelayInSeconds;
        StartCoroutine(LoopWave());
    }

    public IEnumerator LoopWave()
    {
        yield return new WaitForSeconds(FirstSpawnDelayInSeconds);

        while (true)
        {
            waveCount++;
            amountOfUnits += 0.15f;
            Resources.Timer = WaveTimePeriod;

            StartWave();
            yield return new WaitForSeconds(WaveTimePeriod);
        }
    }

    public void StartWave()
    {
        Debug.Log(amountOfUnits);
        for (int i = 0; i < amountOfUnits/2; i++)
            foreach (var spawner in spawners)
                if(spawner!=null)
                    spawner.Spawn(waveCount * 10, waveCount*1.5f);
    }
}
