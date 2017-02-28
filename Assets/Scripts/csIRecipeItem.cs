using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public interface csIRecipeItem  
{

    string Name { get; set; }
    int Amount { get; set; }
    int AmountMax { get; set; }
}
