using UnityEngine;

/// <summary>
/// Class responsible for broken box pieces.
/// </summary>
public class BrokenBoxPiece : MonoBehaviour
{
    private BreakableBoxSounds boxSounds;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxSounds = GetComponent<BreakableBoxSounds>();    
    }

    private void Start()
    {
        Vector3 force = new Vector3(555, 555, 555);
        rb.AddForce(force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        boxSounds.PlaySound(Sound.BoxBreak);
    }
}
