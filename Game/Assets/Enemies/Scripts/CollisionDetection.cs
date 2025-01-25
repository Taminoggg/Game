using Enemies.Scripts;
using UnityEngine;

namespace Enemy
{
    public class CollisionDetection : MonoBehaviour
    {
        public EnemyAiMelee enemy;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("whatIsEnemy"))
            {
                enemy.TakeDamage(1);
            }
        }
    }
}
