using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling fire lamp intensity.
/// </summary>
public class FireLampIntensity : MonoBehaviour, IFindPlayer
{
    private Light spotLight;
    private Player player;

    private readonly float MINIMUMINT = 7;
    private readonly float MAXIMUMINT = 9; 

    private void Awake()
    {
        spotLight = GetComponent<Light>();
    }		

    private IEnumerator Start()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        while (true)
        {
            if (player != null)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < 30)
                {
                    spotLight.intensity = Random.Range(MINIMUMINT, MAXIMUMINT);
                    spotLight.spotAngle = Random.Range(140f, 145f);
                    yield return wffu;

                    spotLight.intensity--;
                    spotLight.spotAngle -= 2;
                    yield return wffu;

                    spotLight.intensity--;
                    spotLight.spotAngle -= 2;
                    yield return wffu;

                    spotLight.intensity--;
                    spotLight.spotAngle -= 2;
                    yield return wffu;
                }
                else
                {
                    if (spotLight.intensity != 0) spotLight.intensity = 0;
                    yield return wffu;
                }
            }
            else
            {
                yield return wffu;
            }
        }
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
