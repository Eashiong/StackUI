using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StackUI.Tool
{
    public class UICodeGen
    {
        [MenuItem("Assets/StackUI/Gen Code", false, 0)]
        static void GenCode(MenuCommand menuCommand)
        {
             GameObject selectedObject = menuCommand.context as GameObject;
        }
        [MenuItem("Assets/StackUI/Gen Code", true)]
        private static bool ValidateAddToPrefabMenu()
        {
            GameObject selectedObject = Selection.activeObject as GameObject;

            if (selectedObject != null && PrefabUtility.IsPartOfPrefabAsset(selectedObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
