using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for running the player components.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private bool inTutorial;
    public bool InTutorial => inTutorial;

    [SerializeField] private PlayerValuesScriptableObj values;
    public PlayerValuesScriptableObj Values => values;

    public byte PlayerCurrentlyFighting { get; set; }

    // ILists with components
    private IList<IAction> myIComponents;
    private IList<IAction> componentsToRun;

    private void Awake()
    {
        myIComponents = GetComponents<IAction>();
        componentsToRun = new List<IAction>();
    }

    private void Start()
    {
        foreach (IAction comp in myIComponents)
            componentsToRun.Add(comp);

        PlayerCurrentlyFighting = 0;
    }

    private void Update()
    {
        foreach (IAction comp in componentsToRun)
            comp?.ComponentUpdate();
    }

    private void FixedUpdate()
    {
        foreach (IAction comp in componentsToRun)
            comp?.ComponentFixedUpdate();
    }
}
