using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling main menu UI.
/// </summary>
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private OptionsScriptableObj options;

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
        yield return new WaitForEndOfFrame();
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGamePaused();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Starts a new game with x difficulty.
    /// </summary>
    /// <param name="difficulty">0 for normal, 1 for hard.</param>
    public void NewGame(int difficulty)
    {
        if (difficulty < 0 || difficulty > 1) 
            throw new Exception("Invalid difficulty");

        options.SaveDifficulty(difficulty);

        OnMainMenuSpawn(SpawnTypeEnum.Newgame);
    }

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
