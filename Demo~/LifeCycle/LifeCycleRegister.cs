using System.Collections;
using UnityEngine;
using StackUI;

namespace StackUI.Demo
{
    public class LifeCycleRegister : MonoBehaviour
    {

        private IEnumerator Start()
        {
            Navigation.AddTable<LifeCyclePresenter01>("StackUIDemo/LifeCycle/LifeCycle01");
            Navigation.AddTable<LifeCyclePresenter02>("StackUIDemo/LifeCycle/LifeCycle02");

            Navigation.Push<LifeCyclePresenter01>("Hello LifeCyclePresenter01!");

            yield return new WaitForSeconds(2f);
            Navigation.Push<LifeCyclePresenter02>("Hello LifeCyclePresenter02!");

            yield return new WaitForSeconds(2f);
            Navigation.Pop("Come back to LifeCyclePresenter01!");


        }


    }
}
