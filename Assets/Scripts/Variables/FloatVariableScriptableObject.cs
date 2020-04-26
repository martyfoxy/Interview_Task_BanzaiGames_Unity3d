using UnityEngine;

namespace Assets.Scripts.Variables
{
    /// <summary>
    /// Скриптовый объект любых переменных типа Float
    /// </summary>
    [CreateAssetMenu(fileName = "NewFloatVariable", menuName = "Game Data/New Float Variable", order = 0)]
    public class FloatVariableScriptableObject : ScriptableObject
    {
        public float Value;

        public void SetValue(FloatVariableScriptableObject newValue)
        {
            Value = newValue.Value;
        }

        public void SetValue(float newValue)
        {
            Value = newValue;
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariableScriptableObject amount)
        {
            Value += amount.Value;
        }
    }
}