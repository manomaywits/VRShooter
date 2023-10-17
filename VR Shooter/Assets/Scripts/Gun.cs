using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using ColorUtility = UnityEngine.ColorUtility;

public class Gun : MonoBehaviour, IShootable
{
    public Action<bool> GunPickedAction;
    private int ammo;

    public int ammunition
    {
        get => ammo;
        set => ammo = value;
    }

    public GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Transform bulletSpawnTransform;
    [SerializeField] private int maxAmmo;
    List<GameObject> bulletPool = new List<GameObject>();
    
    private List<GameObject> muzzleFlashPool = new List<GameObject>();
    private int maxBulletPool = 50;
    private int maxMuzzleFlash = 10;
    private int currentBullet;
    private int currentFlash;
    private bool isShooting = false;
    private int count = -1;
    private float timer = 0;

    private XRGrabInteractable grabInteractable;
    [HideInInspector] public ActionBasedController controller;

    [SerializeField] public ActionBasedController[] controllers;

    [SerializeField] private AudioClip gunShotClip;

    [SerializeField] private bool isSilencedGun = false;

    [SerializeField] private GameObject gunCanvas;
    [SerializeField] private TextMeshProUGUI ammoTMP;

    private Vector3 canvasPos;
    // Start is called before the first frame update

    public enum GunType
    {
        Semi,
        Auto
    };

    public GunType guntype = GunType.Semi;

    void Start()
    {
        ammo = maxAmmo;
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
        UpdateAmmoUI();
        gunCanvas.SetActive(false);
        canvasPos = gunCanvas.transform.localPosition;

        #region ObjectPooling- Bullets & MuzzleFlash

        if (bulletPrefab == null)
        {
            Debug.LogError($"No Bullet Prefab for '{this.gameObject.name}'.Cannot shoot this weapon!");
            return;
        }

        currentBullet = 0;
        for (int i = 0; i < maxBulletPool; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(0, 1000, 0), quaternion.identity);
            bulletPool.Add(bullet);
        }

        foreach (var bullet in bulletPool)
        {
            bullet.SetActive(false);
        }

        currentFlash = 0;
        for (int i = 0; i < maxMuzzleFlash; i++)
        {
            GameObject flash = Instantiate(muzzleFlash, new Vector3(0, 1000, 0), quaternion.identity);
            muzzleFlashPool.Add(flash);
        }

        foreach (var muzzleFlash in muzzleFlashPool)
        {
            muzzleFlash.SetActive(false);
        }

        #endregion
    }

    private void Update()
    {
        if (isShooting && guntype == GunType.Auto)
        {
            if (timer == 0)
            {
                Shoot();
            }

            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                timer = 0;
            }
        }
    }

    private void OnDestroy()
    {
        #region Clear Pooled Objects

        foreach (var bullet in bulletPool)
        {
            Destroy(bullet);
        }

        bulletPool.Clear();

        foreach (var flash in muzzleFlashPool)
        {
            Destroy(flash);
        }

        muzzleFlashPool.Clear();

        #endregion
    }


    void AssignShootAction(bool value, float amount)
    {
        if (guntype == GunType.Semi)
        {
            if (value && amount > 0.5f && isShooting == false)
            {
                isShooting = true;
                Shoot();
            }
            else if (value == false)
                isShooting = false;
        }
        else
        {
            if (value)
            {
                if (amount > 0.5f)
                {
                    if (isShooting == false)
                    {
                        isShooting = true;
                    }
                }
                else
                {
                    isShooting = false;
                    timer = 0;
                }
            }
            else
            {
                isShooting = false;
                timer = 0;
            }
        }
    }

    public void Shoot()
    {
        if (bulletPrefab == null)
        {
            return;
        }

        if (ammo <= 0)
        {
            AudioManager.SFXManager.PlayEmptyShot(transform.position);
            return;
        }

        var bullet = bulletPool[currentBullet];
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        bullet.transform.position = bulletSpawnTransform.position;
        bullet.transform.rotation = bulletSpawnTransform.rotation;

        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward * 500);

        PlayShootAudio();

        StartCoroutine(DisableBullet(bullet));
        currentBullet++;
        currentBullet = currentBullet == maxBulletPool ? 0 : currentBullet;

        var muzzleFlash = muzzleFlashPool[currentFlash];
        muzzleFlash.transform.position = bulletSpawnTransform.position;
        muzzleFlash.transform.rotation = bulletSpawnTransform.rotation;
        muzzleFlash.SetActive(true);
        StartCoroutine(DisableMuzzleFlash(muzzleFlash));
        controller.SendHapticImpulse(0.9f, 0.2f);

        ammo--;
        UpdateAmmoUI();
    }

    void PlayShootAudio()
    {
        if (gunShotClip != null)
        {
            AudioManager.SFXManager.PlaySFX(gunShotClip, true, transform.position);
            Invoke(nameof(BulletShellSFX), 1f);
        }
        else
        {
            if (isSilencedGun)
                AudioManager.SFXManager.PlaySilencedGunShot(transform.position);
            else
                AudioManager.SFXManager.PlayGunShot(transform.position);
        }
    }

    void BulletShellSFX()
    {
        StartCoroutine(AudioManager.SFXManager.PlayBulletShellSFX(transform.position));
    }

    public void UpdateAmmoUI()
    {
        if (ammoTMP == null)
        {
            Debug.LogError($"Cant find TextMeshProGUI for {this.gameObject.name}");
            return;
        }

        if (ammo <= 10 && ammo > 5)
        {
            ammoTMP.color = Color.yellow;
        }
        else if (ammo <= 5)
        {
            ammoTMP.color = Color.red;
        }
        else
        {
            ammoTMP.color = Color.green;
        }

        ammoTMP.text = ammo.ToString();
    }

    IEnumerator DisableMuzzleFlash(GameObject muzzleFlash)
    {
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator DisableBullet(GameObject bullet)
    {
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(3f);
        bullet.SetActive(false);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        controller = args.interactor.GetComponent<ActionBasedController>();

        if (controller == controllers[0])
        {
            count = 0;
            Debug.Log("Left Controller grabbed: " + controller.name);
            gunCanvas.transform.localPosition =
                new Vector3(Mathf.Abs(canvasPos.x), gunCanvas.transform.localPosition.y, gunCanvas.transform.localPosition.z);
        }
        else
        {
            count = 1;
            Debug.Log("Right Controller grabbed: " + controller.name);
            gunCanvas.transform.localPosition = new Vector3(-1 * Mathf.Abs(canvasPos.x), gunCanvas.transform.localPosition.y,
                gunCanvas.transform.localPosition.z);
        }
        
        GunPickedAction?.Invoke(true);
        gunCanvas.SetActive(true);
        SetInputCallbacks(count);
    }


    private void OnSelectExited(SelectExitEventArgs args)
    {
        GunPickedAction?.Invoke(false);
        RemoveInputCallbacks(count);
        controller = null;
        Debug.Log("Controller released.");
        gunCanvas.SetActive(false);
    }


    void SetInputCallbacks(int value)
    {
        if (value == 0)
            InputManager.LeftTriggerButtonAction += AssignShootAction;
        else
            InputManager.RightTriggerButtonAction += AssignShootAction;
    }

    void RemoveInputCallbacks(int value)
    {
        if (value == 0)
            InputManager.LeftTriggerButtonAction -= AssignShootAction;
        else
            InputManager.RightTriggerButtonAction -= AssignShootAction;
    }
}