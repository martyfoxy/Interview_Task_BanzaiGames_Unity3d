using System;

namespace Assets.Scripts.ScriptableObjects.Variables
{
    /// <summary>
    /// Класс описывающей поле Float, которое может быть константой или скриптовым объектом
    /// </summary>
    [Serializable]
    public class FloatReference
    {
        public bool IsConstant = true;
        public float ConstValue;
        public FloatVariableScriptableObject Variable;

        public FloatReference()
        { }

        public FloatReference(float value)
        {
            IsConstant = true;
            ConstValue = value;
        }

        public float Value
        {
            get
            {
                return IsConstant ? ConstValue : Variable.Value;
            }
        }

        /// <summary>
        /// Неявное преобразование во float
        /// </summary>
        /// <param name="reference">Ссылка на FloatVariable</param>
        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }

}