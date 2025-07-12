# Template for 3 issues:
## Issue 1: Puzzles template for Multiplayer and Singleplayer

### User story:
  GIVEN I choose single player or multiplayer mode
  WHEN the puzzle starts,
  THEN the same template is displayed in a separate window for all players.
  
### Bug report:
  Steps to Reproduce:
    Start the single player mode and open the puzzle.
    Start the two-player multiplayer mode and unlock the same puzzle.
Expected Behavior:
    The puzzle template should look and behave the same in both modes.
Actual Behavior:
    In multiplayer, the controls are shifted and look different than in single player.
    
### Technical Task
  Task Summary:
    Design and implement a puzzle template component that works in single and multiplayer modes.
Subtasks / Linked Issues:
  Create a React/Vue component for the template.
  Ensure that the mode (single/multi) is transferred to the parameters.
  Write CSS/styles for consistent display.

## Issue 2: Puzzles scripts — 10 types

### User story:
  GIVEN the players are in the same room,
  WHEN a new puzzle appears,
  THEN it is displayed in a synchronized window for both players.
  
### Bug report:
  Steps to Reproduce:
    Open a type X puzzle in multiplayer.
    One player performs an action in the puzzle window.
Expected Behavior:
    All actions should be instantly synchronized and displayed by the second player.
Actual Behavior:
    For the second player, the window remains in its original state until the page is reloaded.
    
### Technical Task
  Task Summary:
    Design and implement at least 10 types of puzzles in a separate window with full synchronization of actions between players.  
  Subtasks / Linked Issues:
  Write out the logic and scenarios for each type of puzzle.
  Make a separate React/Vue component “PuzzleWindow".
  Integrate WebSocket‑real-time synchronization of actions.

## Issue 3: Profile Window with Local Data

### User story:
  GIVEN I clicked on the Profile button,
  WHEN the profile window opens,
  THEN it shows the username and the last completed level (max. 3) from localStorage.

### Bug report:
  Steps to Reproduce:
    Complete at least one level in the game.
    Click the "Profile" button in the main menu.  
  Expected Behavior:
    The profile window should display the “name” and “lastCompletedLevel" values from localStorage.
  Actual Behavior:
    The fields remain empty or show default values.

### Technical Task
  Task Summary:
    Make a modal Profile window that, when opened, loads and displays the name and lastCompletedLevel (1-3) data from localStorage.
  Subtasks / Linked Issues:
  Add a button and open the modal when clicked.
  Implement reading the name and lastCompletedLevel keys from localStorage.
  Display the data in the templates of the modal window.  
