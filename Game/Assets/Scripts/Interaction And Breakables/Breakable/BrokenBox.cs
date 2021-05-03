using UnityEngine;

/// <summary>
/// Class responsible for broken boxes.
/// </summary>
public class BrokenBox : MonoBehaviour
{
    private BreakableBoxSounds boxSounds;

    private void Awake()
    {
        boxSounds = GetComponent<BreakableBoxSounds>();
    }

    private void Start()
    {
        boxSounds.PlaySound(Sound.BoxBreak);
    }
}
