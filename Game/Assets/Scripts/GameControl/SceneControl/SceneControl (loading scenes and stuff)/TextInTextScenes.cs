using System.Collections;
using UnityEngine;
using TMPro;

public class TextInTextScenes : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private TextMeshProEffect textEffect;

    private IEnumerator Start()
    {
        Time.timeScale = 1f;

        // Transparent text
        textMeshPro.color = new Color(0, 0, 0, 0);

        yield return new WaitForSeconds(0.75f);
        // White text
        textMeshPro.color = new Color(1, 1, 1, 1);
        textEffect.Play();
    }
}
