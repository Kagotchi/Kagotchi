using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class csMagicPower: csIAttack 
{
    private float rawPower = 0;
    private float Max { get; set; }

    public float Mana { get; set; }
    public string Name { get; set; }
    public float Base { get; set; }
    public float Value { get; set; }
    public float Factor { get; set; }
    public float ManaBase { get; set; }
    public float CasterLevel { get; set; }
    public int BaseStatus { get; set; }
    public int Rarity { get; set; }
    public List<csEffect> Effects { get; set; }
    public List<csMagicPower> CombinedPowers { get; set; }
    public csCorePower Core { get; set; }
    public bool IsStrongAgainst { get; set; }
    public int WeaknessMultiplier { get; set; }
    public int WeaknessFactor { get; set; }
    public int Level { get; set; }
    public float Focus { get; set; }
    public csAttack AttackType { get; set; }
    public float StaminaBase { get; set; }
    public float Stamina { get; set; }
    public float AttackValue { get; set; }

    public float RawPower
    {
        get { return Max * Value; }
        set { rawPower = value; }
    }

    public csMagicPower()
    {
    }

    public void Init()
    {
        Max = CasterLevel * Base;
        Mana = ManaBase + (ManaBase * (CasterLevel - 1.0f) * 0.25f);
        if(Value == 0)
            Value = Factor;
        WeaknessFactor = 2;
        WeaknessMultiplier = 1;
        RawPower = Max * Value;
    }

    public float CastSpell()
    {
        float extraValue = 0.0f;
        var rand = UnityEngine.Random.Range(0 ,Mathf.RoundToInt((BaseStatus * 0.1f)));
        var power = (Max * Value) + rand;

        if(IsStrongAgainst)
            extraValue = power * 0.5f * WeaknessMultiplier * WeaknessFactor;

        if (power + extraValue < 1.0f)
            AttackValue = 1.0f;
        else
            AttackValue = power + extraValue;

        return AttackValue;
    }

    public bool Increase()
    {
        var raiseProbability = 100.0f - (Value * 100.0f);
        var random = UnityEngine.Random.Range(0.0f, 100.0f);

        if(random <= raiseProbability)
        {
            Value += Factor;

            if (Value > 1.0f)
                Value = 1.0f;

            return true;
        }

        return false;
    }

    public float Attack()
    {
        return 0;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
