/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class EditorIconContents
    {
        private static GUIContent _alignVerticallyCenter;
        private static GUIContent _animationFirstKey;
        private static GUIContent _animationPrevKey;
        private static GUIContent _avatarPivot;
        private static GUIContent _consoleErrorIconSmall;
        private static GUIContent _dropdown;
        private static GUIContent _editIconSmall;
        private static GUIContent _folderIcon;
        private static GUIContent _folderOpenedIcon;
        private static GUIContent _gameObject;
        private static GUIContent _hierarchyWindow;
        private static GUIContent _inspectorWindow;
        private static GUIContent _linked;
        private static GUIContent _pauseButtonOn;
        private static GUIContent _prefabIcon;
        private static GUIContent _playButtonOn;
        private static GUIContent _popup;
        private static GUIContent _project;
        private static GUIContent _rectTransformBlueprint;
        private static GUIContent _saveActive;
        private static GUIContent _saveFromPlay;
        private static GUIContent _sceneAsset;
        private static GUIContent _sceneVisibilityHiddenHover;
        private static GUIContent _sceneVisibilityVisibleHover;
        private static GUIContent _settings;
        private static GUIContent _toolHandleGlobal;
        private static GUIContent _toolbarPlus;
        private static GUIContent _unlinked;
        private static GUIContent _verticalLayoutGroup;
        private static GUIContent _winBtnWinMax;

        public static GUIContent alignVerticallyCenter
        {
            get
            {
                if (_alignVerticallyCenter == null)
                {
                    _alignVerticallyCenter = EditorGUIUtility.IconContent("align_vertically_center");
                }
                return _alignVerticallyCenter;
            }
        }

        public static GUIContent animationFirstKey
        {
            get
            {
                if (_animationFirstKey == null)
                {
                    _animationFirstKey = EditorGUIUtility.IconContent("Animation.FirstKey");
                }
                return _animationFirstKey;
            }
        }

        public static GUIContent animationPrevKey
        {
            get
            {
                if (_animationPrevKey == null)
                {
                    _animationPrevKey = EditorGUIUtility.IconContent("Animation.PrevKey");
                }
                return _animationPrevKey;
            }
        }

        public static GUIContent avatarPivot
        {
            get
            {
                if (_avatarPivot == null)
                {
                    _avatarPivot = EditorGUIUtility.IconContent("AvatarPivot");
                }
                return _avatarPivot;
            }
        }

        public static GUIContent consoleErrorIconSmall
        {
            get
            {
                if (_consoleErrorIconSmall == null)
                {
                    _consoleErrorIconSmall = EditorGUIUtility.IconContent("console.erroricon.sml");
                }
                return _consoleErrorIconSmall;
            }
        }

        public static GUIContent dropdown
        {
            get
            {
                if (_dropdown == null)
                {
                    _dropdown = EditorGUIUtility.IconContent("icon dropdown");
                }
                return _dropdown;
            }
        }

        public static GUIContent editIcon
        {
            get
            {
                if (_editIconSmall == null)
                {
                    _editIconSmall = EditorGUIUtility.IconContent("editicon.sml");
                }
                return _editIconSmall;
            }
        }

        public static GUIContent folder
        {
            get
            {
                if (_folderIcon == null)
                {
                    _folderIcon = EditorGUIUtility.IconContent("Folder Icon");
                }
                return _folderIcon;
            }
        }

        public static GUIContent folderOpened
        {
            get
            {
                if (_folderOpenedIcon == null)
                {
                    _folderOpenedIcon = EditorGUIUtility.IconContent("FolderOpened Icon");
                }
                return _folderOpenedIcon;
            }
        }

        public static GUIContent gameObject
        {
            get
            {
                if (_gameObject == null)
                {
                    _gameObject = EditorGUIUtility.IconContent("GameObject Icon");
                }
                return _gameObject;
                
            }
        }

        public static GUIContent hierarchyWindow
        {
            get
            {
                if (_hierarchyWindow == null)
                {
                    _hierarchyWindow = EditorGUIUtility.IconContent("UnityEditor.HierarchyWindow");
                }
                return _hierarchyWindow;
            }
        }

        public static GUIContent inspectorWindow
        {
            get
            {
                if (_inspectorWindow == null)
                {
                    _inspectorWindow = EditorGUIUtility.IconContent("UnityEditor.InspectorWindow");
                }
                return _inspectorWindow;
            }
        }

        public static GUIContent linked
        {
            get
            {
                if (_linked == null)
                {
                    _linked = EditorGUIUtility.IconContent("Linked");
                }
                return _linked;
            }
        }

        public static GUIContent pauseButtonOn
        {
            get
            {
                if (_pauseButtonOn == null)
                {
                    _pauseButtonOn = EditorGUIUtility.IconContent("d_PauseButton On");
                }
                return _pauseButtonOn;
            }
        }

        public static GUIContent prefab
        {
            get
            {
                if (_prefabIcon == null)
                {
                    _prefabIcon = EditorGUIUtility.IconContent("Prefab Icon");
                }
                return _prefabIcon;
            }
        }

        public static GUIContent playButtonOn
        {
            get
            {
                if (_playButtonOn == null)
                {
                    _playButtonOn = EditorGUIUtility.IconContent("d_PlayButton On");
                }
                return _playButtonOn;
            }
        }

        public static GUIContent popup
        {
            get
            {
                if (_popup == null)
                {
                    _popup = EditorGUIUtility.IconContent("_Popup");
                }
                return _popup;
            }
        }

        public static GUIContent project
        {
            get
            {
                if (_project == null)
                {
                    _project = EditorGUIUtility.IconContent("Project");
                }
                return _project;
            }
        }

        public static GUIContent rectTransformBlueprint
        {
            get
            {
                if (_rectTransformBlueprint == null)
                {
                    _rectTransformBlueprint = EditorGUIUtility.IconContent("RectTransformBlueprint");
                }
                return _rectTransformBlueprint;
            }
        }

        public static GUIContent saveActive
        {
            get
            {
                if (_saveActive == null) _saveActive = EditorGUIUtility.IconContent("SaveActive");
                return _saveActive;
            }
        }

        public static GUIContent saveFromPlay
        {
            get
            {
                if (_saveFromPlay == null) _saveFromPlay = EditorGUIUtility.IconContent("SaveFromPlay");
                return _saveFromPlay;
            }
        }

        public static GUIContent sceneAsset
        {
            get
            {
                if (_sceneAsset == null) _sceneAsset = EditorGUIUtility.IconContent("SceneAsset Icon");
                return _sceneAsset;
            }
        }

        public static GUIContent sceneVisibilityHiddenHover
        {
            get
            {
                if (_sceneVisibilityHiddenHover == null)
                {
                    _sceneVisibilityHiddenHover = EditorGUIUtility.IconContent("scenevis_hidden_hover");
                }
                return _sceneVisibilityHiddenHover;
            }
        }

        public static GUIContent sceneVisibilityVisibleHover
        {
            get
            {
                if (_sceneVisibilityVisibleHover == null)
                {
                    _sceneVisibilityVisibleHover = EditorGUIUtility.IconContent("scenevis_visible_hover");
                }
                return _sceneVisibilityVisibleHover;
            }
        }

        public static GUIContent settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = EditorGUIUtility.IconContent("Settings");
                }
                return _settings;
            }
        }

        public static GUIContent toolHandleGlobal {
            get
            {
                if (_toolHandleGlobal == null)
                {
                    _toolHandleGlobal = EditorGUIUtility.IconContent("ToolHandleGlobal");
                }
                return _toolHandleGlobal;
            }
        }

        public static GUIContent toolbarPlus
        {
            get
            {
                if (_toolbarPlus == null)
                {
                    _toolbarPlus = EditorGUIUtility.IconContent("Toolbar Plus");
                }
                return _toolbarPlus;
            }
        }

        public static GUIContent unlinked
        {
            get
            {
                if (_unlinked == null)
                {
                    _unlinked = EditorGUIUtility.IconContent("Unlinked");
                }
                return _unlinked;
            }
        }

        public static GUIContent verticalLayoutGroup
        {
            get
            {
                if (_verticalLayoutGroup == null)
                {
                    _verticalLayoutGroup = EditorGUIUtility.IconContent("VerticalLayoutGroup Icon");
                }
                return _verticalLayoutGroup;
            }
        }

        public static GUIContent winBtnWinMax
        {
            get
            {
                if (_winBtnWinMax == null)
                {
                    _winBtnWinMax = EditorGUIUtility.IconContent("winbtn_win_max");
                }
                return _winBtnWinMax;
            }
        }
    }
}