using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageController : MonoBehaviour
{
    public (int, int) Id { get; set; }
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool BoughtState { get; set; }
    public int FishId { get; set; }

    public CageController()
    {

    }

    public CageController(int rowIndex, int columnIndex, int height, int width, bool boughtState, int fishId)
    {
        Id = (rowIndex, columnIndex);
        RowIndex = rowIndex;
        ColumnIndex = columnIndex;
        Height = height;
        Width = width;
        BoughtState = boughtState;
        FishId = fishId;
    }
}
