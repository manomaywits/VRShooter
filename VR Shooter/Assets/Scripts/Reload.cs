using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Reload : MonoBehaviour
{
    private GameObject currentMag;
    [SerializeField] private Transform firstPont;
    [SerializeField] private Transform secondPoint;

    [SerializeField] private Gun gunscript;

    [SerializeField]
    private AudioClip reloadSFX;

    [SerializeField] private TextMeshProUGUI magtxt;

    // Start is called before the first frame update
    void Start()
    {
        if (gunscript == null)
        {
            gunscript = GetComponentInParent<Gun>();
        }

        gunscript.GunPickedAction += GunPickListner;
        CheckForGunMag();
    }

    void CheckForGunMag()
    {
        if(currentMag==null)
            magtxt.gameObject.SetActive(true);
        else
        {
            magtxt.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        RemoveInputListner();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Mag"))
        {
            if (currentMag == null && gunscript.controller!=null && other.GetComponent<XRGrabInteractable>().selectingInteractor!=null)
                LoadMagzine(other.gameObject);
        }
    }

    void LoadMagzine(GameObject mag)
    {
        this.GetComponent<Collider>().enabled = false;
        currentMag = mag;
        currentMag.GetComponent<MagazineAmmo>().EnableUI(false);
        currentMag.GetComponent<XRGrabInteractable>().enabled = false;
        currentMag.transform.parent = this.gameObject.transform;
        currentMag.GetComponent<Collider>().enabled = false;
        currentMag.GetComponent<Rigidbody>().isKinematic = true;
        currentMag.transform.DOLocalMove(firstPont.transform.localPosition, 0.5f, false);
        StartCoroutine(LoadAnim(0.5f));
    }

    IEnumerator LoadAnim(float time)
    {
        yield return new WaitForSeconds(time);
        currentMag.transform.DOLocalMove(secondPoint.transform.localPosition, 0.5f, false);
        AudioManager.SFXManager.PlaySFX(reloadSFX,true,this.transform.position);
        yield return new WaitForSeconds(0.5f);
        gunscript.ammunition += currentMag.GetComponent<MagazineAmmo>().MagAmmo;
        gunscript.UpdateAmmoUI();
        currentMag.SetActive(false);
        CheckForGunMag();
    }

    void UnloadMagzine(float value)
    {
        if (currentMag == null)
            return;

        if (value == 0)
            return;
        
        currentMag.SetActive(true);
        currentMag.transform.DOLocalMove(firstPont.transform.localPosition, 0.5f, false);
        currentMag.GetComponent<MagazineAmmo>().MagAmmo = gunscript.ammunition;
        currentMag.GetComponent<MagazineAmmo>().UpdateMagUI();
        gunscript.ammunition = 0;
        gunscript.UpdateAmmoUI();
        StartCoroutine(Unload(0.5f));
    }

    IEnumerator Unload(float time)
    {
        yield return new WaitForSeconds(time);
        currentMag.transform.parent = null;
        currentMag.GetComponent<Collider>().enabled = true;
        currentMag.GetComponent<Rigidbody>().isKinematic = false;
        currentMag.GetComponent<XRGrabInteractable>().enabled = true;
        currentMag.GetComponent<MagazineAmmo>().EnableUI(true);
        currentMag = null;
        CheckForGunMag();
        yield return new WaitForSeconds(2f);
        this.GetComponent<Collider>().enabled = true;
    }

    void SetInputListner()
    {
        if (gunscript.controller == gunscript.controllers[0])
        {
            InputManager.LeftXbuttonAction += UnloadMagzine;
        }
        else
        {
            InputManager.RightXbuttonAction += UnloadMagzine;
        }
    }

    void RemoveInputListner()
    {
        if (gunscript.controller == gunscript.controllers[0])
        {
            InputManager.LeftXbuttonAction -= UnloadMagzine;
        }
        else
        {
            InputManager.RightXbuttonAction -= UnloadMagzine;
        }
    }

    void GunPickListner(bool value)
    {
        if (value)
        { 
            SetInputListner();
        }
        else
        {
            RemoveInputListner();
        }
    }
}