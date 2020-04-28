using UnityEngine;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Variables;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Класс для логики танка, которым управляет игрок
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TankCore : MonoBehaviour
    {
        [Tooltip("Ссылка на переменную хранящую текущее здоровье")]
        public IntReference CurrentHealthVariable;

        //Используемый объект описания танка
        private TankScriptableObject _tankDescription;

        //Ссылки на колеса танка
        [SerializeField]
        private GameObject[] _rightWheels;
        [SerializeField]
        private GameObject[] _leftWheels;

        //Ссылки на компоненты танка
        private Rigidbody _rigidBody;
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        /// <summary>
        /// Задать описание танка
        /// </summary>
        public void SetDescription(TankScriptableObject tankDesc)
        {
            _tankDescription = tankDesc;

            CurrentHealthVariable.Variable.SetValue(_tankDescription.Health);
        }

        /// <summary>
        /// Передвинуть танк в направлении заданном вводом
        /// </summary>
        /// <param name="vertInputValue">Значение ввода</param>
        public void MoveTank(float vertInputValue)
        {
            Vector3 movementVector = transform.forward * _tankDescription.Speed * vertInputValue * Time.fixedDeltaTime;

            _rigidBody.MovePosition(_rigidBody.position + movementVector);

            //Вращаем колеса
            for (int i = 0; i < _rightWheels.Length; i++)
            {
                var tr = _rightWheels[i].transform;
                tr.Rotate(tr.rotation.x + vertInputValue * 5f, 0f, 0f);
            }
            for (int i = 0; i < _leftWheels.Length; i++)
            {
                var tr = _leftWheels[i].transform;
                tr.Rotate(tr.rotation.x + vertInputValue * 5f, 0f, 0f);
            }
        }

        /// <summary>
        /// Повернуть танк в направлении заданном вводом
        /// </summary>
        /// <param name="horInputValue"></param>
        public void RotateTank(float horInputValue)
        {
            Quaternion rotationQuat = Quaternion.Euler(0f, horInputValue * 45f * Time.fixedDeltaTime, 0f);

            _rigidBody.MoveRotation(_rigidBody.rotation * rotationQuat);

            //Поворачиваем колеса
            for (int i = 0; i < _rightWheels.Length; i++)
            {
                var tr = _rightWheels[i].transform;
                tr.Rotate(tr.rotation.x - horInputValue * 5f, 0f, 0f);
            }
            for (int i = 0; i < _leftWheels.Length; i++)
            {
                var tr = _leftWheels[i].transform;
                tr.Rotate(tr.rotation.x + horInputValue * 5f, 0f, 0f);
            }
        }
    }
}
