using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace StackUI.Tool
{
    [System.Serializable]
    public class UISettingData 
    {
        private static string dataFilePath = "Tools/UISettingData.json";


        public string codeGenPath;
        public bool createSubDir = false;
        public string simpleUITemplatePath = "Assets/Tools/Template/SimpleUIView";
        public string pageTemplatePath = "Assets/Tools/Template/PageView";

        public string popupTemplatePath = "Assets/Tools/Template/PopupView";
        public string customTemplatePath = "Assets/Tools/Template/CustomView";



        public static UISettingData _currentData;
        public static UISettingData CurrentData
        {
            get
            {
                if(_currentData == null)
                    _currentData = Load();
                return _currentData;
            }
            set
            {
                _currentData = value;
            }
        }

        public static UISettingData Load()
        {
            CheckFile();

            string json = File.ReadAllText(Path.Combine(Application.dataPath,dataFilePath));
            return JsonUtility.FromJson<UISettingData>(json);
            
            
        }
        public void Save()
        {
            CheckFile();
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(Path.Combine(Application.dataPath,dataFilePath),json);
            CurrentData = this;
            AssetDatabase.Refresh();
        }
        public static void CheckFile()
        {
            string file = Path.Combine(Application.dataPath,dataFilePath);
            string path = Path.GetDirectoryName(file);
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if(!File.Exists(file))
            {
                
                string json = JsonUtility.ToJson(new UISettingData());
                File.WriteAllText(file,json);
            }
        }

        
    }
}