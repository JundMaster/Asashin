﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject newGameOption;

    [SerializeField]
    private GameObject continueOption;

    [SerializeField]
    private GameObject quitOption;

    private CameraController vmController;
    // Start is called before the first frame update
    void Awake()
    {
        vmController = FindObjectOfType<CameraController>();
    }

    public void SwitchMenuOption()
    {
        if (vmController.IsNewGameCamActive)
        {
            newGameOption.SetActive(true);
            continueOption.SetActive(false);
            quitOption.SetActive(false);
        }

        else if (vmController.IsContinueCamActive)
        {
            newGameOption.SetActive(false);
            continueOption.SetActive(true);
            quitOption.SetActive(false);
        }

        else
        {
            newGameOption.SetActive(false);
            continueOption.SetActive(false);
            quitOption.SetActive(true);
        }
    }
}
