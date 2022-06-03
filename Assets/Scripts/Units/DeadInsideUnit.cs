using Assets.Scripts.DamagedObject;
using Assets.Scripts.Units;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DeadInsideUnit : Unit
{
    public override void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Team = GetComponent<DamagedObject>().team;
        Anime = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        StartCoroutine(TestCoroutine());
    }

    public IEnumerator TestCoroutine()
    {
        while (true)
        {
            UpdateFighting();

            yield return new WaitForSeconds(1);
        }
    }
    public override void Update()
    {
        ShootTime -= Time.deltaTime;
        if (Random.Range(0,1) >0.975 )
            _audio.Play();
    }
}
