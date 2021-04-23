using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class KillPlayerTemp : MonoBehaviour
{
#if (UNITY_EDITOR)
    [MenuItem("Costume Methods/Kill Player")]
    private static void KillPlayer()
    {
        FindObjectOfType<PlayerStats>().TakeDamage(100, TypeOfDamage.EnemyMelee);
    }

    [MenuItem("Costume Methods/LoadScene")]
    private static void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
#endif
}
