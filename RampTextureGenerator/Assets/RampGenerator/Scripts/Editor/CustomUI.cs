using UnityEditor;

namespace RampTextureGenerator
{
    public static class CustomUI
    {
        public static void TextFieldWithUndo(EditorWindow window, string label, ref string text)
        {
            EditorGUI.BeginChangeCheck();
            var s = EditorGUILayout.TextField(label, text);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(window, "Change String");
                text = s;
            }

        }
    }
}