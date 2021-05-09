using UnityEngine;

/// <summary>
/// Interface implemented by every gameobject that needs to react to sound.
/// </summary>
public interface IHearSound
{
    /// <summary>
    /// Defines what happens when this gameobject hears a sound.
    /// </summary>
    /// <param name="positionOfSound">Position where the sound came from.</param>
    void OnReactToSound(Vector3 positionOfSound);
}
