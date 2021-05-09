using UnityEngine;

public static class GameObjectExtenstions
{
    /// <summary>
    /// If this player isn't fighting, enemies will react to every sound that is emitted.
    /// </summary>
    /// <param name="thisEmitter">What emitted this sound.</param>
    /// <param name="player">Player.</param>
    /// <param name="intensity">Intensity of this sound.</param>
    /// <param name="enemyLayer">Enemy layer.</param>
    public static void EmitSound(this GameObject thisEmitter, Player player, 
        IntensityOfSound intensity, LayerMask enemyLayer)
    {
        if (player.PlayerCurrentlyFighting == 0)
        {
            float sizeOfAlert = 0;

            switch (intensity)
            {
                case IntensityOfSound.Low:
                    sizeOfAlert = 5;
                    break;
                case IntensityOfSound.Normal:
                    sizeOfAlert = 10;
                    break;
                case IntensityOfSound.Extreme:
                    sizeOfAlert = 20;
                    break;
            }

            Collider[] enemiesAround =
                Physics.OverlapSphere(thisEmitter.transform.position, sizeOfAlert, enemyLayer);

            if (enemiesAround.Length > 0)
            {
                foreach (Collider enemyCollider in enemiesAround)
                {
                    if (enemyCollider.TryGetComponent(out EnemySimple enemy))
                    {
                        enemy.OnReactToSound(thisEmitter.transform.position);
                    }
                }
            }
        }
    }
}
