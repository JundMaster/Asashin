using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for running the player components.
/// </summary>
public class Player : MonoBehaviour
{
    private IList<IComponent> myIComponents;
    private IList<IComponent> componentsToRun;

    private void Awake()
    {
        myIComponents = GetComponents<IComponent>();
        componentsToRun = new List<IComponent>();
    }

    private void Start()
    {
        foreach (IComponent comp in myIComponents)
            componentsToRun.Add(comp);
    }

    private void Update()
    {
        foreach (IComponent comp in componentsToRun)
            comp.ComponentUpdate();
    }

    private void FixedUpdate()
    {
        foreach (IComponent comp in componentsToRun)
            comp.ComponentFixedUpdate();
    }
}
