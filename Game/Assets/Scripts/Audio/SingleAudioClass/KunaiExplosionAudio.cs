using UnityEngine;

/// <summary>
/// Class responsible for playing kunai explosion audio.
/// </summary>
public class KunaiExplosionAudio : AbstractSoundBase
{
    [SerializeField] private AbstractSoundScriptableObject explosion;
    [SerializeField] private IntensityOfSound intensityOfSound;
    [SerializeField] private LayerMask enemyLayer;

    public override void PlaySound(Sound sound)
    {
        // Left blank on purpose
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            gameObject.EmitSound(player, intensityOfSound, enemyLayer);
            explosion.PlaySound(audioSource);
        }
    }
}
