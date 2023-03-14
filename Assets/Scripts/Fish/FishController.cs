using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public int Type { get; set; }
    public string Name { get; set; }
    public int TimesEatToGrowUp { get; set; }

    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public bool HungryState { get; set; }
    public int CurrentStack { get; set; }
    public int CurrentValue { get; set; }

    public FishController()
    {

    }

}
