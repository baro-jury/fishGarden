using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public AudioClip clickClip, closeClip;
    public Transform pnBuyCage, pnInteractInCage;
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
        Screen.SetResolution(1600, 900, false);
    }

    void Start()
    {
        txtLevel.text = "Lv." + PlayerPrefsManager.instance._GetCurrentLevel();
        txtCoin.text = PlayerPrefsManager.instance._GetCoinsInPossession() + "";
        _BindingAction();
    }
    void _BindingAction()
    {
        pnBuyCage.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener( //btNo
            delegate
            {
                PlayerPrefsManager.instance.audioSource.PlayOneShot(closeClip);
                pnBuyCage.transform.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad)
                .SetUpdate(true).OnComplete(() =>
                {
                    pnBuyCage.gameObject.SetActive(false); //panel
                });
            }
            );
        pnInteractInCage.transform.GetComponent<Button>().onClick.AddListener(
            delegate
            {
                Debug.Log("pn click close");
                PlayerPrefsManager.instance.audioSource.PlayOneShot(closeClip);
                _EndInteraction();
            }
            );
    }

    
    private void OnApplicationQuit()
    {
        _SavePondState();
    }
    void _SavePondState()
    {
        string json = JsonConvert.SerializeObject(PondController.PondState);
        PlayerPrefsManager.instance._SetCageState(json);

        Debug.Log("Game saved");
    }

    #region Cage
    public void _ConfirmBuyCage(CageController cage)
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        pnBuyCage.gameObject.SetActive(true);
        pnBuyCage.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one, .25f)
            .SetEase(Ease.InOutQuad).SetUpdate(true);

        pnBuyCage.GetChild(0).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        pnBuyCage.GetChild(0).GetChild(2).GetComponent<Button>().onClick
            .AddListener(delegate { _BuyCage(cage); });
    }

    void _BuyCage(CageController cage)
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        if (PlayerPrefsManager.instance._GetCoinsInPossession() >= 100)
        {
            PlayerPrefsManager.instance._SetCoinsInPossession(100, false);
            txtCoin.text = PlayerPrefsManager.instance._GetCoinsInPossession() + "";
            pnBuyCage.GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                pnBuyCage.gameObject.SetActive(false);
                cage.transform.GetChild(1).gameObject.SetActive(false);
                cage.BoughtState = true;
                PondController.PondState.BoughtCageMatrix[cage.RowIndex, cage.ColumnIndex] = true;
                //PondController.PondState.BoughtCageList[cage.RowIndex][cage.ColumnIndex] = true;
            });
        }
    }

    public void _InteractInCage(CageController cage)
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        pnInteractInCage.gameObject.SetActive(true);
        pnInteractInCage.GetChild(0).GetComponent<RectTransform>().DOMove(
            PondController.instance.showInteractionAnchor.position, .25f).SetEase(Ease.InOutQuad).SetUpdate(true);

        pnInteractInCage.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        pnInteractInCage.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().onClick
            .AddListener(delegate { _SellFishInCage(cage); });
        pnInteractInCage.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        pnInteractInCage.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>().onClick
            .AddListener(delegate { _CleanCage(cage); });
    }

    void _SellFishInCage(CageController cage)
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
    }

    void _CleanCage(CageController cage)
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        cage.transform.GetChild(0).gameObject.SetActive(false);
        cage.DirtyState = false;
        PondController.PondState.DirtyCageMatrix[cage.RowIndex, cage.ColumnIndex] = false;
        _GetExp(10);
        _EndInteraction();
    }

    void _EndInteraction()
    {
        //pnInteractInCage.GetComponent<Image>().DOFade(0, .25f).SetEase(Ease.InOutQuad).SetUpdate(true);
        pnInteractInCage.GetChild(0).GetComponent<RectTransform>().DOMove(
            PondController.instance.hideInteractionAnchor.position, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                pnInteractInCage.gameObject.SetActive(false); //panel
            });
    }

    #endregion

    public void _GetExp(float exp)
    {
        ExpController.instance.expEarned = exp;
        ExpController.instance._EarnExp();
    }
}
