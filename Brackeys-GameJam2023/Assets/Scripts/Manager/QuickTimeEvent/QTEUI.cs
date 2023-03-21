using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class QTEUI : StaticInstances<QTEUI>
{
    private Image keyImage;
    private Image fillImage;
    [SerializeField] private Sprite WKey;
    [SerializeField] private Sprite SKey;
    [SerializeField] private Sprite AKey;
    [SerializeField] private Sprite DKey;
    [SerializeField] private Sprite M0Key;
    [SerializeField] private Sprite M1Key;
    protected override void Awake()
    {

        base.Awake();
        keyImage = transform.GetChild(0).GetComponent<Image>();
        fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        keyImage.enabled = false;
    }

    public void ShowKey(KeyCode K)
    {
        keyImage.enabled = true;
        switch (K)
        {
            case KeyCode.W:
                keyImage.sprite = WKey;
                break;
            case KeyCode.S:
                keyImage.sprite = SKey;
                break;
            case KeyCode.A:
                keyImage.sprite = AKey;
                break;
            case KeyCode.D:
                keyImage.sprite = DKey;
                break;
            case KeyCode.Mouse0:
                keyImage.sprite = M0Key;
                break;
            case KeyCode.Mouse1: 
                keyImage.sprite = M1Key;
                break;
        }
    }
    public void DisableImage()
    {
        keyImage.enabled = false;
    }

    private void Update()
    {
        if(keyImage.enabled) 
        {
            fillImage.fillAmount = QuickTimeEvent.Instance.successMeter;
        }
        else
        {
            fillImage.fillAmount = 0;
        }
    }
}
