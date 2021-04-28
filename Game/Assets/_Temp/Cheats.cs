using System.Collections;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private PlayerInputCustom input;
    private SceneControl sceneControl;

    // Cheat variables
    private float timePressed;
    private ButtonPressed buttonPressed;
    private IEnumerator cheatCoroutine;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        sceneControl = FindObjectOfType<SceneControl>();
    }

    private void OnEnable()
    {
        // Not in main menu
        if ((byte)sceneControl.CurrentSceneEnum() > 0)
        {
            input.Roll += Roll;
            input.MeleeLightAttack += Attack;
            input.ItemChange += ItemChange;
            input.Block += Block;
        }   
    }

    private void OnDisable()
    {
        // Not in main menu
        if ((byte)sceneControl.CurrentSceneEnum() > 0)
        {
            input.Roll -= Roll;
            input.MeleeLightAttack -= Attack;
            input.ItemChange -= ItemChange;
            input.Block -= Block;
        }
        if (cheatCoroutine != null) StopCoroutine(cheatCoroutine);
        cheatCoroutine = null;
    }

    private void Start()
    {
        timePressed = 0;
        buttonPressed = ButtonPressed.Default;
        cheatCoroutine = null;
    }

    /// <summary>
    /// The player has 1 second to perform each action in order. If succeded,
    /// it will trigger a cheat.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheatCoroutine()
    {
        while (Time.time - timePressed < 1 && buttonPressed == ButtonPressed.ItemLeft)
            yield return null;
        while (Time.time - timePressed < 1 && buttonPressed == ButtonPressed.Block)
            yield return null;
        while (Time.time - timePressed < 1 && buttonPressed == ButtonPressed.Attack)
            yield return null;
        while (Time.time - timePressed < 1 && buttonPressed == ButtonPressed.ItemRight)
            yield return null;
        while (Time.time - timePressed < 1 && buttonPressed == ButtonPressed.Block)
            yield return null;
        while (Time.time - timePressed < 1 && buttonPressed == ButtonPressed.Attack)
            yield return null;
        while (Time.time - timePressed < 1 && buttonPressed == ButtonPressed.ItemRight)
            yield return null;
        while (Time.time - timePressed < 1 && buttonPressed == ButtonPressed.Roll)
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            playerStats.FirebombKunais += 10;
            playerStats.Kunais += 10;
            playerStats.SmokeGrenades += 10;
            playerStats.HealthFlasks += 10;
            FindObjectOfType<ItemUIParent>().UpdateAllItemUI();
            break;
        }
    }

    private void Roll()
    {
        timePressed = Time.time;
        buttonPressed = ButtonPressed.Roll;
    }

    private void Attack()
    {
        timePressed = Time.time;
        buttonPressed = ButtonPressed.Attack;
    }

    private void ItemChange(Direction dir)
    {
        timePressed = Time.time;
        if (dir == Direction.Right) buttonPressed = ButtonPressed.ItemRight;
        if (dir == Direction.Left)
        {
            if (cheatCoroutine != null) StopCoroutine(cheatCoroutine);
            buttonPressed = ButtonPressed.ItemLeft;
            cheatCoroutine = CheatCoroutine();
            StartCoroutine(cheatCoroutine);
        }
    }

    private void Block(bool condition)
    {
        timePressed = Time.time;
        if (condition) buttonPressed = ButtonPressed.Block;
    }

    private enum ButtonPressed
    {
        Roll,
        Attack,
        ItemRight,
        ItemLeft,
        Block,
        Default,
    }
}
