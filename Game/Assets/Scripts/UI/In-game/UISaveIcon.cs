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
        if (spawnerController != null)
            spawnerController.CheckpointReached += ShowSaveIcon;
    }

    private void OnDisable()
    {
        if (spawnerController != null)
            spawnerController.CheckpointReached -= ShowSaveIcon;
    }

    private void ShowSaveIcon()
    {
        saveIcon.SetTrigger("Save");
    }
}
