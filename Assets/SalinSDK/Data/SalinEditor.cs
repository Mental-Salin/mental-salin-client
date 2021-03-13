using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


# if UNITY_EDITOR
namespace SalinSDK
{
    [InitializeOnLoad]
    public class SalinEditor : Editor 
    {
        static SalinEditor()
        {
            EditorApplication.projectChanged += OnProjectChanged;
        }

        //[MenuItem("Assets/Create/TestAssetDataBase")]
        //static void CreateData()
        //{
        //    var asset = ScriptableObject.CreateInstance<SalinData>();
        //    var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        //    if(string.IsNullOrEmpty(path))
        //    {
        //        path = "Assets";
        //    }
        //    Debug.Log(path);

        //    AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(path + "/SalinData.asset"));
        //    Debug.Log(AssetDatabase.GenerateUniqueAssetPath(path + "/Data.asset"));
        //    AssetDatabase.SaveAssets(); //저장되지 않은 모든 에셋의 변경사항을 저장함.
        //    Selection.activeObject = asset;//생성한 에셋을 선택 상태로 만듭니다.
        //    asset.hideFlags = HideFlags.DontSave; //새로운 장면이 로드 될 때 파괴되지 않음.
        //}

        private static void OnProjectChanged()
        {
            if(SalinSetting.Data == null)
            {
                SalinSetting.CreateDataSettings();          
                if (SalinSetting.Data == null)
                {
                    Debug.LogError("CreateSettings() failed to create SalinSetting.Data.");
                    return;
                }
            }
        }

        private static void SaveSettings()
        {
            //실행중에 값을 변경하면 값이 저장이 되지 않고 날아가는데 그 값을 디스크에 저장해서 Asset값을 바꾸는 것
            EditorUtility.SetDirty(SalinSetting.Data);
        }
    }
}
#endif