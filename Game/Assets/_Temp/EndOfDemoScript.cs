using UnityEngine;
using System.Collections;

public class EndOfDemoScript : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3);

        FindObjectOfType<SceneControl>().LoadScene(SceneEnum.MainMenu);
    }
}
