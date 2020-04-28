using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    /// <summary>
    /// Класс для логики противника
    /// </summary>
    public class EnemyCore : MonoBehaviour
    {
        private EnemyScriptableObject _enemyDescription;

        public void SetDescription(EnemyScriptableObject enemyDesc)
        {
            _enemyDescription = enemyDesc;
        }
    }
}
