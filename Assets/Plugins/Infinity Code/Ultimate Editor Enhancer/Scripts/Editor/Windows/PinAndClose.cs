/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;


namespace InfinityCode.UltimateEditorEnhancer.Windows
{
    public class PinAndClose : PopupWindow
    {
        public const int HEIGHT = 20;

        private static GUIContent _closeContent;
        private static GUIContent _tabContent;
        public bool closeOnLossFocus = true;

        private Action OnPin;
        private Action OnClose;

        private EditorWindow _targetWindow;
        private bool isDragging;
        private GUIContent labelContent;
        private Vector2 lastMousePosition;
        private Rect targetRect;
        private bool waitDragAndDropEnds;
        private bool waitRestoreAfterPicker;

        public static GUIContent closeContent
        {
            get
            {
                if (_closeContent == null) _closeContent = new GUIContent(Icons.closeWhite, "Close");
                return _closeContent;
            }
        }

        public static GUIContent tabContent
        {
            get
            {
                if (_tabContent == null) _tabContent = new GUIContent(Icons.pin, "To Tab Window"); 
                return _tabContent;
            }
        }

        public EditorWindow targetWindow
        {
            get { return _targetWindow; }
        }

        private void DrawLabel()
        {
            if (labelContent != null)
            {
                float maxWidth = position.width - 35;
                if (OnPin != null) maxWidth -= 20;
                
                GUILayout.Label(labelContent, EditorStyles.whiteLabel, GUILayout.MaxWidth(maxWidth));
            }
            EditorGUILayout.Space();

            ProcessLabelEvents(GUILayoutUtility.GetLastRect());
        }

        private bool InvokeClose()
        {
            if (OnClose != null) OnClose();
            Close();
            return true;
        }

        private void InvokePin()
        {
            OnPin();
            Close();
        }

        protected void OnDestroy()
        {
            OnPin = null;
            OnClose = null; 

            if (_targetWindow != null) _targetWindow.Close();
            _targetWindow = null;
        }

        protected override void OnGUI()
        {
            try
            {
                if (_targetWindow == null)
                {
                    Close();
                    return;
                }

                if (closeOnLossFocus && focusedWindow != this && focusedWindow != _targetWindow)
                {
                    if (waitRestoreAfterPicker && (focusedWindow == null || focusedWindow.GetType() != ObjectSelectorRef.type))
                    {
                        _targetWindow.Focus();
                        waitRestoreAfterPicker = false;
                    }
                    else
                    {
                        AutoSizePopupWindow w = _targetWindow as AutoSizePopupWindow;
                        if (w != null && !w.closeOnLossFocus)
                        {
                        }
                        else if (TryToClose()) return;
                    }
                }

                if (!isDragging && targetRect != _targetWindow.position && _targetWindow.position.position != Vector2.zero) SetRect(_targetWindow.position);

                base.OnGUI();

                EditorGUILayout.BeginHorizontal();

                DrawLabel();

                if (OnPin != null && GUILayout.Button(tabContent, Styles.transparentButton, GUILayout.Width(12), GUILayout.Height(12))) InvokePin();
                if (GUILayout.Button(closeContent, Styles.transparentButton, GUILayout.Width(12), GUILayout.Height(12))) InvokeClose();

                EditorGUILayout.EndHorizontal();
            }
            catch (ExitGUIException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                Log.Add(e);
            }

            Repaint();
        }

        private void OnTargetRectChanged(Rect rect)
        {
            SetRect(rect);
        }

        private void ProcessLabelEvents(Rect labelRect)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                Rect r = new Rect(0, 0, labelRect.xMax, 20);
                if (e.button == 0 && r.Contains(e.mousePosition) && GUIUtility.hotControl == 0)
                {
                    isDragging = true;
                    _targetWindow.Focus();
                    Focus();
                    lastMousePosition = GUIUtility.GUIToScreenPoint(e.mousePosition);
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                    e.Use();
                    GUIUtility.ExitGUI();
                }
            }
            else if (e.rawType == EventType.MouseUp)
            {
                if (isDragging && e.button == 0)
                {
                    isDragging = false;

                    AutoSizePopupWindow cw = _targetWindow as AutoSizePopupWindow;
                    if (cw != null) cw.wasMoved = true;

                    e.Use();
                    GUIUtility.hotControl = 0;
                    GUIUtility.ExitGUI();
                }
            }
            else if (e.type == EventType.MouseDrag)
            {
                if (isDragging)
                {
                    Vector2 mousePosition = GUIUtility.GUIToScreenPoint(e.mousePosition);
                    Vector2 delta = mousePosition - lastMousePosition;

                    Rect rect = position;
                    rect.position += delta;
                    position = rect;

                    rect = _targetWindow.position;
                    rect.position += delta;
                    _targetWindow.position = rect;

                    lastMousePosition = mousePosition;

                    e.Use();
                    GUIUtility.ExitGUI();
                }
            }
        }

        private void SetRect(Rect rect)
        {
            targetRect = rect;
            Vector2 size = new Vector2(rect.width, HEIGHT);
            Vector2 pos = rect.position - new Vector2(0, size.y);
            position = new Rect(pos, size);
        }

        public static PinAndClose Show(EditorWindow window, Rect inspectorRect, Action OnClose, string label)
        {
            return Show(window, inspectorRect, OnClose, null, label);
        }

        public static PinAndClose Show(EditorWindow window, Rect inspectorRect, Action OnClose, Action OnLock = null, string label = null)
        {
            PinAndClose wnd = CreateInstance<PinAndClose>();
            wnd.minSize = new Vector2(10, 10);
            wnd._targetWindow = window;
            wnd.OnClose = OnClose;
            wnd.OnPin = OnLock;
            wnd.SetRect(inspectorRect);
            if (!string.IsNullOrEmpty(label)) wnd.labelContent = new GUIContent(label);
            wnd.ShowPopup();
            wnd.Focus();
            window.Focus();

            ComponentWindow cw = window as ComponentWindow;
            if (cw != null)
            {
                cw.OnPositionChanged += wnd.OnTargetRectChanged; 
            }

            return wnd;
        }

        private bool TryToClose()
        {
            if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
            {
                waitDragAndDropEnds = true;
                return false;
            }

            if (waitDragAndDropEnds)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    waitDragAndDropEnds = false;
                    _targetWindow.Focus();
                }
                return false;
            }
            if (focusedWindow != null && focusedWindow.GetType() == ObjectSelectorRef.type)
            {
                waitRestoreAfterPicker = true;
                return false;
            }
            return InvokeClose();
        }

        public void UpdatePosition(Rect rect)
        {
            Rect r = position;
            Vector2 size = r.size;
            Vector2 pos = rect.position + new Vector2(rect.width, 0) - size;
            r.position = pos;
            r.size = size;
            position = r;
        }
    }
}