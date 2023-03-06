using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpController : MonoBehaviour
{
    public static ExpController instance;

    [SerializeField]
    private Slider expSlider;
    [HideInInspector]
    public bool earningExp = false;
    public float expEarned = 10;

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

    void Start()
    {
        earningExp = false;
        expSlider.value = PlayerPrefsManager.instance._GetCurrentExp();
    }

    void Update()
    {
        //if (earningExp)
        //{
        //    if (expSlider.value < PlayerPrefsManager.instance._GetCurrentExp())
        //    {
        //        //expSlider.value += 7 * Time.deltaTime;
        //        expSlider.value += 20 * Time.deltaTime;
        //    }
        //    if (expSlider.value >= PlayerPrefsManager.instance._GetCurrentExp())
        //    {
        //        expSlider.value = Mathf.Floor(expSlider.value);
        //    }
        //    if (expSlider.value == PlayerPrefsManager.instance._GetCurrentExp())
        //    {
        //        earningExp = false;
        //    }
        //}

        if (earningExp)
        {
            if (expSlider.value < PlayerPrefsManager.instance._GetCurrentExp())
            {
                //expSlider.value += 7 * Time.deltaTime;
                expSlider.value += 20 * Time.deltaTime;
            }
            else if (expSlider.value > PlayerPrefsManager.instance._GetCurrentExp())
            {
                expSlider.value = Mathf.Floor(expSlider.value);
            }
            else
            {
                earningExp = false;
            }
        }

        if (expSlider.value == expSlider.maxValue)
        {
            _LevelUp();
        }

    }

    public void _EarnExp()
    {
        PlayerPrefsManager.instance._SetCurrentExp(PlayerPrefsManager.instance._GetCurrentExp() + expEarned);
        earningExp = true;
    }

    void _LevelUp()
    {
        if (PlayerPrefsManager.instance._GetCurrentExp() >= 100)
        {
            PlayerPrefsManager.instance._SetCurrentExp(PlayerPrefsManager.instance._GetCurrentExp() % 100);
        }
        PlayerPrefsManager.instance._SetCurrentLevel(PlayerPrefsManager.instance._GetCurrentLevel() + 1);
        GameplayManager.instance.txtLevel.text = "Lv." + PlayerPrefsManager.instance._GetCurrentLevel();
        expSlider.value = expSlider.minValue;
        earningExp = true;
    }
}
