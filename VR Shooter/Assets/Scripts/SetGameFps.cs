using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.XR.Oculus;
using UnityEngine.XR;

public class SetGameFps : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpro;
    [SerializeField] private int targetFramesPerSecond = 90;
    private void Awake()
    {
        if(Application.isEditor)
            return;
    
        Performance.TrySetDisplayRefreshRate(targetFramesPerSecond);
        XRSettings.eyeTextureResolutionScale = 1.5f;
    }
    
    public float updateInterval = 0.5f; // How frequently to update the FPS display
    private float accumulatedFPS = 0f;
    private int frames = 0;
    private float timeLeft;

    private void Start()
    {
        timeLeft = updateInterval;

    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        accumulatedFPS += Time.timeScale / Time.deltaTime;
        frames++;

        // Calculate and update FPS every updateInterval
        if (timeLeft <= 0.0)
        {
            float avgFPS = accumulatedFPS / frames;
            tmpro.text = $"FPS: {avgFPS:F2}";

            timeLeft = updateInterval;
            accumulatedFPS = 0f;
            frames = 0;
        }
    }
    

}
