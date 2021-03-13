using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    private PlayableDirector timelineController;

    private CameraController vmController;

    [SerializeField]
    private PlayableAsset newGameTimeline;

    [SerializeField]
    private PlayableAsset continueTimeline;

    [SerializeField]
    private PlayableAsset quitTimeline;

    void Awake()
    {
        timelineController = GetComponent<PlayableDirector>();

        vmController = FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        timelineController.playableAsset = newGameTimeline;
        timelineController.Play(timelineController.playableAsset);
    }

    public void ChangeTimeline()
    {
        if (vmController.IsNewGameCamActive)
        {
            if (timelineController.playableGraph.IsPlaying())
            {
                
                timelineController.time = 0;
                timelineController.Stop();
                timelineController.Evaluate();
                timelineController.playableAsset = newGameTimeline;
                timelineController.Play();
            }
        }
        else if (vmController.IsContinueCamActive)
        {
            if (timelineController.playableGraph.IsPlaying())
            {
                timelineController.time = 0;
                timelineController.Stop();
                timelineController.Evaluate();
                timelineController.playableAsset = continueTimeline;
                timelineController.Play();
            }
        }
        else
        {
            if (timelineController.playableGraph.IsPlaying())
            {
                timelineController.time = 0;
                timelineController.Stop();
                timelineController.Evaluate();
                timelineController.playableAsset = quitTimeline;
                timelineController.Play();
            }
        }
    }
}
