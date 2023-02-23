using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public AudioClip clickClip, closeClip;
    public Sprite cleanCage, dirtyCage;
    public GameObject pnBuyCage;
    public Text txtLevel, txtCoin;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        txtLevel.text = "Lv." + PlayerPrefsManager.instance._GetCurrentLevel();
        txtCoin.text = PlayerPrefsManager.instance._GetCoinsInPossession() + "";
        pnBuyCage.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(
            delegate {
                PlayerPrefsManager.instance.audioSource.PlayOneShot(closeClip);
                pnBuyCage.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad)
                .SetUpdate(true).OnComplete(() =>
                    {
                        pnBuyCage.SetActive(false); //panel
                    });
            }
            );
    }

    public void _SavePondState()
    {
        string json = JsonConvert.SerializeObject(PondController.PondState);
        File.Delete(Application.dataPath + "/Resources/cageStateInPond.json");
        File.WriteAllText(Application.dataPath + "/Resources/cageStateInPond.json", json);
        Debug.Log("Game saved");
    }

    private void OnApplicationQuit()
    {
        _SavePondState();
    }

    public void _ConfirmBuyCage(CageController cage)
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        pnBuyCage.SetActive(true);
        pnBuyCage.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one, .25f)
            .SetEase(Ease.InOutQuad).SetUpdate(true);

        pnBuyCage.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        pnBuyCage.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick
            .AddListener(delegate { _BuyCage(cage); });
    }

    void _BuyCage(CageController cage)
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        if (PlayerPrefsManager.instance._GetCoinsInPossession() >= 100)
        {
            PlayerPrefsManager.instance._SetCoinsInPossession(100, false);
            txtCoin.text = PlayerPrefsManager.instance._GetCoinsInPossession() + "";
            pnBuyCage.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                pnBuyCage.SetActive(false); //panel
                cage.transform.GetChild(1).gameObject.SetActive(false);
                cage.BoughtState = true;
                PondController.PondState.BoughtCageMatrix[cage.RowIndex, cage.ColumnIndex] = true;
                _SavePondState();
            });
        }
    }

    public void _Decor()
    {
        
    }
}
