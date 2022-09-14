/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.SceneTools.QuickAccessActions
{
    public abstract class QuickAccessAction
    {
        public abstract GUIContent content { get; }

        public virtual void Draw()
        {
            ButtonEvent buttonEvent = GUILayoutUtils.Button(content, QuickAccess.contentStyle, GUILayout.Width(QuickAccess.width), GUILayout.Height(QuickAccess.width));
            if (buttonEvent == ButtonEvent.click)
            {
                OnClick();
            }
        }

        public abstract void OnClick();

        public virtual void ResetContent()
        {
            
        }

        public virtual bool Validate()
        {
            return true;
        }
    }
}
