using UnityEngine;

/// <summary>
/// Finds tutorial scenes control. Used in tutorial pause menu UI buttons..
/// </summary>
public class UITutorialControlButtons : MonoBehaviour
{
    private TutorialScenesControl tutorialControl;
    private SceneControl sceneControl;
    private SceneEnum sceneToLoad;

    private void Awake()
    {
        sceneControl = FindObjectOfType<SceneControl>();
        tutorialControl = FindObjectOfType<TutorialScenesControl>();
        sceneToLoad = tutorialControl.SceneToLoad;
    }

    public void NextTutorial() =>
        sceneControl.LoadScene(sceneToLoad);

    public void QuitTutorial() =>
        sceneControl.LoadScene(SceneEnum.Area1);

    public void RepeatTutorial() =>
        sceneControl.LoadScene(sceneControl.CurrentSceneEnum());
}
