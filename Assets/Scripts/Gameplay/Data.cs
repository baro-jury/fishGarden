using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PondState
{
    //private List<List<bool>> boughtCageList = new List<List<bool>>
    //{
    //    new List<bool>{ true, false, false, false, false, false, false, false },
    //    new List<bool>{ true, false, false, false, false, false, false, false },
    //    new List<bool>{ false, false, false, false, false, false, false, false },
    //    new List<bool>{ false, false, false, false, false, false, false, false }
    //};

    //private List<List<int>> fishInCageList = new List<List<int>>
    //{
    //    new List<int>{1, 0, 0, 0, 0, 0, 0, 0},
    //    new List<int>{0, 0, 0, 0, 0, 0, 0, 0},
    //    new List<int>{0, 0, 0, 0, 0, 0, 0, 0},
    //    new List<int>{0, 0, 0, 0, 0, 0, 0, 0}
    //};

    private bool[,] boughtCageMatrix =
        {
            {true, false, false, false, false, false, false ,false},
            {true, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false}
        };
    private int[,] fishInCageMatrix =
        {
            {1, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

    //public List<List<bool>> BoughtCageList { get { return boughtCageList; } set { boughtCageList = value; } }
    //public List<List<int>> FishInCageList { get { return fishInCageList; } set { fishInCageList = value; } }

    public bool[,] BoughtCageMatrix { get { return boughtCageMatrix; } set { boughtCageMatrix = value; } }
    public int[,] FishInCageMatrix { get { return fishInCageMatrix; } set { fishInCageMatrix = value; } }

    public PondState()
    {

    }

    //public PondState(List<List<bool>> boughtCageList, List<List<int>> fishInCageList)
    //{
    //    BoughtCageList = boughtCageList;
    //    FishInCageList = fishInCageList;
    //}

    public PondState(bool[,] boughtCageMatrix, int[,] fishInCageMatrix)
    {
        BoughtCageMatrix = boughtCageMatrix;
        FishInCageMatrix = fishInCageMatrix;
    }


}

