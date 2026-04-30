using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StackUI.Demo.Sync
{
    public class SyncView : View
    {

    }

    public class SyncPresenter : Presenter
    {
        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            Debug.Log($"{id} -> OnInit");
        }
    }
}