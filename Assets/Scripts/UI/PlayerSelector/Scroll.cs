using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using Assets.Scripts.Units;

public class Scroll : MonoBehaviour
{
    [Header("Select amount of your objects")]
    [Range(1, 100)]
    public int amount;
    [Header("Select smooth speed")]
    [Range(0.05f, 0.5f)]
    public float smoothSpeed;
    [SerializeField]
    private Text paramsText;

    [Header("Select distance between objects")]
    [Range(5, 80)]
    public int distance;

    [Header("Select names for your objects")]
    public string[] names;
    public GameObject[] obj;
    private GameObject[] instatiatedObj;
    private Vector2[] points;
    public GameObject parentScroll;
    public Text characterName;
    private float smoothedX, smoothedScale;
    private Vector3[] defaultScale, bigScale;

    public int currentNum = 0;
    public Text Description;
    public string[] Descriptions;

    public Text DPS;
    public Text MovementsSpeed;
    public Text Health;
    public Text Armor;

    void Start()
    {
        instatiatedObj = new GameObject[amount];
        points = new Vector2[amount + 1];
        defaultScale = new Vector3[amount];
        bigScale = new Vector3[amount];
        for (int i = 0; i < amount; i++)
        {
            if (i == 0)
            {
                instatiatedObj[i] = Instantiate(obj[i], new Vector3(0, parentScroll.transform.position.y, 0), Quaternion.identity);
            }
            else
            {
                instatiatedObj[i] = Instantiate(obj[i], new Vector3(instatiatedObj[i - 1].transform.position.x + distance,
                  instatiatedObj[i - 1].transform.position.y, 0), Quaternion.identity);
            }

            instatiatedObj[i].transform.parent = parentScroll.transform;
            defaultScale[i] = new Vector3(instatiatedObj[i].transform.localScale.x - 25, instatiatedObj[i].transform.localScale.y - 25, 0);
            bigScale[i] = new Vector3(instatiatedObj[i].transform.localScale.x + 10, instatiatedObj[i].transform.localScale.y + 10, 0);

            var nma = instatiatedObj[i].GetComponents<MonoBehaviour>();
            foreach (var script in nma)
                if (script != null)
                    script.enabled = false;
        }
        for (int y = 0; y < amount + 1; y++)
        {
            if (y == 0)
                points[y] = new Vector2(parentScroll.transform.position.x + distance / 2, parentScroll.transform.position.y);
            else
                points[y] = new Vector2(points[y - 1].x - distance, parentScroll.transform.position.y);
        }
        StartCoroutine(Rotate());
        PrintParameters();
    }

    public void Next()
    {
        if (currentNum == amount - 1)
            return;
        currentNum = Math.Abs((currentNum + 1) % amount);
        smoothedX = points[currentNum].x - distance / 5;
        parentScroll.transform.position = new Vector2(smoothedX, parentScroll.transform.position.y);
        PrintParameters();
    }

    public void Previous()
    {
        if (currentNum == 0)
            return;
        currentNum = Math.Abs((currentNum - 1) % amount);
        smoothedX = points[currentNum].x - distance / 5;
        parentScroll.transform.position = new Vector2(smoothedX, parentScroll.transform.position.y);
        PrintParameters();
    }

    private void PrintParameters()
    {
        var currentUnit = obj[currentNum];

        var cUDO = currentUnit.GetComponent<PlayerDamagedObject>();
        var armor = cUDO.Armor;
        var health = cUDO.MaxHealth;

        var cMCU = currentUnit.GetComponent<MainCharacterUnit>();
        var damage = cMCU.Damage;
        //var attackRadius = cMCU.AttackRadius;
        var shootDelay = cMCU.ShootDelay;
        var nma = currentUnit.GetComponent<NavMeshAgent>();
        var movementSpeed = nma.speed;

        DPS.text = ((int)(damage * (1 / shootDelay))).ToString();
        Health.text = ((int)health).ToString();
        Armor.text = ((int)armor).ToString();
        MovementsSpeed.text = ((int)movementSpeed).ToString();

        Description.text = Descriptions[currentNum];
    }

    public IEnumerator Rotate()
    {
        while (true)
        {
            for (int i = 0; i < amount; i++)
                instatiatedObj[i].transform.Rotate(0, 1, 0);
            yield return new WaitForSeconds(0.005f);
        }
    }
    void Update()
    {
        for (int i = 0; i < amount; i++)
        {
            if (parentScroll.transform.position.x < points[i].x && parentScroll.transform.position.x > points[i + 1].x)
            {
                smoothedX = Mathf.SmoothStep(parentScroll.transform.position.x, points[i].x - distance / 2, smoothSpeed);
                smoothedScale = Mathf.SmoothStep(bigScale[i].x, defaultScale[i].x, smoothSpeed);
                //Select(i);
                currentNum = i;
                characterName.text = names[i];
                PrintParameters();
            }
            else
            {
                smoothedScale = Mathf.SmoothStep(defaultScale[i].x, bigScale[i].x, smoothSpeed);
            }
            instatiatedObj[i].transform.localScale = new Vector3(smoothedScale, smoothedScale, smoothedScale);
        }
        parentScroll.transform.position = new Vector2(smoothedX, parentScroll.transform.position.y);
    }
}
