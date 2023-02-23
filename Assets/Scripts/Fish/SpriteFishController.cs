using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteFishController : MonoBehaviour
{
    public static SpriteFishController instance;
    public static Dictionary<int, Sprite> spritesDict = new Dictionary<int, Sprite>();

    public List<Sprite> Fishes;
    public Sprite blank;

    private int idElement = 1;

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
        _CreateDictionary(Fishes);
    }

    void _CreateDictionary(List<Sprite> list)
    {
        Dictionary<int, Sprite> temp = new Dictionary<int, Sprite>();
        temp.Add(0, blank);
        foreach (Sprite sprite in list)
        {
            temp.Add(idElement, sprite);
            idElement++;
        }
        spritesDict = temp;
        idElement = 1;
    }

}
