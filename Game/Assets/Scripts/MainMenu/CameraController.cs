using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera newGameCam;

    [SerializeField]
    private CinemachineVirtualCamera continueCam;

    [SerializeField]
    private CinemachineVirtualCamera optionsCam;

    [SerializeField]
    private CinemachineVirtualCamera quitCam;

    private CinemachineVirtualCamera activeCam;

    public bool IsNewGameCamActive { get; private set; }
    public bool IsContinueCamActive { get; private set; }
    public bool IsOptionCamActive { get; private set; }

    private void Awake()
    {
        activeCam = newGameCam;
        IsNewGameCamActive = true;
        IsContinueCamActive = false;
        IsOptionCamActive = false;
    }

    private void SetActiveCam()
    {
        if (IsNewGameCamActive)
        {
            newGameCam.Priority = 1;
            continueCam.Priority = 0;
            optionsCam.Priority = 0;
            quitCam.Priority = 0;
            activeCam = newGameCam;
        }
        else if (IsContinueCamActive)
        {
            newGameCam.Priority = 0;
            continueCam.Priority = 1;
            optionsCam.Priority = 0;
            quitCam.Priority = 0;
            activeCam = continueCam;
        }
        else if (IsOptionCamActive)
        {
            newGameCam.Priority = 0;
            continueCam.Priority = 0;
            optionsCam.Priority = 1;
            quitCam.Priority = 0;
            activeCam = optionsCam;
        }
        else
        {
            newGameCam.Priority = 0;
            continueCam.Priority = 0;
            optionsCam.Priority = 0;
            quitCam.Priority = 1;
            activeCam = quitCam;
        }
    }

    private void SwitchToNewGameCam()
    {
        IsNewGameCamActive = true;
        IsContinueCamActive = false;
        IsOptionCamActive = false;
        SetActiveCam();
    }

    private void SwitchToContinueCam()
    {
        IsNewGameCamActive = false;
        IsContinueCamActive = true;
        IsOptionCamActive = false;
        SetActiveCam();
    }

    private void SwitchToOptionsCam()
    {
        IsNewGameCamActive = false;
        IsContinueCamActive = false;
        IsOptionCamActive = true;
        SetActiveCam();
    }

    private void SwitchToQuitCam()
    {
        IsNewGameCamActive = false;
        IsContinueCamActive = false;
        IsOptionCamActive = false;
        SetActiveCam();
    }

    public void SwitchActiveCamDown()
    {
        if (activeCam == newGameCam) SwitchToContinueCam();

        else if (activeCam == continueCam) SwitchToOptionsCam();

        else if(activeCam == optionsCam) SwitchToQuitCam();

        else SwitchToNewGameCam();
    }

    public void SwitchActiveCamUp()
    {
        if (activeCam == newGameCam) SwitchToQuitCam();

        else if (activeCam == quitCam) SwitchToOptionsCam();

        else if (activeCam == optionsCam) SwitchToContinueCam();

        else SwitchToNewGameCam();
    }
}
