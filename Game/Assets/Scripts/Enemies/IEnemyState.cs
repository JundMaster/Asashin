/// <summary>
/// Interface implemented by enemy states.
/// </summary>
public interface IEnemyState
{
    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="enemy">Enemy parent of this state.</param>
    void Initialize(Enemy enemy);

    /// <summary>
    /// Method that defines what this state does.
    /// </summary>
    /// <param name="enemy">Enemy parent of this state.</param>
    /// <returns>Returns an IEnemyState.</returns>
    IState Execute(Enemy enemy);
}
