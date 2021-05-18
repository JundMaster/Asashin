using System;

/// <summary>
/// Class responsible for handling a tutorial enemy.
/// </summary>
public class EnemyTutorial : EnemySimple
{
    /// <summary>
    /// Method that invokes TutorialAlert.
    /// Happens every time the enemy finds the player on tutorial.
    /// </summary>
    public void OnTutorialAlert() => TutorialAlert?.Invoke();

    /// <summary>
    /// Method that invokes TutorialBlock.
    /// Happens every time the player blocks an enemy on tutorial.
    /// </summary>
    public void OnTutorialBlock(TypeOfTutorial typeOfTutorial) => 
        TutorialBlock?.Invoke(typeOfTutorial);

    /// <summary>
    /// Method that invokes TutorialDie.
    /// Happens every time the enemy finds the player on tutorial.
    /// </summary>
    public void OnTutorialDie(TypeOfTutorial typeOfTutorial) =>
        TutorialDie?.Invoke(typeOfTutorial);

    /// <summary>
    /// Event registered on Tutorial script. Happens when enemy finds player.
    /// </summary>
    public event Action TutorialAlert;

    /// <summary>
    /// Event registered on Tutorial script. Happens when enemy finds player.
    /// </summary>
    public event Action<TypeOfTutorial> TutorialBlock;

    /// <summary>
    /// Event registerd on Tutorial script. Happens when the enemy dies.
    /// </summary>
    public event Action<TypeOfTutorial> TutorialDie;
}
