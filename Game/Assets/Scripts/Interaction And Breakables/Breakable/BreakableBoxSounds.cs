using UnityEngine;

/// <summary>
/// Class responsible for handling breakable box sounds.
/// </summary>
public class BreakableBoxSounds : AbstractSoundBase, IFindPlayer
{
    [SerializeField] private AbstractSoundScriptableObject boxBreakSound;

    [Header("Sound emission variables")]
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] private IntensityOfSound intensityOfSound;
    private Player player;

    public override void PlaySound(Sound sound)
    {
        if (sound == Sound.BoxBreak)
        {
            if (player != null)
                gameObject.EmitSound(player, intensityOfSound, enemyLayer);

            boxBreakSound.PlaySound(audioSource);
        }
    }

    public void FindPlayer() =>
        player = FindObjectOfType<Player>();

    public void PlayerLost()
    {
        // Left blank on purpose
    }

    private void OnDrawGizmosSelected()
    {
        if (intensityOfSound == IntensityOfSound.None) { }
        else if (intensityOfSound == IntensityOfSound.Low)
            Gizmos.DrawWireSphere(transform.position, 5);
        else if (intensityOfSound == IntensityOfSound.Normal)
            Gizmos.DrawWireSphere(transform.position, 13);
        else
            Gizmos.DrawWireSphere(transform.position, 20);
    }
}
