using Assets.Scripts.DamagedObject;
using System.Collections;
using UnityEngine;

public class PlayerDamagedObject : DamagedObject
{
    public override void Start()
    {
        StartCoroutine(AutoHealth());
    }

    public IEnumerator AutoHealth()
    {
        while (true)
        {
            if (Health < MaxHealth)
                Health += MaxHealth * 0.005f;
            yield return new WaitForSeconds(1);
        }
    }
}