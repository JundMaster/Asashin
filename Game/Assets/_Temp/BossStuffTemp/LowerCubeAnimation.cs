using UnityEngine;
using UnityEngine.Playables;

public class LowerCubeAnimation : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            GetComponent<Animator>().SetTrigger("lowerCube");
            timeline.Play();
        }
    }
}
