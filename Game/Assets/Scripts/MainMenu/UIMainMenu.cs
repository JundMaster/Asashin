using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling main menu UI.
/// </summary>
public class UIMainMenu : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.SetString("TypeOfSpawn", SceneEnum.MainMenu.ToString());
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGamePaused();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void NewGame()
    {
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
