using UnityEngine;
using System.Collections;

public class csFightPlanElement 
{
    public csPhysicalPower PhysicalPower { get; set; }
    public csPhysicalDefense PhysicalDefense { get; set; }
    public csMagicPower MagicalPower { get; set; }
    public csMagicDefense MagicalDefense { get; set; }
    public string AttackType { get; set; }
    public int Turn { get; set; }

}
