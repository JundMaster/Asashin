using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class responsible for controlling scenes.
/// </summary>
public class SceneControl : MonoBehaviour
{
    [SerializeField] private Image loadingBar;

    // Components
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable() => 
        SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() =>
        SceneManager.sceneLoaded -= OnSceneLoaded;

    public Scene CurrentScene() => SceneManager.GetActiveScene();

    public SceneEnum CurrentSceneEnum() =>
        (SceneEnum)Enum.Parse(typeof(SceneEnum), CurrentScene().name);

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
        OnStartedLoadingScene();

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
        while (sceneToLoad.progress <= 1)
        {
            loadingBar.fillAmount = sceneToLoad.progress;
            yield return waitForFrame;
        }
    }

    /// <summary>
    /// Disables all controls. Happens when scene is ending.
    /// </summary>
    private void DisableControls()
    {
        PlayerInputCustom input = FindObjectOfType<PlayerInputCustom>();
        if (input != null)
        {
            input.SwitchActionMapToDisable();
            input.EnableInputModule(false);
        }
    }

    /// <summary>
    /// Every time a scene loads, calls a coroutine to enable base module input.
    /// </summary>
    /// <param name="scene">Null.</param>
    /// <param name="mode">Null.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(EnableControls());
    }

    /// <summary>
    /// Enables base module input.
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableControls()
    {
        yield return new WaitForFixedUpdate();

        PlayerInputCustom input = FindObjectOfType<PlayerInputCustom>();
        if (input != null)
            input.EnableInputModule(true);
    }

    /// <summary>
    /// Invokes startedloadingscene event.
    /// </summary>
    protected virtual void OnStartedLoadingScene() =>
        StartedLoadingScene?.Invoke();

    /// <summary>
    /// Event triggered when this class starts loading a scene.
    /// Event registered on AudioController.
    /// </summary>
    public event Action StartedLoadingScene;
}
