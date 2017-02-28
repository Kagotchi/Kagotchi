using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface csIDefense
{
    string Name { get; set; }
    float Value { get; set; }
    float Factor { get; set; }
    csAttack PhysicalDefenseType { get; set; }
    csCorePower MagicalDefenseType { get; set; }
    int Level { get; set; }

    void Increase();
}
