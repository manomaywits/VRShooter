using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private float score;
    private float overallAvgAccuracy;
    private float totalAccuracy;
    private int accuracyTargetHitCount;

    [SerializeField] private TextMeshProUGUI scoretxt;
    [SerializeField] private TextMeshProUGUI accuracytxt;

    /// <summary>
    /// Gets triggered whenever score changes
    /// </summary>
    public Action<float> OnScoreChange;

    /// <summary>
    /// Gets triggered whenever AvgAccuracy changes
    /// </summary>
    public Action<float> OnAvgAccuracyChange;

    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        score = 0;
        overallAvgAccuracy = 0;
        totalAccuracy = 0;

        OnScoreChange += SubToScoreChange;
        OnAvgAccuracyChange += SubToAccuracyChange;
        OnScoreChange?.Invoke(this.score);
        OnAvgAccuracyChange?.Invoke(this.overallAvgAccuracy);
    }

    void SubToScoreChange(float value)
    {
        scoretxt.text = value.ToString();
    }

    void SubToAccuracyChange(float value)
    {
        accuracytxt.text = value.ToString(format: "00.00") + " %";
    }
    

    /// <summary>
    ///  Sets the score for the ScoreManager
    /// </summary>
    /// <param name="score"></param>
    public void SetScore(float score)
    {
        this.score = score;
        if (this.score < 0)
        {
            Debug.LogWarning("---- Score is less than 0! Setting score = 0 ----");
            this.score = 0;
        }

        OnScoreChange?.Invoke(this.score);
    }

    /// <summary>
    /// Returns the score of the ScoreManager
    /// </summary>
    /// <returns></returns>
    public float GetScore()
    {
        return score;
    }

    /// <summary>
    /// Increases score by some value
    /// </summary>
    /// <param name="increaseByValue"></param>
    public void IncreaseScore(float increaseByValue)
    {
        score += increaseByValue;
        OnScoreChange?.Invoke(this.score);
    }

    /// <summary>
    /// Decreases score by some value
    /// </summary>
    /// <param name="decreaseByValue"></param>
    public void DecreaseScore(float decreaseByValue)
    {
        score -= decreaseByValue;
        if (score < 0)
        {
            Debug.LogWarning("---- Score is less than 0! Setting score = 0 ----");
            score = 0;
        }

        OnScoreChange?.Invoke(this.score);
    }

    /// <summary>
    /// Calculates overall accuracy of the targets hit
    /// </summary>
    /// <param name="accuracy"></param>
    public void CalculateOverallAccuracy(float accuracy)
    {
        accuracyTargetHitCount++;
        totalAccuracy += accuracy;
        overallAvgAccuracy = totalAccuracy / accuracyTargetHitCount;
        OnAvgAccuracyChange?.Invoke(overallAvgAccuracy);
    }

    /// <summary>
    /// Resets overall average accuracy
    /// </summary>
    public void ResetAccuracy()
    {
        overallAvgAccuracy = 0;
        OnAvgAccuracyChange?.Invoke(overallAvgAccuracy);
    }
}