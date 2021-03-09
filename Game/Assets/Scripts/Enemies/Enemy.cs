using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for handlign enemy script.
/// </summary>
public class Enemy : MonoBehaviour
{
    // Variables
    [SerializeField] private Transform myTarget;
    public Transform MyTarget => myTarget;

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
