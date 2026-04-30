using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace StackUI.Demo.Sync
{
    public class Sync : MonoBehaviour
    {
        // Start is called before the first frame update
        private async void Start()
        {
            Navigation.AddTableWithAsync<SyncPresenter>("customLoader",false,CustomLoader,CustomAssetRemoveHandler);

            Debug.Log("开始加载页面");
            await Navigation.PushAsync<SyncPresenter>();

            await Task.Delay(1000);
            Debug.Log("开始销毁页面");
            await Navigation.ClearAsync();

            Debug.Log("页面销毁完成");
        }

        private async Task<GameObject> CustomLoader(string sourceName)
        {
            
            await Task.Delay(1000);
            return new GameObject(sourceName).AddComponent<SyncView>().gameObject;
        }

        private async Task CustomAssetRemoveHandler(AssetRemoveHandlerArgs args)
        {
            await Task.Delay(1000);
            UnityEngine.GameObject.Destroy(args.asset);
        }
    }
}