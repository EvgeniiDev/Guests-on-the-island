using Assets.Scripts.DamagedObject;
using Assets.Scripts.Units;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerUiPrefab;

    [SerializeField]
    private GameObject healthPrefab;

    [SerializeField]
    public DamagedObject _damaged;

    public void Start()
    {
        if (playerUiPrefab != null)
        {
            var health = Instantiate(healthPrefab);
            health.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }

    public void Update()
    {
        if (_damaged.Health <= 0f)
        {
            var obj = gameObject.GetComponent<DamagedObject>();

            if (obj.team == Assets.Scripts.Types.Team.Wood)
            {
                Resources.MainHeroGold += (int)obj.MaxHealth / 10;
            }
            else if (obj.team == Assets.Scripts.Types.Team.Enemy)
            {
                Resources.MainHeroExperience += obj.MaxHealth / 2;
                Resources.MainHeroGold += obj.MaxHealth / 15;
            }
            else if (obj.team == Assets.Scripts.Types.Team.Friend)
            {
                var pObj = obj.GetComponent<MainCharacterUnit>();
                if (pObj != null)
                {
                    Resources.DeathCount++;
                    Resources.DeadTime = 15 + Resources.DeathCount;
                }
            }

            if (obj.name.Contains("BaracEnemy"))
                Resources.BaracEnemyCount -= 1;

            if (obj.name.Contains("BaracFriend"))
                Resources.BaracFriendCount -= 1;

            Resources.AllObjects.Remove(obj);
            DestroyImmediate(gameObject);
        }
    }
}