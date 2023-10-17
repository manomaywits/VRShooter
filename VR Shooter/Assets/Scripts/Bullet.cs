using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 initalPoint;
    [SerializeField] private GameObject bulletholePrefab;
    private GameObject bullethole;
    [SerializeField] private AudioClip hitClip;

    private void Start()
    {
        initalPoint = transform.position;
        bullethole = Instantiate(bulletholePrefab);
        bullethole.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Hit = {other.collider.gameObject.name}");
        SetBulletHole(other);
        if (other.collider.gameObject.CompareTag("Enemy"))
        {
            other.collider.gameObject.GetComponent<Enemy>().Damage();
        }

        if (other.collider.gameObject.GetComponent<Target>()!=null)
        {
            other.collider.gameObject.GetComponent<Target>().TargetHit(this.transform.position);
        }
    }

    void SetBulletHole(Collision other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            other.gameObject.GetComponent<Enemy>().bulletHole = bullethole;
        }
        var currentPos = transform.position;
        Vector3 targetdir = currentPos - initalPoint;
        bullethole.transform.position = currentPos;
        bullethole.transform.forward = targetdir;
        bullethole.transform.position += bullethole.transform.forward * -0.05f;
        bullethole.transform.parent = other.transform;
        bullethole.SetActive(true);
        AudioManager.SFXManager.PlaySFX(hitClip, true, currentPos,129);
        Invoke(nameof(ResetBullethole), 5f);
        this.gameObject.SetActive(false);
    }

    private void ResetBullethole()
    {
        bullethole.transform.parent = null;
        bullethole.SetActive(false);
    }
}