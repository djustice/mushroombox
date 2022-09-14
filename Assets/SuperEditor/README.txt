-------------------------------------
SuperEditor
Copyright © 2021, Vik
-------------------------------------

Super Editor is a versatile Unity editor extension that includes a powerful, easy-to-use built-in IDE, a Hieararchy enhancements, and a Favorites enhancements

Unity's built-in IDE is a intelligent code window for C#, USS, UXML, Shader, Text, Prefab, Scene, and any text Asset, it contains code Automatic completion, keyword searching and replacing, custom themes, and so on. You don't need to switch visual studio or VS Code anymore, which can significantly simplify the development process.

Hieararchy enhancements is an editor extension that adds some often used functions to the
hierarchy window, which can significantly simplify the development process.

Favorites enhancements is a favorites panel for Unity3D to list all your favorites assets or scene gameobjects in your project, which can significantly simplify the development process.


0. SHORTCUTS and mouse functions

(assume Cmd instead of Ctrl on Mac OS if not noted otherwise)

(a) Cursor navigation (all these clear the selection)

Arrow keys - caret navigation
Mouse click – sets caret position
Ctrl+Left and Ctrl+Right arrow key - Moves the cursor to the previous or next word on Windows or sub-word on OS Alt+Left and Alt+Right arrow key - Moves the cursor to the previous or next word on Mac OS
Ctrl+Up and Ctrl+Down arrow key - Scrolls the code view by one line
PageUp and PageDown - move the cursor by one page
End - moves the cursor to the end of a line
Home - moves the cursor before the first non-whitespace character on a line or at the beginning of the line
Ctrl+Home - move the cursor at the beginning of the whole script
Ctrl+End - move the cursor at the end of the whole script


(b) Cursor navigation & history

Ctrl+G (Cmd-L on Mac OS) - Go To Line
Alt+Left arrow key (Alt-Cmd-Left arrow on Mac OS) - Go Back
Alt+Right arrow key (Alt-Cmd-Right arrow on Mac OS) - Go Forward
Alt+M (Ctrl-M on Mac OS) - opens the last code navigation breadcrumb button (C# files only)

(c) Selections

Shift + any cursor navigation shortcut or mouse click - selects text or alters the selection
Mouse double-click or Ctrl+mouse click - selects the whole word
Mouse click and drag - mouse drag selection
Mouse double-click and drag or Ctrl+mouse click and drag - mouse drag selection for whole words
Mouse click on line numbers - line select
Mouse tripple-click - line select
Mouse click and drag on line numbers - mouse drag line selection
Mouse tripple-click and drag - mouse drag lines selection
Ctrl+A, or Edit->Select All from the main menu, or Select All from the context menu - selects the whole content of a file
Escape - clears the selection

(d) Editing

Typing text – inserts text 
Backspace – deletes selected text or the character before of the caret
Delete – deletes selected text or the character after the caret
Ctrl+Backspace – deletes selected text or the word or part of it before the caret
Ctrl+Delete – deletes selected text or the word or part of it after the caret

(e) Cut, Copy, Paste, Duplicate

Ctrl+X, or Edit->Cut from the main menu, or Cut from the context menu - cuts the selected text
Ctrl+C, or Edit->Copy from the main menu, or Copy from the context menu - copies the selected text
Ctrl+V, or Edit->Paste from the main menu, or Paste from the context menu – pastes the clipboard text (replacing the selected text, if any)
Mouse dragging selected text – moves the selected text
Ctrl+mouse dragging selected text – duplicates the selected text
Ctrl+D, or Edit->Duplicate (and also Alt-Cmd-Down on Mac OS) - duplicates the line at cursor or selected lines

(f) More editing

Ctrl+Z (Control-Z on Mac OS) – Undo
Shift+Ctrl+Z (Shift-Control-Z on Mac OS) – Redo
Alt+Up/Down arrow keys - moves current line or selected lines up or down and re-indents
Ctrl+K or Ctrl+/ - toggles on or off code comments on a single line or selected lines (except while editing text assets, of course)
Ctrl+[ - decreases indentation of a single line or more selected lines
Ctrl+] - increases indentation of a single line or more selected lines
Tab – inserts a Tab character if nothing is selected, otherwise increases indentation
Shift+Tab – deletes the Tab character before caret, or otherwise decreases indentation

(g) Automatic completion

Escape or Ctrl+Space - opens aut-completion popup window
Up/Down arrow keys - select word to complete automatically
Typing - filters the completion list
Enter, Tab, or a non-alphanumeric character - accepts selected completion
Escape - cancels completion list popup window

(h) Searching & Replacing

F12 (F12 or Cmd-Y on Mac OS) - Go To Definition
Shift+F12 (Shift-F12 or Shift-Cmd-Y on Mac OS) - Find All References
Shift+Ctrl+F - Find in Files
Shift+Ctrl+H - Replace in Files
Shift+Ctrl+Up – finds the previous occurrence of the word at caret or of the selected text
Shift+Ctrl+Down – finds the next occurrence of the word at caret or of the selected text
Ctrl+F – sets the keyboard input to the search field in the toolbar
Escape inside the search field – sets the keyboard focus back to the code editor
Enter inside the search field – finds the next occurrence of the searched text
Shift+Enter inside the search field – finds the previous occurrence of the searched text
Up/Down Arrow keys and arrow buttons inside the search field – find previous/next occurrence of the searched text
F3, or Ctrl+G – finds next occurrence of the searched text
Shift+F3, or Shift+Ctrl+G – finds the previous occurrence of the searched text

(i) CodeWindow tabs related

Ctrl+F4 or Ctrl+W (Cmd-F4 or Cmd-Shift-W on Mac OS) – closes the active CodeWindow tab
Ctrl+Tab and Shift+Ctrl+Tab (Alt-Tab and Shift-Alt-Tab on Mac OS) - Tab history navigation (hold Ctrl to see full history and then navigate with arrow keys)
Ctrl+PageUp or Ctrl+Alt+Left arrow key (Cmd-PageUp or Ctrl-Alt-Left on Mac OS) – activates the first CodeWindow tab to the left of the current one
Ctrl+PageDown or Ctrl+Alt+Right arrow key (Cmd-PageDown or Ctrl-Alt-Right on Mac OS) – activates the first CodeWindow tab to the right of the current one
Shift+Ctrl+Alt+Left (same on Mac OS) – moves the active CodeWindow tab one position to the left in a dock group
Shift+Ctrl+Alt+Right (same on Mac OS) – moves the active CodeWindow tab one position to the right in a dock group

(j) Font size

Shift+Ctrl+Minus, Ctrl+Plus, Shift+Ctrl+Equals sign, Ctrl+Mouse Wheel, and magnify touchpad gestures – increase and decrease font size

(k) Opening & Saving, New Tabs, External IDE

Shift+Ctrl+O (Shift-Control-O on Mac OS) - open file
Shift+Alt+O (Cmd-Alt-O on Mac OS) - open file from any folder
Ctrl+S (Cmd-S on Mac OS) - saves changes
Ctrl+R (Cmd-Alt-R on Mac OS) - saves all modified files
Ctrl+T – opens the same file in a new tab
Ctrl+Enter, or Open at Line from the context menu - opens the script in the external editor at the same line where the caret is



1. Description


Code Window is the latest HUGE improvement editor extension for Unity which you've been using so far only as a quick editor for your C#, USS, UXML, Shaders, and text assets.

Built with performance in mind since the beginning while keeping the quality at the highest standards, it was growing with new features, to finally become the fastest and most comfortable solution for programmers to write code for Unity.

Code Window opens files instantly and you can start editing them immediately! No more waiting for external editors to start, load the project, and then your script, and by the time all that finishes you'll often forget what your intention was initially. :p Code Window will keep you stay focused on programming instead of fighting with code editing tools. :)

Using its greatest advantage, being integrated inside Unity, Code Window is definitely the greatest tool available for improving programmer’s workflow! Accessibility combined with quick iteration cycles achieved with Code Window makes this extension the preferred IDE for many Unity programmers including myself, the author. As a result of that, the development of this magnificent tool was done in it, making Code Window the first Unity extension programmed in itself! ;)



2. Using Code Window

CodeWindow windows can be arranged, docked, undocked, maximized, or closed as any other regular Unity window.

CodeWindow folder can be moved anywhere in Assets folder

You can open additional script, shader, or text assets into a particular tab group by dragging assets from the Project window to one of the existing Code Windows. You can also drag Materials to open the shader used in them directly. Similarly you can drag GameObjects from the current scene or Prefabs to open their components based on MonoBehaviour scripts. Component scripts can also be easily accessed from the component’s wrench menus in the Inspector. Similarly shaders used in materials can be opened from the wrench menu.

Code Window can (optionally) open a script, shader, or text assets on double-click in the Project window. These options are located in the wrench menu of any of the CodeWindow views. Pressing Enter (Command-Down Arrow on Mac) on the selected asset in the Project window would still open them in the external IDE, so you still have that option available.

Assets can be opened in multiple Code Windows at the same time showing different parts of their content with different caret position and selection. Editing is synchronized. Changes made in one of these tabs are immediately visible in the other Code Windows. The Undo/Redo buffer is also shared so that changes made in one Window can be reverted in another one, for example.

Code Window holds unlimited size of its Undo/Redo buffers for each edited asset independently. These buffers are also independent of the Unity’s built-in undo buffer, so that changes in scripts don’t interfere with the changes made in the rest of Unity and each can be reverted or repeated independently. There are Undo and Redo toolbar buttons in each Code Window, and there are keyboard shortcuts associated with them – Ctrl+Z and Shift+Ctrl+Z on Windows and Control-Z and Shift-Control-Z on Mac OS for Undo and Redo respectively.

Code Window handles very wide range of mouse and keyboard input actions, providing the users with an experience similar to other modern IDE’s. Cursor navigation, selecting words, selecting lines, Cut/Copy/Paste, drag-selections, dragging selected text to move or copy, search functionality, quick search, are only some of the features implemented in Code Window. See the Shortcuts section for a more complete list of included features.


3. Saving and Reloading

Code Window keeps track of all changes made using it. Asterisk signs in Code Window’s titles indicate unsaved changed. You can save the changes with the Save toolbar button or with the keyboard shortcut Ctrl+S on Windows or Control-S on Mac OS. All modified assets will be automatically saved on entering Editor Game Mode.

Navigating away from a modified file when closing a Code Window  will warn you and ask if you want to save the changes made in the asset, discard them, or keep them in memory and continue editing later. You’ll also get similar warnings on quitting the Editor or loading another Unity project, only without the option to keep the changes in memory, of course.

After saving the changes made Unity will compile them as usual and display all compile warnings and errors in the Console.

Reimported scripts, either manually or automatically as a result of external changes, will be updated in CodeWindow views automatically. Well, unless those scripts have been modified and not saved before reimporting, in which case Code Window gives you a warning and asks which version to keep.




4. Support, Bugs, Requests, and Feedback

Feel free to contact us at https://github.com/UnitySuperEditor/SuperEditor for support, bug reports, suggestions, feedback, etc...


* Follow me on github https://github.com/UnitySuperEditor

