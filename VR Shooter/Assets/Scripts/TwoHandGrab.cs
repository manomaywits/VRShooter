using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrab : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondhandgrab = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;

    public enum TwoHandRotationType
    {
        None,
        First,
        Second
    };

    public TwoHandRotationType twoHandRotationType;
    public bool snapToSecondHand = true;
    private Quaternion initialRotationOffset;

    private void Start()
    {
        foreach (var item in secondhandgrab)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        secondInteractor = interactor;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandedRotation()) * selectingInteractor.attachTransform.rotation;
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        secondInteractor = null;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        attachInitialRotation = args.interactor.attachTransform.localRotation;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        secondInteractor = null;
        args.interactor.attachTransform.localRotation = attachInitialRotation;
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }


    private Quaternion GetTwoHandedRotation()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position,
                selectingInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position,
                secondInteractor.attachTransform.up);
        }

        return targetRotation;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor)
        {
            //compute
            if (snapToSecondHand)
                selectingInteractor.attachTransform.rotation = GetTwoHandedRotation();
            else
                selectingInteractor.attachTransform.rotation = GetTwoHandedRotation() * initialRotationOffset;
        }

        base.ProcessInteractable(updatePhase);
    }
}