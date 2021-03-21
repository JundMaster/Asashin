using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handlign enemy script.
/// </summary>
public class Enemy : MonoBehaviour
{
    // TEMP VARIABLE
    [SerializeField] private GameObject kunai;
    [SerializeField] private bool spawnKunais;
    /////////////////////////////////////////////


    // Variables
    [SerializeField] private Transform myTarget;
    public Transform MyTarget => myTarget;

    // ILists with all components on this gameobject
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

        // TEMP
        if (spawnKunais) StartCoroutine(ThrowKunaiTemporary());
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


    // TEMP
    private IEnumerator ThrowKunaiTemporary()
    {
        yield return new WaitForSeconds(1f);
        Player player = FindObjectOfType<Player>();
        while (player != null)
        {
            if (kunai)
            {
                GameObject thisKunai = Instantiate(kunai, MyTarget.position + transform.forward, Quaternion.identity);
                thisKunai.layer = 15;
                thisKunai.GetComponent<Kunai>().Behaviour.ParentEnemy = this;
            }
            yield return new WaitForSeconds(2.5f);
        }
    }
}
