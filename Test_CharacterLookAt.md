# Description for chelange function Test_CharacterLookAt

## Test_CharacterLookAt 

**Test_CharacterLookAt** is setup function, for identify target actor object (LookAtTarget) and set EnableLookAtTarget for enable character rotation and enable OnTick event.

## Global variables

Added two global variables into blueprint:
- LookAtTarget (Actor) - Set in Test_CharacterLookAt; Get in OnTick event
- EnableLookAtTarget (Boolean) - Set in Test_CharacterLookAt; Get in OnTick event

## OnTick event

All rotation magic in character onTick event

1. Validate IsPlayerControlled and split the execution of the following rotation mechanics: SetControlRotation (for Player characters), SetActorRotation (for NPC characters)
2. Try cast to LookAtTarget as LyraCharacter, its check LookAtTarget is a LyraCharacter object. If LookAtTarget not is a LyraCharacter we can use simple rotation mechanic
3. If LookAtTarget is LyraCharacter we use target point from head bone for targeting on character head

