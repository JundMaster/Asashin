using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Class responsible for controlling scenes.
/// </summary>
public class SceneControl : MonoBehaviour
{
    // Components
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        // If there is a UI Input base module, it enables it
        EnableControls();
    }

    public Scene CurrentScene() => SceneManager.GetActiveScene();

    /// <summary>
    /// Loads a scene.
    /// Can't overload because of animation events.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    public void LoadScene(SceneEnum scene) => 
        StartCoroutine(LoadNewScene(scene));

    /// <summary>
    /// Coroutine that loads a new scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns null.</returns>
    private IEnumerator LoadNewScene(SceneEnum scene)
    {
        YieldInstruction waitForFrame = new WaitForEndOfFrame();

        DisableControls();
        // Triggers transition to area animation
        anim.SetTrigger("TransitionToArea");

        yield return waitForFrame;
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("TransitionToArea"))
        {
            yield return waitForFrame;
        }
        // After the animation is over

        // Asyc loads a scene
        AsyncOperation sceneToLoad =
            SceneManager.LoadSceneAsync(scene.ToString());

        // After the progress of the async operation reaches 1, the scene loads
        while (sceneToLoad.progress < 1)
        {
            yield return waitForFrame;
        }
        yield return null;
    }

    /// <summary>
    /// Disables all controls.
    /// </summary>
    private void DisableControls()
    {
        PlayerInputCustom input = FindObjectOfType<PlayerInputCustom>();
        BaseInputModule inputModule = FindObjectOfType<BaseInputModule>();
        if (input != null) input.SwitchActionMapToDisable();
        if (inputModule != null) inputModule.enabled = false;
    }

    /// <summary>
    /// Enables controls.
    /// </summary>
    private void EnableControls()
    {
        BaseInputModule inputModule = FindObjectOfType<BaseInputModule>();
        if (inputModule != null) inputModule.enabled = true;
    }
}
