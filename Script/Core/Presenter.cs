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

        //资源加载完成时
        void OnAssetLoaded();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="arg">参数</param>
        void OnInit(object arg);
        
        //重复初始化
        void OnReInit(object arg);

        /// <summary>
        /// 隐藏
        /// </summary>
        void OnClose();

        void OnDispose();

        bool isActive { get; set; }
    }
    
    public class Presenter:IPresenter
    {
        public Presenter() { }
        private string _id;
        string IPresenter.id { get=>_id;set=>_id = value; }
        protected string ID { get=> _id;}

        public View view { get;set; }

        private bool _isActive;
        bool IPresenter.isActive { get=>_isActive;set=>_isActive = value; }
        public virtual void OnAssetLoaded()
        {
            
        }
        public virtual void OnInit(object arg)
        {
            view.OnShow();
            _isActive = true;
        }
        public virtual void OnReInit(object arg)
        {
        }
        public virtual void OnClose()
        {
            view.OnClose();
            _isActive = false;
        }

        public virtual void OnDispose()
        {
            
        }
    }
}