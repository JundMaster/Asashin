using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for controlling scenes.
/// </summary>
public class SceneControl : MonoBehaviour
{
    public int SceneToLoad() => SceneManager.GetActiveScene().buildIndex;

    public void LoadScene(int scene) => SceneManager.LoadScene(scene);
}
