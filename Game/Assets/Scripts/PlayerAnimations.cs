using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour, IComponent
{
    // Components
    private Animator anim;
    private PlayerMovement movement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    public void ComponentFixedUpdate()
    {
        
    }

    public void ComponentUpdate()
    {
        anim.SetFloat("Movement", movement.Direction.magnitude);
    }
}
