using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class PondController : MonoBehaviour
{
    //private: _ || private|internal static|thread static: s_ |t_ ||

    public static PondController instance;
    public static PondState PondState;
    public static List<CageController> CageList = new List<CageController>();
    public static List<CageController> BoughtCageList = new List<CageController>();
    public static List<FishController> FishInCageList = new List<FishController>();
    public Transform hideInteractionAnchor, showInteractionAnchor;
    public int dirtyTimeStampBySeconds = 300;

    private float _timeToDirty;
    private int _row = 4, _column = 8;

    [SerializeField]
    private CageController Cage;
    [SerializeField]
    private FishController Fish;

    void _MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Awake()
    {
        _MakeInstance();
        _LoadPondState();
    }

    void Start()
    {
        _GenerateCages();
        _timeToDirty = 0;
        showInteractionAnchor = GameObject.Find("ShowAnchor").transform;
    }

    void Update()
    {
        if (_timeToDirty < dirtyTimeStampBySeconds)
        {
            _timeToDirty += Time.deltaTime;
        }
        else
        {
            _DirtyTheCage();
            _timeToDirty = 0;
        }
    }

    void _DirtyTheCage()
    {
        List<CageController> temp = BoughtCageList.FindAll(x => x.DirtyState == false && x.FishId != 0);
        if (temp.Count != 0)
        {
            int index = UnityEngine.Random.Range(0, temp.Count);
            temp[index].DirtyState = true;
            temp[index].transform.GetChild(0).gameObject.SetActive(true);
            PondState.DirtyCageMatrix[temp[index].RowIndex, temp[index].ColumnIndex] = true;
        }
    }

    void _LoadPondState()
    {
        string json = PlayerPrefsManager.instance._GetCageState();
        if (json == null || json == "")
        {
            PondState = new PondState();
        }
        else
        {
            PondState = JsonConvert.DeserializeObject<PondState>(json);
        }
    }

    void _GenerateCages()
    {
        float cageW = gameObject.GetComponent<RectTransform>().rect.width / _column;
        float cageH = gameObject.GetComponent<RectTransform>().rect.height / _row;
        int cageWidth = Mathf.RoundToInt(cageW);
        int cageHeight = Mathf.RoundToInt(cageH);

        Cage.GetComponent<CageController>().Width = cageWidth;
        Cage.GetComponent<CageController>().Height = cageHeight;
        Cage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(cageWidth, cageHeight);

        int pondWidth = _column * cageWidth;
        int pondHeight = _row * cageHeight;

        for (int r = 0; r < _row; r++)
        {
            for (int c = 0; c < _column; c++)
            {
                Vector3 tilePos = _ConvertMatrixIndexToLocalPos(c, r, pondWidth, pondHeight, cageWidth, cageHeight);
                var objCage = Instantiate(Cage, tilePos / 40, Quaternion.identity, gameObject.transform);

                objCage.transform.GetComponent<Button>().onClick.AddListener(
                    delegate { GameplayManager.instance._InteractInCage(objCage); }
                    );

                objCage.RowIndex = r;
                objCage.ColumnIndex = c;

                objCage.BoughtState = PondState.BoughtCageMatrix[r, c];
                objCage.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(
                    delegate { GameplayManager.instance._ConfirmBuyCage(objCage); }
                    );
                objCage.transform.GetChild(1).gameObject.SetActive(!objCage.BoughtState);
                if (objCage.BoughtState)
                {
                    BoughtCageList.Add(objCage);
                }

                objCage.DirtyState = PondState.DirtyCageMatrix[r, c];
                objCage.transform.GetChild(0).gameObject.SetActive(objCage.DirtyState);

                objCage.FishId = PondState.FishInCageMatrix[r, c];
                if (objCage.BoughtState && objCage.FishId != 0)
                {
                    var objFish = Instantiate(Fish, objCage.transform);
                    objFish.Type = objCage.FishId;
                    objFish.transform.GetComponent<Image>().sprite = SpriteFishController.spritesDict[objFish.Type];
                    FishInCageList.Add(objFish);
                }

                objCage.name = objCage.GetComponent<CageController>().RowIndex.ToString() + " - " + objCage.GetComponent<CageController>().ColumnIndex.ToString();
                objCage.transform.localPosition = tilePos;
                CageList.Add(objCage);
            }
        }

        GameplayManager.instance.pnBuyCage.transform.SetAsLastSibling();
        GameplayManager.instance.pnInteractInCage.transform.SetAsLastSibling();
    }

    (int, int) _ConvertPositionToMatrixIndex(float x, float y, float pondWidth, float pondHeight, float cageWidth, float cageHeight)
    {
        (int, int) temp = (0, 0);
        temp.Item1 = (int)(((pondHeight - cageHeight) / 2 - y) / cageHeight);  //rowIndex
        temp.Item2 = (int)(((pondWidth - cageWidth) / 2 + x) / cageWidth); //columnIndex
        return temp;
    }

    Vector3 _ConvertMatrixIndexToLocalPos(int colIndex, int rowIndex, int pondWidth, int pondHeight, float cageWidth, float cageHeight)
    {
        return new Vector3((colIndex * cageWidth - (pondWidth - cageWidth) / 2), ((pondHeight - cageHeight) / 2 - rowIndex * cageHeight), 0);
    }
}
