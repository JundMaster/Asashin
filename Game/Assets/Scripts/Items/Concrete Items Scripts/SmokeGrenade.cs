using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : ItemBehaviour
{
    [SerializeField] private float secondsToBlindEnemies;

    public override void Execute()
    {
        // Blinds enemies for x seconds


        base.Execute();
    }
}
