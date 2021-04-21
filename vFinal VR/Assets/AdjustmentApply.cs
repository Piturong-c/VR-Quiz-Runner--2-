using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdjustmentApply : MonoBehaviour
{    
    public Button save;
    public Button _default;
    public Button cancel;
    public GameObject SAVE_SUCCESS;
    
    public void Awake()
    {
        SAVE_SUCCESS = GameObject.Find("SAVE_SUCCESS");
        save.onClick.AddListener(() => Serialize());
        _default.onClick.AddListener(() => SetDefault());
        cancel.onClick.AddListener(() => GoBackToPlayState());
    }

    public void Serialize()
    {
        AdjustmentUI[] adjusts = FindObjectsOfType<AdjustmentUI>();
        foreach (AdjustmentUI adjust in adjusts)
            adjust.Serialize();

        Popup("<color=green>Save settings successfully!</color>");
    }

    public void Popup(string message)
    {
        SAVE_SUCCESS.transform.GetChild(0).GetComponent<TMP_Text>().text = message;
        SAVE_SUCCESS.transform.DOScale(Vector3.one, .25f).SetEase(Ease.InOutCubic);
        SAVE_SUCCESS.transform.DOScale(Vector3.zero, .25f).SetDelay(1f).SetEase(Ease.InOutCubic);
    }

    public void SetDefault()
    {
        AdjustmentUI[] adjusts = FindObjectsOfType<AdjustmentUI>();
        foreach (AdjustmentUI adjust in adjusts)
            adjust.SetDefault();
        Popup("<color=orange>Already set all setting to default!</color>");
    }

    public void GoBackToPlayState()
    {
        GameObject.Find("UI_SETTING").transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutCubic);
        FindObjectOfType<MainMenu>().state = MainMenu.CameraState.PLAY;
    }
}
