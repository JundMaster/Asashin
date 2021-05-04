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
        float timeToPressNext = 1;
        byte value = 0;
        while (value == 0 && Time.time - timePressed < timeToPressNext)
        {
            if (buttonPressed != ButtonPressed.ItemLeft &&
                buttonPressed != ButtonPressed.Block) 
                StopCoroutine(cheatCoroutine);

            if (buttonPressed == ButtonPressed.Block) value = 1;
            yield return null;
        }

        while (value == 1 && Time.time - timePressed < timeToPressNext)
        {
            if (buttonPressed != ButtonPressed.Block &&
                buttonPressed != ButtonPressed.Attack)
                StopCoroutine(cheatCoroutine);

            if (buttonPressed == ButtonPressed.Attack) value = 2;
            yield return null;
        }

        while (value == 2 && Time.time - timePressed < timeToPressNext)
        {
            if (buttonPressed != ButtonPressed.Attack &&
                buttonPressed != ButtonPressed.ItemRight)
                StopCoroutine(cheatCoroutine);

            if (buttonPressed == ButtonPressed.ItemRight) value = 3;
            yield return null;
        }

        while (value == 3 && Time.time - timePressed < timeToPressNext)
        {
            if (buttonPressed != ButtonPressed.ItemRight &&
                buttonPressed != ButtonPressed.Block)
                StopCoroutine(cheatCoroutine);

            if (buttonPressed == ButtonPressed.Block) value = 4;
            yield return null;
        }

        while (value == 4 && Time.time - timePressed < timeToPressNext)
        {
            if (buttonPressed != ButtonPressed.Block &&
                buttonPressed != ButtonPressed.Attack)
                StopCoroutine(cheatCoroutine);

            if (buttonPressed == ButtonPressed.Attack) value = 5;
            yield return null;
        }

        while (value == 5 && Time.time - timePressed < timeToPressNext)
        {
            if (buttonPressed != ButtonPressed.Attack &&
                buttonPressed != ButtonPressed.ItemRight)
                StopCoroutine(cheatCoroutine);

            if (buttonPressed == ButtonPressed.ItemRight) value = 6;
            yield return null;
        }

        while (value == 6 && Time.time - timePressed < timeToPressNext)
        {
            if (buttonPressed != ButtonPressed.ItemRight &&
                buttonPressed != ButtonPressed.Roll)
                StopCoroutine(cheatCoroutine);

            if (buttonPressed == ButtonPressed.Roll) value = 7;
            yield return null;
        }

        while (value == 7 && Time.time - timePressed < timeToPressNext)
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
