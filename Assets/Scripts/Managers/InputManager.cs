using Assets.Scripts.Events;
using Assets.Scripts.Interface;
using Assets.Scripts.Variables;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера для обработки ввода
    /// </summary>
    [CreateAssetMenu(fileName = "InputManager", menuName = "Game Manager/Input Manager", order = 2)]
    public class InputManager : BaseManager, IAwake, IUpdate
    {
        //Ссылки на скриптовые объекты переменных и событий
        public FloatReference HorizontalInput;
        public FloatReference VerticalInput;
        public GameEventScriptableObject FireEvent;
        public GameEventScriptableObject NextWeaponEvent;
        public GameEventScriptableObject PreviousWeaponEvent;

        public void OnAwake()
        {
            UpdateManager.Register(this);
        }

        public void OnUpdate()
        {
            //Считываем данные с устройств ввода
            var horInput = Input.GetAxis("Horizontal");
            var vertInput = Input.GetAxis("Vertical");
            var fireInput = Input.GetButtonDown("Fire");
            var nextInput = Input.GetButtonDown("NextWeapon");
            var previousInput = Input.GetButtonDown("PreviousWeapon");

            //Задаем переменные и вызываем события
            HorizontalInput.Variable.SetValue(horInput);
            VerticalInput.Variable.SetValue(vertInput);

            //Нужно делать проверку, чтобы события не вызывались одновременно
            if (fireInput)
                FireEvent.Invoke();
            else if (nextInput)
                NextWeaponEvent.Invoke();
            else if (previousInput)
                PreviousWeaponEvent.Invoke();
        }
    }
}
