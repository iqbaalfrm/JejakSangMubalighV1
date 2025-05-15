using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLevel : MonoBehaviour
{
    public string sceneName = "LevelSatu";

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
