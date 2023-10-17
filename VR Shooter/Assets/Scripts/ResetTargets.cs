using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetTargets : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;
    private Dictionary<GameObject, Vector3> dict = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        foreach (var target in targets)
        {
            dict.Add(target, target.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var kvpair in dict)
        {
            kvpair.Key.transform.position = kvpair.Value;
        }
    }
}