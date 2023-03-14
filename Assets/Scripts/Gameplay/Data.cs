using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondState
{
    private bool[,] boughtCageMatrix =
        {
            {true, false, false, false, false, false, false ,false},
            {true, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false}
        };
    private bool[,] dirtyCageMatrix =
        {
            {true, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false}
        };
    private int[,] fishInCageMatrix =
        {
            {1, 0, 0, 0, 0, 0, 0, 0},
            {2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };
    private bool[,] hungryFishMatrix =
        {
            {true, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false},
            {false, false, false, false, false, false, false ,false}
        };

    public bool[,] BoughtCageMatrix { get { return boughtCageMatrix; } set { boughtCageMatrix = value; } }
    public bool[,] DirtyCageMatrix { get { return dirtyCageMatrix; } set { dirtyCageMatrix = value; } }
    public int[,] FishInCageMatrix { get { return fishInCageMatrix; } set { fishInCageMatrix = value; } }

    public PondState()
    {

    }

    public PondState(bool[,] boughtCageMatrix, bool[,] dirtyCageMatrix, int[,] fishInCageMatrix)
    {
        BoughtCageMatrix = boughtCageMatrix;
        DirtyCageMatrix = dirtyCageMatrix;
        FishInCageMatrix = fishInCageMatrix;
    }

}

public class FishData
{
    private List<Fish> fishList = new List<Fish>
    {
        new Fish(1, "Butterflyfish", 100, 2),
        new Fish(2, "Goldfish", 150, 3),
        new Fish(3, "Kingfish", 200, 4),
        new Fish(4, "Tuna", 250, 5)
    };
    //mỗi lần cho ăn sẽ tăng giá trị thêm 70 coins

    public List<Fish> FishList { get { return fishList; } set { fishList = value; } }

    public FishData()
    {

    }
}

public class Fish
{
    public int Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PriceBuy { get; set; }
    public int TimesEatToGrowUp { get; set; }
    public int PriceSellMin { get; set; }
    public int PriceSellMax { get; set; }

    public Fish()
    {

    }

    public Fish(int type, string name, int priceBuy, int timesEatToGrowUp)
    {
        Type = type;
        Name = name;
        Description = "Fish will mature after " + timesEatToGrowUp + " times of feeding";
        PriceBuy = priceBuy;
        TimesEatToGrowUp = timesEatToGrowUp;
        PriceSellMin = priceBuy / 10;
        PriceSellMax = priceBuy / 2 * 3;
    }
}

