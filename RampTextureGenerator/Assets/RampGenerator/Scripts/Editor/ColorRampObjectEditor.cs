namespace RampTextureGenerator
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ColorRampObject))]
    public class ColorRampObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var component = target as ColorRampObject;
            if (component == null) return;

            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            EditorGUI.EndChangeCheck();

            if (EditorGUI.EndChangeCheck())
            {
                component.ApplyGradient();
                EditorUtility.SetDirty(component);
            }
        }

        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            var component = target as ColorRampObject;
            if (component == null) return;
            if (component.Texture == null) return;
            
            GUI.DrawTexture(r, component.Texture);
        }
    }
}