using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface csIAttack
{
    string Name { get; set; }
    int BaseStatus { get; set; }
    float CasterLevel { get; set; }
    float Value { get; set; }
    float Factor { get; set; }
    float Focus { get; set; }
    csAttack AttackType { get; set; }
    float StaminaBase { get; set; }
    float Stamina { get; set; }
    int Level { get; set; }
    float Mana { get; set; }
    float ManaBase { get; set; }
    int Rarity { get; set; }
    List<csEffect> Effects { get; set; }
    List<csMagicPower> CombinedPowers { get; set; }
    csCorePower Core { get; set; }
    bool IsStrongAgainst { get; set; }
    int WeaknessMultiplier { get; set; }
    int WeaknessFactor { get; set; }
    float RawPower { get; set; }
    float AttackValue { get; set; }

    void Init();

    float Attack();

    float CastSpell();

    bool Increase();

    object Clone();
}
