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

    protected new void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
    }

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
}
