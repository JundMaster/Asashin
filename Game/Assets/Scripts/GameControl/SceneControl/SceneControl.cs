using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for controlling scenes.
/// </summary>
public class SceneControl : MonoBehaviour
{
    /// <summary>
    /// Loads a scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    public void LoadScene(int scene) => StartCoroutine(LoadNewScene(scene));


    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    private IEnumerator LoadNewScene(int scene)
    {
        YieldInstruction waitForFrame = new WaitForEndOfFrame();
        AsyncOperation sceneToLoad =
            SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(scene).name);

        // After the progress reaches 1, the scene loads
        while (sceneToLoad.progress < 1)
        {
            yield return waitForFrame;
        }

        yield return null;
    }
}
