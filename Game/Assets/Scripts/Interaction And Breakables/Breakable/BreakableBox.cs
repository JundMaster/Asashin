using System;
using UnityEngine;

/// <summary>
/// Class responsible for breakable boxs.
/// </summary>
public class BreakableBox : MonoBehaviour, IBreakable
{
    [SerializeField] private bool inTutorial;

    [SerializeField] private GameObject brokenObject;

    private ISpawnItemBehaviour spawnItemsBehaviour;

    private void Awake()
    {
        spawnItemsBehaviour = GetComponent<SpawnItemBehaviour>();
    }

    /// <summary>
    /// Method that defines what happens when something collides with this object.
    /// </summary>
    public void Execute()
    {
        Instantiate(brokenObject, transform.position, Quaternion.identity);

        if (inTutorial)
            OnTutorialBrokenBox(TypeOfTutorial.LootWoodenBox);

        spawnItemsBehaviour?.ExecuteBehaviour();

        Destroy(gameObject);
    }


    ///////////////////// Tutorial methods and events //////////////////////////
    protected virtual void OnTutorialBrokenBox(TypeOfTutorial typeOfTutorial) => 
        TutorialBrokenBox?.Invoke(typeOfTutorial);

    public event Action<TypeOfTutorial> TutorialBrokenBox;
    ////////////////////////////////////////////////////////////////////////////
}
