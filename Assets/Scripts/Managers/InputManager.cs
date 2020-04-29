using Assets.Scripts.Interface;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Variables;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера для обработки ввода
    /// </summary>
    [CreateAssetMenu(fileName = "InputManager", menuName = "Game Manager/Input Manager", order = 2)]
    public class InputManager : BaseManager, IAwake, IUpdate
    {
        [Header("Ссылки на объекты с переменными ввода")]
        [Tooltip("Переменная для ввода стрелками влево-вправо")]
        public FloatReference HorizontalInput;
        [Tooltip("Переменная для ввода стрелками вперед-назад")]
        public FloatReference VerticalInput;

        [Header("Ссылки на объекты с событиями ввода")]
        [Tooltip("Ссылка на событие нажатия кнопки выстрела")]
        public GameEventScriptableObject FireEventButton;
        [Tooltip("Ссылка на событие нажатия кнопки следующего оружия")]
        public GameEventScriptableObject NextWeaponButtonEvent;
        [Tooltip("Ссылка на событие нажатия кнопки предыдущего оружия ")]
        public GameEventScriptableObject PreviousWeaponButtonEvent;

        public void OnAwake()
        {
            UpdateManager.Register(this);
        }

        public void OnUpdate()
        {
            //Считываем данные с устройств ввода
            float horInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");
            bool fireInput = Input.GetButtonDown("Fire");
            bool nextInput = Input.GetButtonDown("NextWeapon");
            bool previousInput = Input.GetButtonDown("PreviousWeapon");

            //Задаем переменные и вызываем события
            HorizontalInput.Variable.SetValue(horInput);
            VerticalInput.Variable.SetValue(vertInput);

            //Нужно делать проверку, чтобы события не вызывались одновременно
            if (fireInput)
                FireEventButton.Invoke();
            else if (nextInput)
                NextWeaponButtonEvent.Invoke();
            else if (previousInput)
                PreviousWeaponButtonEvent.Invoke();
        }
    }
}
