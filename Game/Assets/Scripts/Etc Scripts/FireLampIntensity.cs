using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling fire lamp intensity.
/// </summary>
public class FireLampIntensity : MonoBehaviour, IFindPlayer
{
    private Light spotLight;
    private Player player;

    private void Awake()
    {
        spotLight = GetComponent<Light>();
    }		

    private IEnumerator Start()
    {
        YieldInstruction wffu = new WaitForFixedUpdate();
        float multiplier = 40;
        spotLight.innerSpotAngle = 30;

        yield return new WaitForSeconds(Random.Range(0f, 1.5f));
        while (true)
        {
            if (player != null)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < 35)
                {
                    if (spotLight.intensity != 20) spotLight.intensity = 20;

                    if (spotLight.innerSpotAngle < 15)
                    {
                        multiplier *= -1;
                        spotLight.innerSpotAngle = 20;
                    }

                    if (spotLight.innerSpotAngle > 60)
                    {
                        multiplier *= -1;
                        spotLight.innerSpotAngle = 59;
                    }

                    spotLight.innerSpotAngle += Time.fixedDeltaTime * multiplier;
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
