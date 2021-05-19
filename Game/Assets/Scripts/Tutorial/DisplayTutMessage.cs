using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DisplayTutMessage : MonoBehaviour
{
    [SerializeField] private GameObject initialSelectedButton;

    // Components
    private EventSystem eventSys;
    private PlayerInputCustom input;

    private GameObject lastSelectedGameObject;

    void Awake()
    {
        eventSys = FindObjectOfType<EventSystem>();
        input = FindObjectOfType<PlayerInputCustom>();
    }

    void Start()
    {
        StartCoroutine(SelectCloseButton());  
    }

    // Update is called once per frame
    void Update()
    {
        input.SwitchActionMapToGamePaused();

        // Keeps last selected gameobject
        if (eventSys.currentSelectedGameObject != null &&
            eventSys.currentSelectedGameObject != lastSelectedGameObject)
        {
            lastSelectedGameObject = eventSys.currentSelectedGameObject;
        }
        // If the button is null, it selects the last selected button
        if (eventSys.currentSelectedGameObject == null)
        {
            eventSys.SetSelectedGameObject(lastSelectedGameObject);
        }
    }

    private IEnumerator SelectCloseButton()
    {
        yield return new WaitForEndOfFrame();
        eventSys.SetSelectedGameObject(initialSelectedButton);
    }
}
