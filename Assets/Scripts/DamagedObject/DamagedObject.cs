using System.Collections;
using UnityEngine;
using static Assets.Scripts.Types;

namespace Assets.Scripts.DamagedObject
{
    public abstract class DamagedObject : MonoBehaviour
    {
        [SerializeField]
        public float Health = 100;

        [SerializeField]
        public float MaxHealth = 100;

        [SerializeField]
        public Team team = Team.Enemy;

        [SerializeField]
        public float Armor = 0;

        public virtual void ApplyDamage(float damage)
        {
            Health -= damage * (1 - Armor / 100) * Random.Range(0.8f, 1.2f);
        }

        public virtual void Start()
        {
            
        }
    }
}
