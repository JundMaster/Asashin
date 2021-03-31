using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for making every IFindPlayer interface find player.
/// </summary>
public class PlayerFindMe : MonoBehaviour
{
    /// <summary>
    /// What happens when the player is spawned.
    /// </summary>
    private void OnEnable()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IFindPlayer[] childrenInterfaces = 
                rootGameObject.GetComponentsInChildren<IFindPlayer>();

            foreach (IFindPlayer childInterface in childrenInterfaces)
            {
                childInterface.FindPlayer();
                Debug.Log(childInterface);
            }
        }        
    }

    /// <summary>
    /// What happens when the player is dies.
    /// </summary>
    private void OnDisable()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IFindPlayer[] childrenInterfaces =
                rootGameObject.GetComponentsInChildren<IFindPlayer>();

            foreach (IFindPlayer childInterface in childrenInterfaces)
            {
                childInterface.PlayerLost();
            }
        }
    }
}
