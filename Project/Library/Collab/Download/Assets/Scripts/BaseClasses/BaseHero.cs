using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseHero : BaseClass
{
    public int health;
    public int dexterity;
    public int damage;
    public int defense;


    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public int Dexterity
    {
        get
        {
            return dexterity;
        }

        set
        {
            dexterity = value;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    public int Defense
    {
        get
        {
            return defense;
        }

        set
        {
            defense = value;
        }
    }
		

    public List<BaseAttack> magicAttacks = new List<BaseAttack>();

    public BaseHero()
    {
        baseHP = 200 + health * 18;
        curHP = baseHP;
        coolDownTime = 64f / (dexterity + 8);
        baseATK = 10 + damage * 2;
        baseDEF = 1 + defense * 0.2f;
        curDEF = baseDEF;
    }

    public void UpdateStats()
    {
        baseHP = 200 + health * 18;
        curHP = baseHP;
        coolDownTime = 64f / (dexterity + 8);
        baseATK = 10 + damage * 2;
        baseDEF = 1 + defense * 0.2f;
        curDEF = baseDEF;
    }
}
