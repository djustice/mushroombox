﻿/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using InfinityCode.UltimateEditorEnhancer.EditorMenus.Actions;
using InfinityCode.UltimateEditorEnhancer.Windows;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.EditorMenus.Layouts
{
    public class ActionsLayout : MainLayoutItem<ActionsLayout, ActionItem>
    {
        protected override void CalculateRect(ref Vector2 position, ref Vector2 offset, ref bool flipHorizontal, ref bool flipVertical)
        {
            rect = new Rect(position, Vector2.zero);
            foreach (ActionItem item in items)
            {
                if (!item.isActive) continue;

                Vector2 size = item.size;
                rect.width = Mathf.Max(size.x, rect.width);
                rect.height += size.y;
            }

            rect.height += GUI.skin.button.margin.top;

            Resolution resolution = Screen.currentResolution;
            int width = resolution.width;

            if (rect.x % width < 11 + rect.width) offset.x = rect.width + 11 - rect.x % width;
#if !UNITY_EDITOR_OSX
            else if (flipHorizontal && rect.xMin % width + rect.width > width - 11) offset.x = width - 11 - rect.xMin % width - rect.width;
#endif

#if !UNITY_EDITOR_OSX
            if (rect.yMax > resolution.height - 40)
            {
                flipVertical = true;
                offset.y = resolution.height - rect.yMax + 60;
            }
#endif

            if (position.y < 10) offset.y = Mathf.Max(offset.y, 10);
        }

        protected override bool CheckPrefs()
        {
            return Prefs.actions;
        }

        public override void OnGUI()
        {
            if (items == null) return;

            foreach (ActionItem item in items)
            {
                if (item.isActive)
                {
                    try
                    {
                        item.Draw();
                    }
                    catch (Exception e)
                    {
                        Log.Add(e);
                    }
                }
            }
        }

        public override void SetPosition(Vector2 position, Vector2 offset, bool flipHorizontal, bool flipVertical)
        {
            int ox = -10;
            int oy = -10;
            rect.position = position + offset + new Vector2(ox - rect.width, oy);

            if (flipHorizontal) rect.x += rect.width - ox * 2;
            if (flipVertical) rect.y -= rect.height + oy * 2;
        }

        public override void Show()
        {
            wnd = LayoutWindow.Show(this, rect);
        }
    }
}