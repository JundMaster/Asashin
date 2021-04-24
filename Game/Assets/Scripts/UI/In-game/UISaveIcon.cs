using UnityEngine;

public class UISaveIcon : MonoBehaviour
{
    private Animator saveIcon;
    private SpawnerController spawnerController;

    private void Awake()
    {
        saveIcon = GetComponent<Animator>();
        spawnerController = FindObjectOfType<SpawnerController>();
    }

    private void OnEnable()
    {
        spawnerController.CheckpointReached += ShowSaveIcon;
    }

    private void OnDisable()
    {
        spawnerController.CheckpointReached -= ShowSaveIcon;
    }

    private void ShowSaveIcon()
    {
        saveIcon.SetTrigger("Save");
    }
}
