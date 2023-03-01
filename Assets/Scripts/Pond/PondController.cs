using DG.Tweening;
using Newtonsoft.Json;
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
    public static List<Transform> CageList = new List<Transform>();
    public static List<CageController> BoughtCageList = new List<CageController>();
    public static float dirtyTimeStamp = 0;

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

    // Start is called before the first frame update
    void Start()
    {

        _GenerateCages();
    }
    private void Load()
    {
        string arrayString = PlayerPrefs.GetString("Array2D");
        int[,] loadedArray = JsonUtility.FromJson<int[,]>(arrayString);
        // Do something with the loaded array...
    }
    void _LoadPondState()
    {
        //string json = (Resources.Load("cageStateInPond") as TextAsset).text;
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
                objCage.RowIndex = r;
                objCage.ColumnIndex = c;

                objCage.BoughtState = PondState.BoughtCageMatrix[r, c];
                //objCage.BoughtState = PondState.BoughtCageList[r][c];
                objCage.transform.GetChild(1).gameObject.SetActive(!objCage.BoughtState);
                objCage.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(
                    delegate { GameplayManager.instance._ConfirmBuyCage(objCage); }
                    );
                if (objCage.BoughtState) BoughtCageList.Add(objCage);

                objCage.FishId = PondState.FishInCageMatrix[r, c];
                //objCage.FishId = PondState.FishInCageList[r][c];
                if (objCage.BoughtState && objCage.FishId != 0)
                {
                    var objFish = Instantiate(Fish, objCage.transform);
                    objFish.Id = objCage.FishId;
                    objFish.transform.GetComponent<Image>().sprite = SpriteFishController.spritesDict[objFish.Id];
                }

                objCage.name = objCage.GetComponent<CageController>().RowIndex.ToString() + " - " + objCage.GetComponent<CageController>().ColumnIndex.ToString();
                objCage.transform.localPosition = tilePos;
                CageList.Add(objCage.transform);
            }
        }

        GameplayManager.instance.pnBuyCage.transform.SetAsLastSibling();
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
