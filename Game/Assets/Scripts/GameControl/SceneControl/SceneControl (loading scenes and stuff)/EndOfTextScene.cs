using UnityEngine;
using System.Collections;

public class EndOfTextScene : MonoBehaviour
{
    [SerializeField] private float secondsToWaitBeforeChangingScene;
    [SerializeField] private SceneEnum sceneToChangeTo;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(secondsToWaitBeforeChangingScene);

        FindObjectOfType<SceneControl>().LoadScene(sceneToChangeTo);
    }
}
