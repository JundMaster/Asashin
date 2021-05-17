using System;

/// <summary>
/// If you add scenes, add to the Delete method on SpawnerController.
/// </summary>
[Flags]
public enum SceneEnum
{
    MainMenu = 0,
    Area1 = 1,
    Area2 = 2,
    Area3 = 3,
    Area4 = 4,
    Area5 = 5,
    Area6 = 6,







    ProgrammingTests = 7,
    TESTAREA = 8,
    EndOfDemo = 9,


    TutorialMovement,
    TutorialItemUse,
    TutorialWallHug,
    TutorialWalkAndHidden,
    TutorialWalkAndHiddenWithEnemy,

}
