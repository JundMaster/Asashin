using System.Collections;

/// <summary>
/// Interface implemented by every item UI.
/// </summary>
public interface IItemUI
{
    /// <summary>
    /// Updates current value on ui.
    /// </summary>
    void UpdateValue();

    /// <summary>
    /// Updates item delay UI.
    /// </summary>
    /// <returns>Null.</returns>
    IEnumerator UpdateDelay();
}
