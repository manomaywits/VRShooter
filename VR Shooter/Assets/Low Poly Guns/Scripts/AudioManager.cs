using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip silencedGunshotSFX;
    [SerializeField] private AudioClip gunshotSFX;
    [SerializeField] private AudioClip bulletShellSFX;
    [SerializeField] private AudioClip emptyShotSFX;
    [SerializeField] private AudioClip ambience;

    public static AudioManager SFXManager;

    public void Awake()
    {
        if (SFXManager == null)
            SFXManager = this;
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        PlayAmbienceMusic();
    }

    void PlayAmbienceMusic()
    {
        var src = GetComponent<AudioSource>();
        if (src == null)
            src = gameObject.AddComponent<AudioSource>();

        src.clip = ambience;
        src.loop = true;
        src.spatialBlend = 0;
        src.Play();
    }

    public void PlaySFX(AudioClip clip, bool isAudio3D, Vector3 pos = default(Vector3), int priority = 128)
    {
        GameObject soundPrefab = Resources.Load<GameObject>("SFX");
        if (soundPrefab == null)
        {
            Debug.LogError("Cannot find SFX Gameobject in Resources");
            return;
        }

        if (soundPrefab.GetComponent<AudioSource>() == null)
        {
            Debug.LogError("Cannot find AudioSource on SFX Gameobject");
            return;
        }

        GameObject sfxobj;

        if (isAudio3D)
        {
            sfxobj = Instantiate(soundPrefab, pos, Quaternion.identity);
            sfxobj.GetComponent<AudioSource>().spatialBlend = 1f;
        }
        else
        {
            sfxobj = Instantiate(soundPrefab);
            sfxobj.GetComponent<AudioSource>().spatialBlend = 0f;
        }

        sfxobj.GetComponent<AudioSource>().priority = priority;
        sfxobj.GetComponent<AudioSource>().PlayOneShot(clip);

        StartCoroutine(RemoveSFXObj(sfxobj, clip));
    }

    public void PlaySilencedGunShot(Vector3 pos)
    {
        PlaySFX(silencedGunshotSFX, true, pos);
        StartCoroutine(PlayBulletShellSFX(pos));
    }

    public void PlayGunShot(Vector3 pos)
    {
        PlaySFX(gunshotSFX, true, pos);
        StartCoroutine(PlayBulletShellSFX(pos));
    }

    public void PlayEmptyShot(Vector3 pos)
    {
        PlaySFX(emptyShotSFX,true,pos);
    }

    public IEnumerator PlayBulletShellSFX (Vector3 pos = default(Vector3))
    {
        yield return new WaitForSeconds(1f);
        PlaySFX(bulletShellSFX, true, pos);
    }

    IEnumerator RemoveSFXObj(GameObject sfx, AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);

        Destroy(sfx);
    }
}