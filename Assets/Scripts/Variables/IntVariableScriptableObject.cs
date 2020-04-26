using UnityEngine;

namespace Assets.Scripts.Variables
{
    /// <summary>
    /// Скриптовый объект любых переменных типа Int
    /// </summary>
    [CreateAssetMenu(fileName = "NewIntVariable", menuName = "Game Data/New Int Variable", order = 1)]
    public class IntVariableScriptableObject : ScriptableObject
    {
        public int Value;

        public void SetValue(IntVariableScriptableObject newValue)
        {
            Value = newValue.Value;
        }

        public void SetValue(int newValue)
        {
            Value = newValue;
        }

        public void ApplyChange(int amount)
        {
            Value += amount;
        }

        public void ApplyChange(IntVariableScriptableObject amount)
        {
            Value += amount.Value;
        }
    }
}