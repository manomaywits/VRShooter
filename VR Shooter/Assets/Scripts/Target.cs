using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isActive = false;
    public float accuracyScale=1f;
    private float accuracy = 0f;

    public void TargetHit(Vector3 hitPosition = default)
    {
        if(!isActive)
            return;
        
        ScoreManager.Instance.IncreaseScore(1f);
        if (hitPosition != default)
        {
            CalculateAccuracy(hitPosition);
        }

        isActive = false;
       
        if (TargetManager.Instance.inSixtySecondMode)
        {
            TargetManager.Instance.ShowTargetWithDelay();
        }
        this.gameObject.transform.parent.gameObject.SetActive(false);
        
    }

  

    private void CalculateAccuracy(Vector3 hitposition)
    {
        float dist = Vector3.Distance(hitposition, transform.position);
            
        if (dist < 0.1 * accuracyScale)
        {
            accuracy = 100;
            
        }
        else if (dist >= 0.1* accuracyScale && dist < 0.2* accuracyScale)
        {
            accuracy = 75;
        }
        else if (dist >= 0.2* accuracyScale && dist < 0.3* accuracyScale)
        {
            accuracy = 50;
        }
        else
        {
            accuracy = 25;
        }
        ScoreManager.Instance.CalculateOverallAccuracy(accuracy);
    }
}
