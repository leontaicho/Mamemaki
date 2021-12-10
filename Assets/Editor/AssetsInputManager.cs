using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
public class AssetsInputManager : AssetPostprocessor
{
    void OnPreprocessAsset()
    {
        // 最初にアセットをインポートするときに、プリセットを適用するように確認します
        if (assetImporter.importSettingsMissing)
        {
            // インポートされた現在のアセットフォルダーを取得します
            var path = Path.GetDirectoryName(assetPath);
            while (!string.IsNullOrEmpty(path))
            {
                // このフォルダー内のすべてのプリセットアセットを見つけます
                var presetGuids = AssetDatabase.FindAssets("t:Preset", new[] { path });
                foreach (var presetGuid in presetGuids)
                {
                    // サブフォルダー内のプリセットをテストしていないことを確認します
                    string presetPath = AssetDatabase.GUIDToAssetPath(presetGuid);
                    if (Path.GetDirectoryName(presetPath) == path)
                    {
                        // プリセットをロードし、インポーターに適用します
                        var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                        if (preset.ApplyTo(assetImporter))
                            return;
                    }
                }

                // 親フォルダーで再度行います
                path = Path.GetDirectoryName(path);
            }
        }
    }
}
