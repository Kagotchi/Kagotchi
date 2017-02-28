using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public interface csIFighter 
    {
        int Level { get; set; }
        int Strength { get; set; }
        int Dexterity { get; set; }
        int Intelligence { get; set; }
        int Hitpoints { get; set; }
        int Stamina { get; set; }
        int Mana { get; set; }
        string Name { get; set; }
        List<csWeakness> Weaknesses { get; set; }
        List<csWeakness> AllWeaknesses { get; set; }
        List<csIAttack> Powers { get; set; }
        List<csIAttack> AllPowers { get; set; }
        List<csIAttack> Attacks { get; set; }
        List<csPhysicalDefense> PhysicalDefense { get; set; }
        List<csPassivePower> PassivePower { get; set; }
        List<csMagicDefense> MagicalDefense { get; set; }
        List<csMagicDefense> AllMagicalDefenses { get; set; }
        List<csIAttack> CurrentCombat { get; set; }
    }
}
