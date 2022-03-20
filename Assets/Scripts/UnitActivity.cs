using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitActivity : MonoBehaviour
{
    public float hpMax;
    public float hpCurrent;
    public float attackRange;
    public float attackValue;
    public float attackSpeed;
    public float viewingRange;
    public float defenceValue;
    public string attackType; // normal, pierce, magic, chaos, siege, hero, spells
    public string defenseType; // unarmored, light, medium, heavy, fortress, divine, hero

    public float recievedDamage;

    public Animator unitAnimator;
    public Transform enemyCastle; // указываем переменную типа transform, для задания позиции Замка
    public Transform target;
    public string enemyFaction;
    public GameObject enemyUnit; // переменная типа gameObject, для определения вражеского юнита как объекта

    private NavMeshAgent UnitAgent;
    private float targerDistance = 0f;
    private float lastAttackTime = 0f;
    private float damagePercent;
    


    void Start()
    {
        enemyFaction = gameObject.tag == "East" ? "West" : "East"; // получаем тэг вражеских юнитов
        UnitAgent = GetComponent<NavMeshAgent>();
        target = enemyCastle;

    }

    void Update()
    {
        if (hpCurrent <= 0)
        {
            unitDie();
        }
        enemyUnit = GameObject.FindWithTag(enemyFaction); //ищем на сцене юнитов с вражеским тэгом

        target = enemyUnit == null ? enemyCastle.transform : enemyUnit.transform; // на сцене есть враг?

        targerDistance = Vector3.Distance(target.position, transform.position); // какое расстояние до цели?

        if (targerDistance > attackRange) // сокращаем дистанцию
        {
            UnitAgent.enabled = true;
            UnitAgent.SetDestination(target.position);
            unitAnimator.SetBool("running", true);
            unitAnimator.SetBool("attacking", false);
        }
        else // если можем, то атакуем
        {
            UnitAgent.enabled = false;
            doAttack(enemyUnit);
        }
    }

   void doAttack(GameObject attackTarget)
    {
        UnitActivity enemy = attackTarget.GetComponent<UnitActivity>();
        if (lastAttackTime >= 100 / attackSpeed)
        {
            enemy.getDamage(attackValue, attackType);
            lastAttackTime = 0;
        }
        unitAnimator.SetBool("attacking", true);
        lastAttackTime++;
    }

    void getDamage(float damage, string damageType)
    {
        float damageReduction = (0.06f * this.defenceValue) / (1 + 0.06f * this.defenceValue);
        float recievedDamage = damage * (1 - damageReduction) * AttackToDefenseType(damageType, this.defenseType);
        hpCurrent = hpCurrent - recievedDamage;
    }

    void unitDie()
    {
        Destroy(gameObject);
    }

    public float AttackToDefenseType(string damageType, string defenseType) // подбор процента от чисторого урона в зависимости от типа атаки и брони
    {
        switch (damageType)
        {
            case "normal":
                switch (defenseType)
                {
                    case "unarmored":
                        damagePercent = 1.05f;
                        break;

                    case "light":
                        damagePercent = 0.7f;
                        break;

                    case "medium":
                        damagePercent = 1.75f;
                        break;

                    case "heavy":
                        damagePercent = 1;
                        break;

                    case "fortified":
                        damagePercent = 0.5f;
                        break;

                    case "divine":
                        damagePercent = 0.25f;
                        break;

                    case "hero":
                        damagePercent = 0.6f;
                        break;
                }
                break;

            case "pierce":
                switch (defenseType)
                {
                    case "unarmored":
                        damagePercent = 1.05f;
                        break;

                    case "light":
                        damagePercent = 1.75f;
                        break;

                    case "medium":
                        damagePercent = 1;
                        break;

                    case "heavy":
                        damagePercent = 0.7f;
                        break;

                    case "fortified":
                        damagePercent = 0.45f;
                        break;

                    case "divine":
                        damagePercent = 0.25f;
                        break;

                    case "hero":
                        damagePercent = 0.6f;
                        break;
                }
                break;

            case "magic":
                switch (defenseType)
                {
                    case "unarmored":
                        damagePercent = 1.05f;
                        break;

                    case "light":
                        damagePercent = 1;
                        break;

                    case "medium":
                        damagePercent = 0.7f;
                        break;

                    case "heavy":
                        damagePercent = 1.75f;
                        break;

                    case "fortified":
                        damagePercent = 0.4f;
                        break;

                    case "divine":
                        damagePercent = 0.25f;
                        break;

                    case "hero":
                        damagePercent = 0.6f;
                        break;
                }
                break;

            case "chaos":
                damagePercent = 1;
                break;

            case "siege":
                switch (defenseType)
                {
                    case "unarmored":
                        damagePercent = 1;
                        break;

                    case "light":
                        damagePercent = 0.7f;
                        break;

                    case"medium":
                        damagePercent = 0.7f;
                        break;

                    case "heavy":
                        damagePercent = 0.7f;
                        break;

                    case "fortified":
                        damagePercent = 1.75f;
                        break;

                    case "divine":
                        damagePercent = 0.2f;
                        break;

                    case "hero":
                        damagePercent = 0.4f;
                        break;
                }
                break;

            case "hero":
                switch (defenseType)
                {
                    case"unarmored":
                        damagePercent = 1.1f;
                        break;

                    case "light":
                        damagePercent = 1.1f;
                        break;

                    case "medium":
                        damagePercent = 1.1f;
                        break;

                    case "heavy":
                        damagePercent = 1.1f;
                        break;

                    case "fortified":
                        damagePercent = 0.6f;
                        break;

                    case "divine":
                        damagePercent = 0.4f;
                        break;

                    case "hero":
                        damagePercent = 0.6f;
                        break;
                }
                break;
        }
        return (damagePercent);
    }
}
