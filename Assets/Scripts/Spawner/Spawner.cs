using Assets.Scripts.DamagedObject;
using Assets.Scripts.Units;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Assets.Scripts.Types;

public class Spawner : MonoBehaviour
{
    public GameObject[] objsToSpawn;

    [SerializeField]
    private float _spawnDelay;

    [SerializeField]
    private Team team;


    void Awake()
    {
        StartCoroutine(LoopWave());
        StartCoroutine(MainHeroRespawner());
    }

    public IEnumerator MainHeroRespawner()
    {
        while (true)
        {
            if (team == Team.Friend && Resources.DeadTime <= 0)
            {
                Resources.DeadTime = int.MaxValue;
                SpawnMainHero();
            }
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator LoopWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);
            Spawn();
        }
    }

    private void SpawnMainHero()
    {
        (float x, float z) = GetRandomLocation();

        var position = transform.position + new Vector3(x, 0, z);
        GameObject obj = null;

        if (Resources.MainHeroPrefab != null)
        {
            obj = Instantiate(Resources.MainHeroPrefab, position, Quaternion.identity);

            var damagedObj = obj.GetComponent<DamagedObject>();
            Resources.AllObjects.Add(damagedObj);

            if (Resources.MainHeroExperience != 0)
            {
                var dO = obj.GetComponent<PlayerDamagedObject>();
                dO.Health = Resources.MainHeroHP;
                dO.MaxHealth = Resources.MainHeroHP;
                dO.Armor = Resources.MainHeroArmor;

                var pC = obj.GetComponent<MainCharacterUnit>();
                pC.Damage = Resources.MainHeroDamage;
                pC.AttackRadius = Resources.MainHeroDistance;
                pC.ShootDelay = Resources.MainHeroSpeedAttack;

                obj.GetComponent<NavMeshAgent>().speed = Resources.MainHeroSpeed;
            }
            else
            {
                Resources.MainHeroHP = damagedObj.Health;
            }
        }
    }

    public void Spawn(float health = default, float damage = default)
    {
        var obj = objsToSpawn[Random.Range(0, objsToSpawn.Length - 1)];

        (float x, float z) = GetRandomLocation();

        var position = transform.position + new Vector3(x, 0, z);

        if (team == Team.Enemy)
            UnitCounter.CurrentUnitCount++;

        Create(obj, position, health, damage);
    }
    
    private (float, float) GetRandomLocation()
    {
        int max = 30;
        int min = 10;

        float x;
        float z;

        if (Random.value > 0.5)
            x = Random.Range(-max, -min);
        else
            x = Random.Range(min, max);

        if (Random.value > 0.5)
            z = Random.Range(-max, -min);
        else
            z = Random.Range(min, max);

        return (x, z);
    }

    private void Create(GameObject e, Vector3 pos, float health = default, float damage = default)
    {
        var obj = Instantiate(e, pos, Quaternion.identity);
        var unit = obj.GetComponent<Unit>();
        var damagedObj = obj.GetComponent<DamagedObject>();

        Resources.AllObjects.Add(damagedObj);

        if (team == Team.Friend)
        {
            health += Resources.MainHeroExperience / 10 + Resources.FriendlyUnitHpBoost;
            damage += (Resources.MainHeroExperience / 100) + Resources.FriendlyUnitDamageBoost;
        }
        if (unit != null)
        {
            unit.status = Status.Fighting;

            if (damage != default)
            {
                unit.Damage += damage;
            }
        }

        if (health != default)
        {
            damagedObj.Health += health;
            damagedObj.MaxHealth += health;
        }
    }
}
