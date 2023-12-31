# Stacklands Extra Search Mod

Lets you right click on a Card and search for all Ideas that use the Card as component.

- Right Click on a Card to add the Cardname into the search textfield.
- Press the button under the searchfield to toggle between the searchmodes.
  - **Title Search**, goes through the Ideas and displays all that contain the searched Term in the Title. (original Search)
  - **Description Search** (activated by Default), displays all Ideas that contain the searched Term in the Idea description.

Because it searches through the description, there might be some Idea results that dont use the searched Card as component but use the name in the flavour text description.

Should be compatible with the BetterMouseControls-Mod.

## Development

- uses the Stackland [mod-template](https://github.com/stacklandsdev/mod-template) as basis
- Build using `python build.py` in the mod root folder

## Links

- Github: https: //github.com/Jakhes/extra_search_mod
- Steam Workshop: https://steamcommunity.com/sharedfiles/filedetails/?id=3013114897
- Stacklands Modding Wiki: https://modding.stacklands.co/en/latest/index.html

## Changelog

- v0.0.3
  - Added a button to clear the searchfield
  - Added when right clicking on an empty field the searchfield gets cleared
- v0.0.2
  - Added a button to change the searchmode under the searchfield
  - F no longer changes the seachmode
  - Fix bug when searching with the searchfield and not the expected results are found
  - should be clearer what searchmode is active
- v0.0.1: Initial release
