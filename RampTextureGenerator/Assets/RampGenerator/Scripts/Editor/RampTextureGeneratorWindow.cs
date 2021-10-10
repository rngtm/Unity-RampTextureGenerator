using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;

namespace RampTextureGenerator
{
    using UnityEditor;

    public class RampTextureGeneratorWindow : EditorWindow
    {
        [SerializeField] private string _exportFolder = "RampGenerator/Textures/Ramp/";
        [SerializeField] private string _fileName = "Ramp";
        private string _textureName = "Ramp";
        private int textureHeight = 1;
        private int textureWidth = 256;

        [MenuItem("Tools/Ramp Texture Generator")]
        static void Open()
        {
            GetWindow<RampTextureGeneratorWindow>();
        }

        private string ExportPath =>
            Path.Combine(ConvertToAssetPath(_exportFolder), $"{_fileName}.asset");

        private void OnGUI()
        {
            CustomUI.TextFieldWithUndo(this,"Directory", ref _exportFolder);
            CustomUI.TextFieldWithUndo(this,"FileName", ref _fileName);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("Texture Export Path", ExportPath);
            EditorGUI.EndDisabledGroup();

            ExportButton();
        }

        private void ExportButton()
        {
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(_fileName) || string.IsNullOrEmpty(_exportFolder));
            if (GUILayout.Button("Create", MyStyles.Button, MyStyles.ButtonOptions))
            {
                Export(out var exportPath);
            }
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Gradientのテクスチャ出力
        /// </summary>
        /// <param name="exportPath"></param>
        private void Export(out string exportPath)
        {
            var folderPath = GetOrCreateFolder(_exportFolder);
            exportPath = Path.Combine(folderPath, $"{_fileName}.asset");
            exportPath = AssetDatabase.GenerateUniqueAssetPath(exportPath);

            // RampObject作成
            var rampObject = ScriptableObject.CreateInstance<ColorRampObject>();
            var gradient = rampObject.Gradient;
            AssetDatabase.CreateAsset(rampObject, exportPath);

            // Texture格納
            var texture = TextureGenerator.CreateTexture(gradient, textureWidth, textureHeight);
            texture.name = _textureName;
            rampObject.Initialize(texture);
            AssetDatabase.AddObjectToAsset(texture, rampObject);
            AssetDatabase.SaveAssets();
            
            EditorGUIUtility.PingObject(rampObject);
        }

        void DrawFooter()
        {
            GUILayout.FlexibleSpace();
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUILayout.LabelField(ExportPath, MyStyles.FooterLabel, MyStyles.FooterLabelOptions);
            }
        }

        /// <summary>
        /// フォルダの取得。フォルダが無かったらフォルダを作成
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetOrCreateFolder(string path)
        {
            string folderPath;
            if (!IsFolderExists(path))
            {
                folderPath = CreateFolder(path);
            }
            else
            {
                folderPath = ConvertToAssetPath(path);
            }

            return folderPath;
        }

        private bool IsFolderExists(string path)
        {
            return Directory.Exists(ConvertToFullPath(path));
        }

        /// <summary>
        /// フルパスに変換
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ConvertToFullPath(string path)
        {
            return Path.Combine(Application.dataPath, path);
        }

        /// <summary>
        /// Assetsで始まる形式のフォルダパスに変換
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ConvertToAssetPath(string path)
        {
            return Path.Combine("Assets", path);
        }

        /// <summary>
        /// フォルダ作成
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string CreateFolder(string path)
        {
            var folderPath = ConvertToAssetPath(path);
            Directory.CreateDirectory(folderPath);
            AssetDatabase.ImportAsset(folderPath);

            return folderPath;
        }
    }
}