using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for running the player components.
/// </summary>
public class Player : MonoBehaviour
{
    // Components
    [SerializeField] private PlayerValues values;
    public PlayerValues Values => values;

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
    }

    private void Update()
    {
        foreach (IAction comp in componentsToRun)
            comp.ComponentUpdate();
    }

    private void FixedUpdate()
    {
        foreach (IAction comp in componentsToRun)
            comp.ComponentFixedUpdate();
    }
}
