namespace RampTextureGenerator
{
    using UnityEditor;
    using UnityEngine;

    public static class MyStyles
    {
        [System.NonSerialized] public static readonly GUIStyle FooterLabel;
        [System.NonSerialized] public static readonly GUIStyle Button;

        [System.NonSerialized] public static readonly GUILayoutOption[] FooterLabelOptions;
        [System.NonSerialized] public static readonly GUILayoutOption[] ButtonOptions;

        static MyStyles()
        {
            // Footer Label
            FooterLabel = new GUIStyle(EditorStyles.label);
            FooterLabel.alignment = TextAnchor.MiddleRight;
            FooterLabelOptions = new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true)
            };

            // Button
            Button = new GUIStyle(GUI.skin.button);
            ButtonOptions = new GUILayoutOption[]
            {
                GUILayout.Height(30)
            };
        }
    }
}