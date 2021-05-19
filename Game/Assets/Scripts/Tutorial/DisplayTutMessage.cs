using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class DisplayTutMessage : MonoBehaviour
{
    [SerializeField] private GameObject initialSelectedButton;
    private GameObject lastSelectedGameObject;

    [Header("Tutorial Text")]
    [SerializeField] TextMeshProEffect textEffect;
    [SerializeField] TextMeshProUGUI textMeshPro;

    [Header("Close Button Text")]
    [SerializeField] TextMeshProUGUI closeTextMeshPro;

    // Components
    private EventSystem eventSys;
    private PlayerInputCustom input;

    private enum CurrentTutorial { _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13 }
    [SerializeField] private CurrentTutorial currentTutorial;

    private readonly string KEYBOARDMOVEMENT    = "WASD";
    private readonly string GAMEPADMOVEMENT     = "LEFT ANALOG";
    private readonly string KEYBOARDSPRINT      = "LEFT SHIFT";
    private readonly string GAMEPADSPRINT       = "R2";
    private readonly string KEYBOARDWALK        = "CTRL";
    private readonly string GAMEPADWALK         = "L2";
    private readonly string KEYBOARDATTACK      = "LEFT MOUSE";
    private readonly string GAMEPADATTACK       = "SQUARE";
    private readonly string KEYBOARDBLOCK       = "RIGHT MOUSE";
    private readonly string GAMEPADBLOCK        = "R1";
    private readonly string KEYBOARDLOOT        = "LEFT MOUSE";
    private readonly string GAMEPADLOOT         = "SQUARE";
    private readonly string KEYBOARDWALLHUG     = "MIDDLE MOUSE";
    private readonly string GAMEPADWALLHUG      = "CIRCLE";
    private readonly string KEYBOARDWALLHUGMOVEMENT = "AD";
    private readonly string GAMEPADWALLHUGMOVEMENT  = "LEFT ANALOG";
    private readonly string KEYBOARDITEMUSE     = "F";
    private readonly string GAMEPADITEMUSE      = "TRIANGLE";
    private readonly string KEYBOARDITEMLEFT    = "1";
    private readonly string GAMEPADITEMLEFT     = "D-PAD DOWN";
    private readonly string KEYBOARDITEMRIGHT   = "3";
    private readonly string GAMEPADITEMRIGHT    = "D-PAD UP";
    private readonly string KEYBOARDTARGET      = "LEFT ALT";
    private readonly string GAMEPADTARGET       = "L1";
    private readonly string KEYBOARDTARGETLEFT  = "Q";
    private readonly string GAMEPADTARGETLEFT   = "D-PAD LEFT";
    private readonly string KEYBOARDTARGETRIGHT = "E";
    private readonly string GAMEPADTARGETRIGHT  = "D-PAD RIGHT";
    private readonly string KEYBOARDROLL        = "SPACE";
    private readonly string GAMEPADROLL         = "X";

    private string movement;
    private string sprint;
    private string walk;
    private string attack;
    private string block;
    private string loot;
    private string wallHug;
    private string wallHugMovement;
    private string itemUse;
    private string itemLeft;
    private string itemRight;
    private string target;
    private string targetLeft;
    private string targetRight;
    private string roll;

    private void Awake()
    {
        eventSys = FindObjectOfType<EventSystem>();
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private IEnumerator Start()
    {
        // Transparent text
        textMeshPro.color = new Color(0, 0, 0, 0);

        yield return new WaitForEndOfFrame();
        eventSys.SetSelectedGameObject(initialSelectedButton);

        yield return new WaitForSeconds(1);
        // White text
        textMeshPro.color = new Color(1, 1, 1, 1);
        textEffect.Play();
    }

    private void OnDisable()
    {
        // Enables gameplay controls
        input.SwitchActionMapToGameplay();
    }

    private void Update()
    {
        // Disables gameplay controls
        input.SwitchActionMapToGamePaused();

        #region Always keeps close button selected
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
        #endregion

        // Checks if there is any gamepad connected and updates text
        var gamePads = Gamepad.all;
        if (gamePads.Count > 0)
        {
            movement = GAMEPADMOVEMENT;
            sprint = GAMEPADSPRINT;
            walk = GAMEPADWALK;
            attack = GAMEPADATTACK;
            block = GAMEPADBLOCK;
            loot = GAMEPADLOOT;
            wallHug = GAMEPADWALLHUG;
            itemUse = GAMEPADITEMUSE;
            itemLeft = GAMEPADITEMLEFT;
            itemRight = GAMEPADITEMRIGHT;
            target = GAMEPADTARGET;
            targetLeft = GAMEPADTARGETLEFT;
            targetRight = GAMEPADTARGETRIGHT;
            roll = GAMEPADROLL;

            closeTextMeshPro.text = "x";
        }
        else
        {
            movement = KEYBOARDMOVEMENT;
            sprint = KEYBOARDSPRINT;
            movement = KEYBOARDMOVEMENT;
            sprint = KEYBOARDSPRINT;
            walk = KEYBOARDWALK;
            attack = KEYBOARDATTACK;
            block = KEYBOARDBLOCK;
            loot = KEYBOARDLOOT;
            wallHug = KEYBOARDWALLHUG;
            itemUse = KEYBOARDITEMUSE;
            itemLeft = KEYBOARDITEMLEFT;
            itemRight = KEYBOARDITEMRIGHT;
            target = KEYBOARDTARGET;
            targetLeft = KEYBOARDTARGETLEFT;
            targetRight = KEYBOARDTARGETRIGHT;
            roll = KEYBOARDROLL;

            closeTextMeshPro.text = "enter";
        }

        DisplayTutorialText();
    }

    private void DisplayTutorialText()
    {
        switch (currentTutorial)
        {
            case CurrentTutorial._1:
                textMeshPro.text = 
                    $"In order to move your character, use {movement} to run. You can also use {sprint} to sprint while running.";
                break;
            case CurrentTutorial._2:
                textMeshPro.text =
                    $"Use {walk} to sneak. If you sneak inside a bush you won't be visibile by enemies.";
                break;
            case CurrentTutorial._3:
                textMeshPro.text =
                    $"Notice that the enemy has a vision cone in front of him. This cone represents his visible area. Sneak through the bushes without beeing seen.";
                break;
            case CurrentTutorial._4:
                textMeshPro.text =
                    $"In Asashin, keep in mind that every action produces noise. In order to avoid producing noise you can sneak instead of running. Try to pass through the guards without alerting them.";
                break;
            case CurrentTutorial._5:
                textMeshPro.text =
                    $"You are able to attack or block attacks with your Ninjatō. To attack, choose a direction and press {attack}. To block, choose a direction and press {block}. Move towards the vision cone when you're ready.";
                break;
            case CurrentTutorial._6:
                textMeshPro.text =
                    $"If you approach an enemy off-guard you will be able to perform an instant kill. In order to do this, approach the enemy while sneaking without alerting him and press {attack}.";
                break;
            case CurrentTutorial._7:
                textMeshPro.text =
                    $"While exploring, you'll find wooden boxes and treasures with random quantities of loot inside. You can attack and break the wooden boxes and interact with treasures by pressing {loot}.";
                break;
            case CurrentTutorial._8:
                textMeshPro.text =
                    $"If you approach a wall, you can wall hug in order to be more stealthy and approach an enemy with care. In order to wall hug, approach a wall and press {wallHug}. You can move by pressing {wallHugMovement}.";
                break;
            case CurrentTutorial._9:
                textMeshPro.text =
                    $"In the bottom right corner you'll find the current items you have. In order to choose an item press {itemLeft} or {itemRight} and use that item with {itemUse}.";
                break;
            case CurrentTutorial._10:
                textMeshPro.text =
                    $"In order to have better control over your enemy, you are able to target him. While targeting the enemy, you are able to attack, block and use items in his direction. To target an enemy press {target}. To switch to next target press {targetLeft} or {targetRight}.";
                break;
            case CurrentTutorial._11:
                textMeshPro.text =
                    $"In order to avoid attacks you can roll by pressing {roll}.";
                break;
            case CurrentTutorial._12:
                textMeshPro.text =
                    $"If you roll in the exact moment an enemy is attacking, you'll perform a dodge. This dodge action will perform a slow motion over some seconds, giving you a big advantage in fights against various enemies. Move towards the vision cone when you're ready.";
                break;
            case CurrentTutorial._13:
                textMeshPro.text =
                    $"In this last tutorial you'll fight an enemy using the techniques you have learned so far. Move towards the vision cone when you're ready.";
                break;
        }
    }
}
