using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableRayInteractor : MonoBehaviour
{
    public XRDirectInteractor[] Interactors;
    public GameObject rayInteractor;

    private void Update()
    {
        bool somethingGrabbed = false;

        foreach (XRDirectInteractor interactor in Interactors)
        {
            somethingGrabbed = interactor.hasSelection;

            if (somethingGrabbed)
                break;
        }

        if (somethingGrabbed)
        {
            if (rayInteractor.activeInHierarchy)
                rayInteractor.SetActive(false);
        }
        else
        {
            if (!rayInteractor.activeInHierarchy)
                rayInteractor.SetActive(true);
        }
    }
}