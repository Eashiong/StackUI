using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StackUI;
namespace StackUI.Demo
{
    public class DialogWin : View
    {
        [Header("提示框")]
        public GameObject tipGroup;
        public UIText tipText;

        [Header("警告框")]
        public GameObject warningGroup;
        public Transform warningText;

        [Header("选择框")]
        public GameObject selectionGroup;
        public Transform selectionTitle;
        public Transform selectionText;
    }
    public class DialogPresenter : Presenter
    {   
        private DialogWin myWin;
        private GameObject cur;

        public override void OnInit(object arg)
        {
            base.OnInit(arg);
            myWin = view as DialogWin;

        }
        public void Tip(string msg,float duration = 0)
        {
            if(cur)
                cur.SetActive(false);
            myWin.tipText.text = msg;
            cur = myWin.tipGroup;
            cur.SetActive(true);
            
        }
    }
}