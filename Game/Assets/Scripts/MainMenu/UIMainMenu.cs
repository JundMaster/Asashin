using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling main menu UI.
/// </summary>
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private OptionsScriptableObj options;

    private void Awake()
    {
        PlayerPrefs.SetString("TypeOfSpawn", SceneEnum.MainMenu.ToString());
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGamePaused();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NewGame(int difficulty)
    {
        if (difficulty != 0 || difficulty != 1) 
            throw new Exception("Invalid difficulty");

        options.SaveDifficulty(difficulty);

        OnMainMenuSpawn(SpawnTypeEnum.Newgame);
    }

    public void LoadGame()
    {
        OnMainMenuSpawn(SpawnTypeEnum.Loadgame);
    }

    public void Quit()
    {
        Application.Quit();
    }

    protected virtual void OnMainMenuSpawn(SpawnTypeEnum spawnType) =>
        MainMenuSpawn?.Invoke(spawnType);

    /// <summary>
    /// Event registered on SpawnerController.
    /// </summary>
    public event Action<SpawnTypeEnum> MainMenuSpawn;
}
