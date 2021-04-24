using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for running the player components.
/// </summary>
public class Player : MonoBehaviour
{
    // Components
    [SerializeField] private PlayerValuesScriptableObj values;
    public PlayerValuesScriptableObj Values => values;

    private bool playerCurrentlyFighting;
    public bool PlayerCurrentlyFighting
    {
        get => playerCurrentlyFighting;
        set
        {
            if (value == true)
                OnEnteredCombat(true);
            else if (value == false)
                OnEnteredCombat(false);

            playerCurrentlyFighting = value;
        }
    }
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

        PlayerCurrentlyFighting = false;
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

    protected virtual void OnEnteredCombat(bool condition) 
        => EnteredCombat?.Invoke(condition);

    public event Action<bool> EnteredCombat;
}
