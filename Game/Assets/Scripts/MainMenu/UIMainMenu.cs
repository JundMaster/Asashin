using UnityEngine;
using System;

/// <summary>
/// Class responsible for handling main menu UI.
/// </summary>
public class UIMainMenu : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.SetString("TypeOfSpawn", SceneEnum.MainMenu.ToString());
    }

    public void NewGame()
    {
        OnMainMenuSpawn(SpawnTypeEnum.Newgame);
    }

    public void LoadGame()
    {
        OnMainMenuSpawn(SpawnTypeEnum.Loadgame);
    }

    protected virtual void OnMainMenuSpawn(SpawnTypeEnum spawnType) =>
        MainMenuSpawn?.Invoke(spawnType);

    /// <summary>
    /// Event registered on SpawnerController.
    /// </summary>
    public event Action<SpawnTypeEnum> MainMenuSpawn;
}
