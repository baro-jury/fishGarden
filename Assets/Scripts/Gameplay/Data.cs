using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PondState
{
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

    public bool[,] BoughtCageMatrix { get { return boughtCageMatrix; } set { boughtCageMatrix = value; } }
    public int[,] FishInCageMatrix { get { return fishInCageMatrix; } set { fishInCageMatrix = value; } }

    public PondState()
    {

    }

    public PondState(bool[,] boughtCageMatrix, int[,] fishInCageMatrix)
    {
        BoughtCageMatrix = boughtCageMatrix;
        FishInCageMatrix = fishInCageMatrix;
    }
}

