using System.Collections;
using UnityEngine;

public class Cheats : MonoBehaviour, IFindPlayer
{
    private PlayerInputCustom input;
    private SceneControl sceneControl;
    private PlayerStats playerStats;

    // Cheat variables
    private float timePressed;
    private ButtonPressed itemCheatButtonPressed;
    private ButtonPressed infiniteHealthButtonPressed;
    private IEnumerator itemCheatCoroutine;
    private IEnumerator infiniteHealthCheatCoroutine;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        sceneControl = FindObjectOfType<SceneControl>();
        playerStats = FindObjectOfType<PlayerStats>();
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

        if (itemCheatCoroutine != null) StopCoroutine(itemCheatCoroutine);
        if (infiniteHealthCheatCoroutine != null) StopCoroutine(infiniteHealthCheatCoroutine);
        itemCheatCoroutine = null;
        infiniteHealthCheatCoroutine = null;
    }

    private void Start()
    {
        timePressed = 0;
        itemCheatButtonPressed = ButtonPressed.Default;
        infiniteHealthButtonPressed = ButtonPressed.Default;
        itemCheatCoroutine = null;
        infiniteHealthCheatCoroutine = null;
    }

    /// <summary>
    /// The player has 1 second to perform each action in order. If succeded,
    /// it will trigger a cheat.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator ItemCheatCoroutine()
    {
        float timeToPressNext = 1;
        byte value = 0;
        while (value == 0 && Time.time - timePressed < timeToPressNext)
        {
            if (itemCheatButtonPressed != ButtonPressed.ItemLeft &&
                itemCheatButtonPressed != ButtonPressed.Block) 
                StopCoroutine(itemCheatCoroutine);

            if (itemCheatButtonPressed == ButtonPressed.Block) value = 1;
            yield return null;
        }

        while (value == 1 && Time.time - timePressed < timeToPressNext)
        {
            if (itemCheatButtonPressed != ButtonPressed.Block &&
                itemCheatButtonPressed != ButtonPressed.Attack)
                StopCoroutine(itemCheatCoroutine);

            if (itemCheatButtonPressed == ButtonPressed.Attack) value = 2;
            yield return null;
        }

        while (value == 2 && Time.time - timePressed < timeToPressNext)
        {
            if (itemCheatButtonPressed != ButtonPressed.Attack &&
                itemCheatButtonPressed != ButtonPressed.ItemRight)
                StopCoroutine(itemCheatCoroutine);

            if (itemCheatButtonPressed == ButtonPressed.ItemRight) value = 3;
            yield return null;
        }

        while (value == 3 && Time.time - timePressed < timeToPressNext)
        {
            if (itemCheatButtonPressed != ButtonPressed.ItemRight &&
                itemCheatButtonPressed != ButtonPressed.Block)
                StopCoroutine(itemCheatCoroutine);

            if (itemCheatButtonPressed == ButtonPressed.Block) value = 4;
            yield return null;
        }

        while (value == 4 && Time.time - timePressed < timeToPressNext)
        {
            if (itemCheatButtonPressed != ButtonPressed.Block &&
                itemCheatButtonPressed != ButtonPressed.Attack)
                StopCoroutine(itemCheatCoroutine);

            if (itemCheatButtonPressed == ButtonPressed.Attack) value = 5;
            yield return null;
        }

        while (value == 5 && Time.time - timePressed < timeToPressNext)
        {
            if (itemCheatButtonPressed != ButtonPressed.Attack &&
                itemCheatButtonPressed != ButtonPressed.ItemRight)
                StopCoroutine(itemCheatCoroutine);

            if (itemCheatButtonPressed == ButtonPressed.ItemRight) value = 6;
            yield return null;
        }

        while (value == 6 && Time.time - timePressed < timeToPressNext)
        {
            if (itemCheatButtonPressed != ButtonPressed.ItemRight &&
                itemCheatButtonPressed != ButtonPressed.Roll)
                StopCoroutine(itemCheatCoroutine);

            if (itemCheatButtonPressed == ButtonPressed.Roll) value = 7;
            yield return null;
        }

        while (value == 7 && Time.time - timePressed < timeToPressNext)
        {
            playerStats.FirebombKunais += 10;
            playerStats.Kunais += 10;
            playerStats.SmokeGrenades += 10;
            playerStats.HealthFlasks += 10;
            FindObjectOfType<ItemUIParent>().UpdateAllItemUI();
            break;
        }
    }

    /// <summary>
    /// The player has 1 second to perform each action in order. If succeded,
    /// it will trigger a cheat.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator InfiniteHealthCheatCoroutine()
    {
        float timeToPressNext = 1;
        byte value = 0;
        while (value == 0 && Time.time - timePressed < timeToPressNext)
        {
            if (infiniteHealthButtonPressed != ButtonPressed.ItemLeft &&
                infiniteHealthButtonPressed != ButtonPressed.ItemRight)
                StopCoroutine(infiniteHealthCheatCoroutine);

            if (infiniteHealthButtonPressed == ButtonPressed.ItemRight) value = 1;
            yield return null;
        }

        while (value == 1 && Time.time - timePressed < timeToPressNext)
        {
            if (infiniteHealthButtonPressed != ButtonPressed.ItemRight &&
                infiniteHealthButtonPressed != ButtonPressed.Attack)
                StopCoroutine(infiniteHealthCheatCoroutine);

            if (infiniteHealthButtonPressed == ButtonPressed.Attack) value = 2;
            yield return null;
        }

        while (value == 2 && Time.time - timePressed < timeToPressNext)
        {
            if (infiniteHealthButtonPressed != ButtonPressed.Attack &&
                infiniteHealthButtonPressed != ButtonPressed.ItemRight)
                StopCoroutine(infiniteHealthCheatCoroutine);

            if (infiniteHealthButtonPressed == ButtonPressed.ItemRight) value = 3;
            yield return null;
        }

        while (value == 3 && Time.time - timePressed < timeToPressNext)
        {
            if (infiniteHealthButtonPressed != ButtonPressed.ItemRight &&
                infiniteHealthButtonPressed != ButtonPressed.Block)
                StopCoroutine(infiniteHealthCheatCoroutine);

            if (infiniteHealthButtonPressed == ButtonPressed.Block) value = 4;
            yield return null;
        }

        while (value == 4 && Time.time - timePressed < timeToPressNext)
        {
            if (infiniteHealthButtonPressed != ButtonPressed.Block &&
                infiniteHealthButtonPressed != ButtonPressed.Attack)
                StopCoroutine(infiniteHealthCheatCoroutine);

            if (infiniteHealthButtonPressed == ButtonPressed.Attack) value = 5;
            yield return null;
        }

        while (value == 5 && Time.time - timePressed < timeToPressNext)
        {
            if (infiniteHealthButtonPressed != ButtonPressed.Attack &&
                infiniteHealthButtonPressed != ButtonPressed.Block)
                StopCoroutine(infiniteHealthCheatCoroutine);

            if (infiniteHealthButtonPressed == ButtonPressed.Block) value = 6;
            yield return null;
        }

        while (value == 6 && Time.time - timePressed < timeToPressNext)
        {
            if (infiniteHealthButtonPressed != ButtonPressed.Block &&
                infiniteHealthButtonPressed != ButtonPressed.Roll)
                StopCoroutine(infiniteHealthCheatCoroutine);

            if (infiniteHealthButtonPressed == ButtonPressed.Roll) value = 7;
            yield return null;
        }

        while (value == 7 && Time.time - timePressed < timeToPressNext)
        {
            playerStats.TookDamage += RestoreHP;
            break;
        }
    }

    /// <summary>
    /// What happens when infinite health cheat is done.
    /// </summary>
    private void RestoreHP()
    {
        if (playerStats != null)
            playerStats.Health += playerStats.MaxHealth;
    }

    private void Roll()
    {
        timePressed = Time.time;
        itemCheatButtonPressed = ButtonPressed.Roll;
        infiniteHealthButtonPressed = ButtonPressed.Roll;
    }

    private void Attack()
    {
        timePressed = Time.time;
        itemCheatButtonPressed = ButtonPressed.Attack;
        infiniteHealthButtonPressed = ButtonPressed.Attack;
    }

    private void ItemChange(Direction dir)
    {
        timePressed = Time.time;
        if (dir == Direction.Right)
        {
            itemCheatButtonPressed = ButtonPressed.ItemRight;
            infiniteHealthButtonPressed = ButtonPressed.ItemRight;
        }
        if (dir == Direction.Left)
        {
            if (itemCheatCoroutine != null) StopCoroutine(itemCheatCoroutine);
            itemCheatButtonPressed = ButtonPressed.ItemLeft;
            itemCheatCoroutine = ItemCheatCoroutine();
            StartCoroutine(itemCheatCoroutine);

            if (infiniteHealthCheatCoroutine != null) StopCoroutine(infiniteHealthCheatCoroutine);
            infiniteHealthButtonPressed = ButtonPressed.ItemLeft;
            infiniteHealthCheatCoroutine = InfiniteHealthCheatCoroutine();
            StartCoroutine(infiniteHealthCheatCoroutine);
        }
    }

    private void Block(bool condition)
    {
        timePressed = Time.time;
        if (condition)
        {
            itemCheatButtonPressed = ButtonPressed.Block;
            infiniteHealthButtonPressed = ButtonPressed.Block;
        }
    }

    public void FindPlayer() =>
        playerStats = FindObjectOfType<PlayerStats>();


    public void PlayerLost()
    {
        // Left blank on purpose
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
