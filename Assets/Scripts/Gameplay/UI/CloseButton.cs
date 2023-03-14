using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void _CloseInteraction()
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(closeClip);
        transform.parent.GetComponent<RectTransform>().DOMove(
            PondController.instance.hideInteractionAnchor.position, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                transform.parent.parent.gameObject.SetActive(false); //panel
            });
    }

    public void _CloseBoard() 
    {
        PlayerPrefsManager.instance.audioSource.PlayOneShot(closeClip);
        transform.parent.GetComponent<RectTransform>().DOMove(
            GameplayManager.instance.hideBoardAnchor.position, .25f).SetEase(Ease.InOutQuad).SetUpdate(true) //gameObject: form
            .OnComplete(() =>
            {
                transform.parent.parent.gameObject.SetActive(false); //panel
            });
    }
}
