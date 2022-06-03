using Assets.Scripts.DamagedObject;
using Assets.Scripts.UI;
using Assets.Scripts.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    [SerializeField]
    private Text textInp;

    [SerializeField]
    private string name = string.Empty;

    [SerializeField]
    private string[] text = new string[0];

    [SerializeField]
    private float price;

    [SerializeField]
    private float HPBoost;

    [SerializeField]
    private float DamageBoost;

    [SerializeField]
    private float ArmorBoost;

    [SerializeField]
    private float SpeedBoost;

    [SerializeField]
    private float SpeedAttackBoost;

    [SerializeField]
    private float DistanceBoost;

    [SerializeField]
    private float ExperienceBoost;

    private static GameObject MainHero;


    private void Start()
    {
        SetText();
        //StartCoroutine(FindMainHero());
    }

    private static void GetMainHeroScript()
    {
        var mainHeroScript = Resources.AllObjects.Where(x => x != null)
                                           .Select(x => x.GetComponent<MainCharacterUnit>())
                                           .Where(x => x != null)
                                           .ToList();


        var tttt = mainHeroScript.FirstOrDefault();

        if (tttt != null)
            MainHero = tttt.gameObject;
    }

    public IEnumerator FindMainHero()
    {
        while (true)
        {
            GetMainHeroScript();
            yield return new WaitForSeconds(1);
        }
    }

    void SetText()
    {
        //извини за это, бро
        textInp.text =
                        name + Environment.NewLine +
                        string.Join(Environment.NewLine, text) + Environment.NewLine +
                        (HPBoost != 0 ? $"{(HPBoost > 0 ? "+" : "")} {HPBoost} к хп" + Environment.NewLine : string.Empty) +
                        (DamageBoost != 0 ? $"{(DamageBoost > 0 ? "+" : "")} {DamageBoost} к дамагу" + Environment.NewLine : string.Empty) +
                        (ArmorBoost != 0 ? $"{(ArmorBoost > 0 ? "+" : "")} {ArmorBoost} к армору" + Environment.NewLine : string.Empty) +
                        (SpeedBoost != 0 ? $"{(SpeedBoost > 0 ? "+" : "")} {SpeedBoost} к скорости передвижения" + Environment.NewLine : string.Empty) +
                        (SpeedAttackBoost != 0 ? $"{(SpeedAttackBoost > 0 ? "+" : "")} {SpeedAttackBoost}% к скорости атаки" + Environment.NewLine : string.Empty) +
                        (DistanceBoost != 0 ? $"{(DistanceBoost > 0 ? "+" : "")} {DistanceBoost} к дальности атаки" + Environment.NewLine : string.Empty) +
                        (ExperienceBoost != 0 ? $"{(ExperienceBoost > 0 ? "+" : "")} {ExperienceBoost} к опыту" + Environment.NewLine : string.Empty) +
                        $"Цена {price}";
    }

    IEnumerator SetTextCor()
    {
        yield return new WaitForSeconds(2);
        SetText();
    }

    public void Clicked()
    {
        GetMainHeroScript();
        if (Resources.MainHeroGold < price)
        {
            textInp.text = "No money" + Environment.NewLine + "No honey!";
        }
        else if (MainHero == null)
        {
            textInp.text = "Вы мертвы(";
        }
        else
        {
            Resources.MainHeroGold -= price;
            price = (int)(price*1.05f);

            var dO = MainHero.GetComponent<PlayerDamagedObject>();
            dO.Health += HPBoost;
            dO.MaxHealth += HPBoost;
            Resources.MainHeroHP = dO.MaxHealth;
            dO.Armor += ArmorBoost;
            Resources.MainHeroArmor = dO.Armor;

            var pC = MainHero.GetComponent<MainCharacterUnit>();
            pC.Damage += DamageBoost;
            Resources.MainHeroDamage = pC.Damage;
            pC.AttackRadius += DistanceBoost;
            Resources.MainHeroDistance = pC.AttackRadius;
            pC.ShootDelay *= 1 - SpeedAttackBoost / 100;
            Resources.MainHeroSpeedAttack = pC.ShootDelay;
            Resources.MainHeroExperience += ExperienceBoost + price / 10;

            var nma = MainHero.GetComponent<NavMeshAgent>();
            nma.speed += SpeedBoost;
            Resources.MainHeroSpeed = nma.speed;
        }
        StartCoroutine(SetTextCor());
    }
}
