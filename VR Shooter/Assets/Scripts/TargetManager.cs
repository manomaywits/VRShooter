using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetManager : MonoBehaviour
{
  public static TargetManager Instance;
  public List<GameObject> targets = new List<GameObject>();
  public bool inSixtySecondMode = false;
  public bool inThirtyTargeyMode = false;
  public GameObject freeTargetMode;
  [SerializeField] private GameObject playUI;
  [SerializeField] private GameObject timer;
  [SerializeField] private TextMeshProUGUI tmpTimer;

  private void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(this);
  }

  private void Start()
  {
    timer.SetActive(false);
  }

  public void SixtySecondsMode()
  {
    freeTargetMode.SetActive(false);
    playUI.SetActive(false);
    StartCoroutine(SixtyModeStarted());
  }

  IEnumerator SixtyModeStarted()
  {
    timer.SetActive(true);
    inSixtySecondMode = true;
    ScoreManager.Instance.SetScore(0);
    ScoreManager.Instance.ResetAccuracy();
    int time = 60;
    tmpTimer.text = time.ToString();
    ShowTarget();
    while (inSixtySecondMode)
    {
      yield return new WaitForSeconds(1f);
      time--;
      tmpTimer.text = time.ToString();
      if (time == 0)
        inSixtySecondMode = false;
    }
    Debug.Log("Sixty Mode Ended");
    foreach (GameObject target in targets)
    {
      target.SetActive(false);
    }
    yield return new WaitForSeconds(3f);
    freeTargetMode.SetActive(true);
    playUI.SetActive(true);
  }

  public void ShowTarget()
  {
    var randomIndex = Random.Range(0, targets.Count);
    foreach (GameObject target in targets)
    {
      target.SetActive(false);
    }
    targets[randomIndex].SetActive(true);
    targets[randomIndex].GetComponentInChildren<Target>().isActive = true;
  }

  public void ShowTargetWithDelay()
  {
    Invoke(nameof(ShowTarget),0.1f);
  }

  public void ThirtyTargetMode()
  {
    freeTargetMode.SetActive(false);
    playUI.SetActive(false);

    StartCoroutine(ThirtyTargetModeStarted());
  }
  
  IEnumerator ThirtyTargetModeStarted()
  {
    inThirtyTargeyMode = true;
    ScoreManager.Instance.SetScore(0);
    ScoreManager.Instance.ResetAccuracy();

    int targetcount = 30;
    while (inThirtyTargeyMode)
    {
      yield return new WaitForSeconds(2f);
      targetcount--;
      ShowTarget();
      if (targetcount == 0)
        inThirtyTargeyMode = false;
    }
    Debug.Log("Thirty Mode Ended");

    yield return new WaitForSeconds(3f);
    freeTargetMode.SetActive(true);
    playUI.SetActive(true);
    
  }
}
