# Room8 Test Task
**Shooter Game Base Mechanics – Test Suite**

## Summary

This test suite is designed to validate the core gameplay mechanics of a shooter game. It covers:
- player movement
- jumping
- crouching
- firing
- damage reception

---

## Table of Test Cases

| Test Case Id | Mechanics | Description | Steps | Expected Result |
| --- | --- | --- | --- | --- |
| TC01 | Player Move | Verify that the player character can move in all directions | Start game, use movement controls: W/A/S/D  | Player moves left, right, forward, and backward smoothly |
| TC02 | Player Jump | Few validates for player can jump | Press jump action key; Press jump and try move forward| Player jump and movment can't interrup jump action |
| TC03 | Player Crouch | Few validates for player in crouch state | Press crouch action key; Try move in crouch; Jump in crouch | Player crouches and movement doesn't interrupt crouch state; Jump interrupt crouch state |
| TC04 | Player Fire | Validates if player can fire their weapon and hit a target | Aim a target press fire button | Weapon fires, weapon ammo count is decreased, target recieve damage |
| TC05 | Player Get Damage | Confirm that the player receives damage | Enemy attack player by weapon, by melee attack, player use grenade | Player health decreases, melee attack instakill player|

---

# Helpers Functions Description

## Changes for default Lyra project:

- Test using map: /ShooterMaps/Maps/L_FiringRange_WP
- Disable the "Allow Player Bots to Attack" option in section of "Lyra Developer Settings". (Menu "Edit->Editor Preferences...")

##  Helpers Functions

All helpers function created in modded Lyra blueprint for default character: [Character_Default](https://github.com/C0mbatHealer/Room8_TestTask/blob/master/TestSuitBaseShooterMechanics/ProjectsMods/Content/Characters/)

| Function Name | Arguments| Return | Description |
| --- | --- | --- | --- |
| Test_CharacterLookAt| TargetName (string) | | Set LookAtTarget (Actor) for looking, and activate OnTick event for rotation camera (for player) or pawn (for bots) to target, if target is a Lyracharacter target point is a head bone |
| Test_DisableLookAt | | |Disable look at target, disable OnTick event (for optimization, default character don't have active OnTick event) |
| Test_DrawCharacterDebugInfo | 0 - disable; 1 - enable | | Draw on screen useful debug info for creating tests |
| Test_GetJumpingCurrentCount | | Return JumpCurrentCount (int32) | Helper for read character property JumpCurrentCount for validate player jump |
| Test_GetVelocity | | Return current character velocity (Vector3) | Helper for check player movement direction |
| Test_DrawText | Text (string), Duration in sec (float) | | Helper for draw text on screen. Can be execute from ingame console (best solution) |
| Test_GetCharacterTeamID | | Return character team id (int32)| Helper for find character team. solution for identifying enemy character for tests |
| Test_UseAbilityFireWeapon | | | Helper for use Weapon Fire action by bots |
| Test_GetCharacterHealth | | Return current health count for character (double) | Helper for tests with validating damage |
| Test_GetCharacterMaxHealth | | Return max health count of character (double) | Helper for tests with validating damage |
| Test_GetCharacterWeaponAmmoCount | | Return current ammo count for character active weapon (int32) | Helper for tests with validating weapons states |
| Test_UseAbilityMelee | | | Helper for use Melee action by bots |
| FindActorByName | ActorName (string) | Actor (Actor Object Ref) | Blueprint Pure Function. Helper for other blueprint functions |
