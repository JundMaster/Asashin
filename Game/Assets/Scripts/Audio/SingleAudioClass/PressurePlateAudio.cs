using UnityEngine;

/// <summary>
/// Class responsible for playing Pressure Plate Audio.
/// </summary>
public class PressurePlateAudio : AbstractSoundBase, IFindPlayer
{
    [SerializeField] private AbstractSoundScriptableObject pressurePlateEnter;

    [Header("Sound emission variables")]
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] private IntensityOfSound intensityOfSound;
    private Player player;

    protected new void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
    }

    public override void PlaySound(Sound sound)
    {
        if (sound == Sound.PressurePlate)
        {
            if (player != null)
                gameObject.EmitSound(player, intensityOfSound, enemyLayer);
            pressurePlateEnter.PlaySound(audioSource);
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
