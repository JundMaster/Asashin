using UnityEngine;
using UnityEngine.Playables;

public class LowerCubeAnimation : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private BoxCollider trigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            trigger.enabled = false;
            GetComponent<Animator>().SetTrigger("lowerCube");
            timeline.Play();
        }
    }
}
