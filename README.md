# EscapeTheSyllabus
A game designed to help students identify and comprehend important syllabus information and course learning objectives from their college professors.

# Release Notes version Escape The Syllabus 1.0
## NEW FEATURES
- New screens for
- Registering/logging-in users
- Selecting levels
- Continuing a previous level
- Viewing user stats
- Logging out
- Firebase database for storing and accessing user accounts
- Levels in a maze layout for players to explore
- Custom-made sprites for the player and enemy
- Collisions with enemies prompt users to answer questions
- Correct questions defeat the enemy
- Incorrect questions take users back to beginning of the level

## BUG FIXES
	None since last release

## KNOWN BUGS
- Back button returns to login/register screen rather than level select screen
- Player collision with walls could be more smooth and less jarring
- If user logs out and a new user signs in, they can access locked levels
- Can’t send Firebase data in csv file

# Install Guide Escape The Syllabus 1.0
## PRE-REQUISITES
- You must have at least Unity v.2018.3.2f1 installed.
- You must have at least Android Studio v.3.0 installed.
- Minimum Android API level 16 (for emulator).

## DEPENDENCIES
	None

## DOWNLOAD
	https://github.com/tbrownlow8/EscapeTheSyllabus

## BUILD
- No build necessary if running from Unity.
- To build for Android, open in Unity. Click ‘File’->’Build Settings’->’Build’. This will create an APK file.

## INSTALLATION
- Open APK file in Android Studio.

## RUNNING THE APPLICATION
- Click ‘Run’ in Android Studio and choose a device (Android Emulator or
connected device).

## TROUBLESHOOTING
- If the ‘Run’ button is not clickable, exit out of Android Studio and reopen. Make sure to open the APK file as “Profile or debug APK”.
