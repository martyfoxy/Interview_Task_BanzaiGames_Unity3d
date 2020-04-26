using System;

namespace Assets.Scripts.Variables
{
    /// <summary>
    /// Класс описывающей поле Int, которое может быть константой или скриптовым объектом
    /// </summary>
    [Serializable]
    public class IntReference
    {
        public bool IsConstant = true;
        public int ConstValue;
        public IntVariableScriptableObject Variable;

        public IntReference()
        { }

        public IntReference(int value)
        {
            IsConstant = true;
            ConstValue = value;
        }

        public int Value
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
        public static implicit operator int(IntReference reference)
        {
            return reference.Value;
        }
    }

}