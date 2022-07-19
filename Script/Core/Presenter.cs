//Presenter基类
namespace StackUI
{
    //限制使用者调用 防止污染接口
    public interface IPresenter
    {
        /// <summary>
        /// 唯一性ID
        /// </summary>
        /// <value></value>
        string id { get; set; }
        /// <summary>
        /// 视图
        /// </summary>
        /// <value></value>
        View view { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="arg">参数</param>
        void OnInit(object arg);
        /// <summary>
        /// 隐藏
        /// </summary>
        void OnClose();
    }
    
    public class Presenter:IPresenter
    {
        public Presenter() { }
        private string _id;
        string IPresenter.id { get=>_id;set=>_id = value; }

        public View view { get;set; }

        private bool _enable;
        /// <summary>
        /// 当前可见状态
        /// </summary>
        /// <value></value>
        public bool enable { get => _enable;}

        public virtual void OnInit(object arg)
        {
            if(!_enable)
                view.OnShow();
            else
                UnityEngine.Debug.LogWarning("重复初始化,id:" + _id);
            _enable = true;
        }
        public virtual void OnClose()
        {
            if(_enable)
                view.OnClose();
            else
                UnityEngine.Debug.LogWarning("重复隐藏,id:" + _id);
            _enable = false;
        }
    }
}