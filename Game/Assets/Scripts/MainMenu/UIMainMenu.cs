using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class responsible for handling main menu UI.
/// </summary>
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private OptionsScriptableObj options;
    [SerializeField] private GameObject initialButton;

    private EventSystem eventSys;
    private GameObject lastSelectedGameObject;

    /// <summary>
    /// Sets playerprefs typeofspawn to main menu.
    /// </summary>
    private void Awake() =>
        PlayerPrefs.SetString("TypeOfSpawn", SceneEnum.MainMenu.ToString());

    /// <summary>
    /// Resets controls, time and mouse lockstate.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        // After a frame

        PlayerPrefs.SetString("TypeOfSpawn", SceneEnum.MainMenu.ToString());
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGamePaused();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        eventSys = FindObjectOfType<EventSystem>();
        lastSelectedGameObject = initialButton;
        eventSys.SetSelectedGameObject(initialButton);
    }

    /// <summary>
    /// Always keeps a button selected.
    /// </summary>
    private void Update()
    {
        if (eventSys != null)
        {
            // Keeps last selected gameobject
            if (eventSys.currentSelectedGameObject != null &&
                eventSys.currentSelectedGameObject != lastSelectedGameObject)
            {
                lastSelectedGameObject = eventSys.currentSelectedGameObject;
            }
            // If the button is null, it selects the last selected button
            if (eventSys.currentSelectedGameObject == null)
            {
                eventSys.SetSelectedGameObject(lastSelectedGameObject);
            }
        }
    }

    /// <summary>
    /// Starts a new game with x difficulty.
    /// </summary>
    /// <param name="difficulty">0 for normal, 1 for hard.</param>
    public void SetDifficulty(int difficulty)
    {
        if (difficulty < 0 || difficulty > 1)
            throw new Exception("Invalid difficulty");

        options.SaveDifficulty(difficulty);
    }

    public void NewGame() => 
        OnMainMenuSpawn(SpawnTypeEnum.Newgame);

    public void Tutorial() => 
        OnMainMenuSpawn(SpawnTypeEnum.Tutorial);

    public void LoadGame() =>
        OnMainMenuSpawn(SpawnTypeEnum.Loadgame);

    public void Quit() =>
        Application.Quit();

    protected virtual void OnMainMenuSpawn(SpawnTypeEnum spawnType) =>
        MainMenuSpawn?.Invoke(spawnType);

    /// <summary>
    /// Event registered on SpawnerController.
    /// </summary>
    public event Action<SpawnTypeEnum> MainMenuSpawn;
}
