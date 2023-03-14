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
    public Transform pnBuyCage, pnInteractInCage, pnInventoryShop, UIInvetoryShop;
    public Transform hideBoardAnchor, showBoardAnchor;
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
        //txtLevel.text = "Lv.";
        //txtCoin.text = "";
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
                PlayerPrefsManager.instance.audioSource.PlayOneShot(closeClip);
                _EndInteractionInCage();
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
        if(cage.FishId != 0)
        {

        }
    }

    void _CleanCage(CageController cage)
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        if (cage.DirtyState)
        {
            cage.transform.GetChild(0).gameObject.SetActive(false);
            cage.DirtyState = false;
            PondController.PondState.DirtyCageMatrix[cage.RowIndex, cage.ColumnIndex] = false;
            _GetExp(10);
        }
        _EndInteractionInCage();
    }

    void _EndInteractionInCage()
    {
        pnInteractInCage.GetChild(0).GetComponent<RectTransform>().DOMove(
            PondController.instance.hideInteractionAnchor.position, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                pnInteractInCage.gameObject.SetActive(false); //panel
            });
    }

    #endregion

    #region Inventory&Shop
    void _ShowTheBoard(int childIndex)
    {
        UIInvetoryShop.GetChild(childIndex).GetChild(0).gameObject.SetActive(true);

        pnInventoryShop.gameObject.SetActive(true);
        pnInventoryShop.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        pnInventoryShop.transform.GetComponent<Button>().onClick.AddListener(
            delegate
            {
                PlayerPrefsManager.instance.audioSource.PlayOneShot(closeClip);
                _HideTheBoard(childIndex);
            }
        );
        pnInventoryShop.GetChild(childIndex).gameObject.SetActive(true);
        pnInventoryShop.GetChild(childIndex).GetComponent<RectTransform>()
            .DOMove(showBoardAnchor.position, .25f).SetEase(Ease.InOutQuad).SetUpdate(true);
    }

    void _HideTheBoard(int childIndex)
    {
        UIInvetoryShop.GetChild(childIndex).GetChild(0).gameObject.SetActive(false);
        pnInventoryShop.GetChild(childIndex).GetComponent<RectTransform>()
            .DOMove(hideBoardAnchor.position, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                pnInventoryShop.GetChild(childIndex).gameObject.SetActive(false);
                pnInventoryShop.gameObject.SetActive(false); //panel
            });
    }

    public void _ShowInventory()
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        _ShowTheBoard(0);
    }

    public void _ShowFishShop()
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        _ShowTheBoard(1);
    }

    public void _ShowFoodShop()
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        _ShowTheBoard(2);
    }

    public void _ShowDecorationShop()
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(clickClip);
        _ShowTheBoard(3);
    }

    #endregion


    public void _GetExp(float exp)
    {
        ExpController.instance.expEarned = exp;
        ExpController.instance._EarnExp();
    }
}
