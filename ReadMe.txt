============================================================
POLARITY BREACH - CONTROLS
============================================================

Top-down twin-stick shooter.
Supports keyboard + mouse and Xbox controller.
You can switch between them at any time: the game
auto-detects the last device used for aiming.

------------------------------------------------------------
GAMEPLAY
------------------------------------------------------------

  ACTION              KEYBOARD & MOUSE          XBOX CONTROLLER
  ------------------  ------------------------  --------------------
  Move                WASD                      Left Stick
  Aim / Rotate        Mouse                     Right Stick
  Fire                Left Click                Right Trigger (RT)
  Charge Shot *       Hold Left Click, release  Hold RT, release
  Switch Polarity     Right Click               Left Trigger (LT)
  Dash *              Space                     B

  * Dash and Charge Shot are unlockable abilities.
    They do nothing until unlocked (togglable in the Cheat Menu).

------------------------------------------------------------
MENUS
------------------------------------------------------------

  ACTION              KEYBOARD & MOUSE          XBOX CONTROLLER
  ------------------  ------------------------  --------------------
  Pause / Resume      Esc                       Start
  Cheat Menu          F1                        Select

  The Pause Menu also has a Cheat Menu button that toggles
  the debug overlay.

------------------------------------------------------------
CORE MECHANIC - POLARITY
------------------------------------------------------------

  The player and every enemy are either BLACK or WHITE.

  - Opposite colors deal damage.
      A white projectile hurts a black enemy.

  - Matching colors do nothing.
      A white projectile passes through a white target.

  Switch your polarity on the fly to damage what's in front
  of you, and to shrug off incoming fire of your own color.

------------------------------------------------------------
CHEAT MENU (development only)
------------------------------------------------------------

  Opens with F1 or from the Pause Menu.
  Mouse only - the sliders can't be operated with a controller.
  The game keeps running while it's open.

  Contents:

    Player Stats   movement speed, max health, God Mode,
                   polarity switch cooldown
    Dash           unlock toggle, speed, duration, cooldown
    Normal Shot    fire delay, damage, projectile speed, knockback
    Charge Shot    unlock toggle, damage, speed, knockback,
                   charge time
    Wave Debug     kill all enemies (skips to the next wave)

------------------------------------------------------------
NOTES
------------------------------------------------------------

  - Dying restarts the current scene.
  - Low health triggers a blood overlay that pulses faster
    the closer you are to dying.

============================================================