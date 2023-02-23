using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip closeClip;

    //close in game
    public void _Close()
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(closeClip);
        transform.parent.GetComponent<RectTransform>().DOScale(Vector3.zero, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                transform.parent.parent.gameObject.SetActive(false); //panel
                transform.parent.localScale = Vector3.one;
            });
    }
}
