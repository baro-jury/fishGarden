using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance;
    public AudioSource audioSource;
    public AudioSource musicSource;

    private const string LEVEL = "Level";
    private const string EXP = "Experience";
    private const string COINS = "Coins";
    private const string CAGE_STATE = "Cage state in pond";

    void _MakeSingleInstance()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void _CheckFirstTimePlayGame()
    {
        if (!PlayerPrefs.HasKey("_CheckFirstTimePlayGame"))
        {
            PlayerPrefs.SetInt(LEVEL, 1);
            PlayerPrefs.SetInt(EXP, 0);
            PlayerPrefs.SetInt(COINS, 0);
            PlayerPrefs.SetString(CAGE_STATE, "");
            PlayerPrefs.SetInt("_CheckFirstTimePlayGame", 0);
        }
    }

    void Awake()
    {
        _MakeSingleInstance();
        _CheckFirstTimePlayGame();
        //_SetCoinsInPossession(9999, true);
        _SetCurrentLevel(5);
    }

    public int _GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(LEVEL);
    }

    public void _SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(LEVEL, level);
    }

    public int _GetCurrentExp()
    {
        return PlayerPrefs.GetInt(EXP);
    }

    public void _SetCurrentExp(int exp)
    {
        PlayerPrefs.SetInt(EXP, exp);
    }

    public int _GetCoinsInPossession()
    {
        return PlayerPrefs.GetInt(COINS);
    }

    public void _SetCoinsInPossession(int coin, bool isEarning)
    {
        int temp = -1;
        if (isEarning) temp = 1;
        if (PlayerPrefs.GetInt(COINS) + temp * coin > 0)
            PlayerPrefs.SetInt(COINS, PlayerPrefs.GetInt(COINS) + temp * coin);
        else
            PlayerPrefs.SetInt(COINS, 0);
    }

    public string _GetCageState()
    {
        return PlayerPrefs.GetString(CAGE_STATE);
    }

    public void _SetCageState(string cageState)
    {
        PlayerPrefs.SetString(CAGE_STATE, cageState);
    }

}
