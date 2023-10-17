using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    

    public void TrainingMode()
    {
        SceneManager.LoadScene("TrainingMode", LoadSceneMode.Single);
    }

    public void ZombieMode()
    {
        SceneManager.LoadScene("ZombieMode", LoadSceneMode.Single);
    }
}
