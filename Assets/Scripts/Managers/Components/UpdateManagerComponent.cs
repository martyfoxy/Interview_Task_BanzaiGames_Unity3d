using UnityEngine;

namespace Assets.Scripts.Managers.Components
{
    /// <summary>
    /// Фасадный класс, выполняющий все методы жизненного цикла MonoBehaviour для менеджеров
    /// </summary>
    public class UpdateManagerComponent : MonoBehaviour
    {
        //Ссылка на менеджер обновлений
        private UpdateManager _updManager;

        public void Setup(UpdateManager mng)
        {
            _updManager = mng;
        }

        private void Update()
        {
            _updManager.DoUpdate();
        }

        private void FixedUpdate()
        {
            _updManager.DoFixedUpdate();
        }
    }
}
