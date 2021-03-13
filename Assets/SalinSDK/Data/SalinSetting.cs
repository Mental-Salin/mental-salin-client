using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace SalinSDK
{
    public class SalinSetting
    {
        private static SalinData data;
        public static SalinData Data
        {
            get
            {
                if (data == null)
                {
                    data = (SalinData)Resources.Load(SalinConstants.dataSettingsFileName, typeof(SalinData));
                }
                return data;
            }
            private set
            {
                data = value;
            }
        }

#if UNITY_EDITOR
        public static void CreateDataSettings()
        {
            AssetDatabase.Refresh();
            SalinSetting.Data = (SalinData)Resources.Load(SalinConstants.dataSettingsFileName, typeof(SalinData));
            if (SalinSetting.Data != null)
            {
                return;
            }

            // if the project does not have SalinData yet, enable "Development Build" to use the Dev Region.
            EditorUserBuildSettings.development = true;

            // find out if SalinData can be instantiated (existing script check)
            ScriptableObject dataSettingTest = ScriptableObject.CreateInstance("SalinData");
            if (dataSettingTest == null)
            {
                Debug.LogError("missing settings script");
                return;
            }
            UnityEngine.Object.DestroyImmediate(dataSettingTest);

            // if still not loaded, create one
            if (SalinSetting.Data == null)
            {
                string _resourcesPath = SalinSetting.FindSalinAssetFolder();

                _resourcesPath += "Resources/";

                string dataSettingsAssetPath = _resourcesPath + SalinConstants.dataSettingsFileName + ".asset";
                string settingsPath = Path.GetDirectoryName(dataSettingsAssetPath);
                if (!Directory.Exists(settingsPath))
                {
                    Directory.CreateDirectory(settingsPath);
                    AssetDatabase.ImportAsset(settingsPath);
                }

                SalinSetting.data = (SalinData)ScriptableObject.CreateInstance("SalinData");
                if (SalinSetting.Data != null)
                {
                    AssetDatabase.CreateAsset(SalinSetting.Data, dataSettingsAssetPath);
                }
                else
                {
                    Debug.LogError("SalinSDK failed creating a settings file. ScriptableObject.CreateInstance(\"SalinData\") returned null. Will try again later.");
                }
            }
        }


        /// <summary>
        /// Finds the pun asset folder. Something like Assets/SalinSDK/Data/Resource
        /// </summary>
        /// <returns>The pun asset folder.</returns>
        public static string FindSalinAssetFolder()
        {
            string _thisPath = FindAssetPath("SalinData");
            string _SalinSdkFolderPath = string.Empty;

            string[] subdirectoryEntries = _thisPath.Split('/');
            foreach (string dir in subdirectoryEntries)
            {
                if (!string.IsNullOrEmpty(dir))
                {
                    _SalinSdkFolderPath += dir + "/";

                    if (string.Equals(dir, "Data"))
                    {
                        return _SalinSdkFolderPath;
                    }
                }
            }
            return "Assets/SalinSDK/Data/";
        }


        /// <summary>
        /// Finds the asset path base on its name or search query: https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html
        /// </summary>
        /// <returns>The asset path.</returns>
        /// <param name="asset">Asset.</param>
        public static string FindAssetPath(string asset)
        {
            string[] guids = AssetDatabase.FindAssets(asset, null);
            if (guids.Length != 1)
            {
                return string.Empty;
            }
            else
            {
                return AssetDatabase.GUIDToAssetPath(guids[0]);
            }
        }

#endif
    }
}