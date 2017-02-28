using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class csPhysicalPower : csIAttack
{
    private float rawPower = 0;
    public string Name { get; set; }
    private float Max { get; set; }
    public int BaseStatus { get; set; }
    public float CasterLevel { get; set; }
    public float Value { get; set; }
    public float Factor { get; set; }
    public float Focus { get; set; }
    public csAttack AttackType { get; set; }
    public float StaminaBase { get; set; }
    public float Stamina { get; set; }
    public int Level { get; set; }
    public float Mana { get; set; }
    public float ManaBase { get; set; }
    public int Rarity { get; set; }
    public List<csEffect> Effects { get; set; }
    public List<csMagicPower> CombinedPowers { get; set; }
    public csCorePower Core { get; set; }
    public bool IsStrongAgainst { get; set; }
    public int WeaknessMultiplier { get; set; }
    public int WeaknessFactor { get; set; }
    public float AttackValue { get; set; }

    public float RawPower
    {
        get { return Max * Value; }
        set { rawPower = value; }
    }


    public csPhysicalPower()
    {
    }

    public void Init()
    {
        Max = CasterLevel * BaseStatus * 0.1f;
        Stamina = StaminaBase + (StaminaBase * (CasterLevel - 1.0f) * 0.25f);
        Value = Factor;
        RawPower = Max * Value;
    }

    public float Attack()
    {
        var rand = UnityEngine.Random.Range(0, Mathf.FloorToInt(Focus));
        if ((Max * Value) + rand < 1.0f)
            AttackValue = 1.0f;
        else
            AttackValue = (Max * Value) + rand;

        return AttackValue;
    }

    public bool Increase()
    {
        var raiseProbability = 100.0f - (Value * 100.0f);
        var random = UnityEngine.Random.Range(0.0f, 100.0f);

        if (random <= raiseProbability)
        {
            Value += Factor;

            if (Value > 1.0f)
                Value = 1.0f;

            return true;
        }

        return false;
            
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public float CastSpell()
    {
        return 0;
    }
}
