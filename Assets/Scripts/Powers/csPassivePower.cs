using UnityEngine;
using System.Collections;

public class csPassivePower 
{
    public string Name { get; set; }
    private float Max { get; set; }
    public int BaseStatus { get; set; }
    public float Value { get; set; }
    public float Factor { get; set; }
    public float CasterLevel { get; set; }
    public bool IsInUse { get; set; }


    public csPassivePower()
    {
    }

    public void Init()
    {
        Max = CasterLevel * BaseStatus * 0.1f;
    }

    public float Use()
    {
        return (Max * Value);
    }

    public bool Increase()
    {
        var raiseProbability = 100.0f - (Value * 100.0f);
        var random = Random.Range(0.0f, 100.0f);

        if (random <= raiseProbability)
        {
            Value += Factor;

            if (Value > 1.0f)
                Value = 1.0f;

            return true;
        }
        else
            return false;
    }
}
