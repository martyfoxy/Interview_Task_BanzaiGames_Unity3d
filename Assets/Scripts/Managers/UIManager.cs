using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера UI
    /// </summary>
    [CreateAssetMenu(fileName = "UI Manager", menuName = "Game Manager/UI Manager")]
    public class UIManager : BaseManager, IAwake, IUpdate
    {
        
        public void OnAwake()
        {
            UpdateManager.Register(this);
        }

        public void OnUpdate()
        {
            //HealthBarScript.SetValue(HPVariable.Value);
        }
    }
}
