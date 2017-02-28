using UnityEngine;
using System.Collections;

public class csPhysicalDefense : csIDefense
{
    public string Name { get; set; }
    private float Max { get; set; }
    public float Value { get; set; }
    public float Factor { get; set; }
    public csAttack PhysicalDefenseType { get; set; }
    public csCorePower MagicalDefenseType { get; set; }
    public int Level { get; set; }

    public csPhysicalDefense()
    {
        Max = 0.95f;
    }

    public void Increase()
    {
        var raiseProbability = 100.0f - Value;
        var random = Random.Range(0.0f, 100.0f);

        if (random <= raiseProbability)
            Value += Factor;

        if (Value > Max)
            Value = Max;
    }
}
