using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Enemies.Scripts
{
    public class EnemyAiMelee : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Transform _player;
        public Animator animator;
        public Slider healthBar;
        public LayerMask whatIsPlayer;

        //private PlayerHealth _playerHealth;

        public float health;
        private float _maxHealth;
        private Rigidbody _rb;
        private bool _isDead;

        //Animation
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int PlayerInRange = Animator.StringToHash("playerInRange");
        private static readonly int Death = Animator.StringToHash("death");

        //Attacking
        public float timeBetweenAttacks = 1f;
        private bool _alreadyAttacked;

        //States
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;
        public float damage = 5.0f;

        private GameObject _leftHandInstance;
        private GameObject _rightHandInstance;

        private void Awake()
        {
            _leftHandInstance = FindChildGameObject(gameObject, "LeftHand");
            _rightHandInstance = FindChildGameObject(gameObject, "RightHand");

            if (_leftHandInstance == null || _rightHandInstance == null)
            {
                Debug.LogError("LeftHand or RightHand not found!");
            }

            _player = GameObject.Find("Slime_01").transform;

            _maxHealth = health;
            _rb = GetComponent<Rigidbody>();

            _agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private static GameObject FindChildGameObject(GameObject parent, string name)
        {
            var children = parent.GetComponentsInChildren<Transform>(true);
            foreach (var child in children)
            {
                if (child.name == name)
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        private void Update()
        {
            if (!_isDead)
            {
                if (_player != null)
                {
                    //_playerHealth = _player.GetComponent<PlayerHealth>();
                }

                healthBar.value = (health / _maxHealth) * 100;

                var position = transform.position;
                playerInSightRange = Physics.CheckSphere(position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);

                if (!playerInSightRange && !playerInAttackRange) PlayerIdle();
                if (playerInSightRange && !playerInAttackRange) ChasePlayer();
                if (playerInAttackRange && playerInSightRange) AttackPlayer();
            }
        }

        private void PlayerIdle()
        {
            transform.LookAt(Vector3.zero);
            animator.SetBool(PlayerInRange, false);
            _agent.SetDestination(gameObject.transform.position);
        }

        private void ChasePlayer()
        {
            if (!(_alreadyAttacked && _isDead))
            {
                _agent.speed = 5;
            }

            animator.SetBool(PlayerInRange, true);
            transform.LookAt(_player);
            SetDestination();
            animator.SetBool(PlayerInRange, true);
            _agent.SetDestination(_player.position);
        }

        private void AttackPlayer()
        {
            transform.LookAt(_player);
            animator.SetTrigger(Attack);

            ActiveAttackHitBox();

            if (!_alreadyAttacked)
            {
                _alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
                _agent.speed = 0;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isDead || collision.collider.gameObject.name != "Slime_01") return;
            PlayerAttack();
            DeactivateAttachHitBox();
        }

        private void PlayerAttack()
        {
            //_playerHealth.TakeDamage(damage);
        }

        private void DeactivateAttachHitBox()
        {
            if (_rightHandInstance == null || _leftHandInstance == null) return;
            _rightHandInstance.SetActive(false);
            _leftHandInstance.SetActive(false);
        }

        private void ActiveAttackHitBox()
        {
            if (_rightHandInstance == null || _leftHandInstance == null) return;
            _rightHandInstance.SetActive(true);
            _leftHandInstance.SetActive(true);
        }

        private void SetDestination()
        {
            _agent.SetDestination(_player.position);
        }

        private void ResetAttack()
        {
            _alreadyAttacked = false;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health >= 0 || _isDead) return;
            StopGame();

            animator.SetTrigger(Death);
            Invoke(nameof(DestroyEnemy), 3.5f);
        }

        private void DestroyEnemy()
        {
            Destroy(gameObject);
            //PlayerLevel.AddXP(10);
        }

        private void StopGame()
        {
            _isDead = true;
            DeactivateAttachHitBox();
            
            _rb.freezeRotation = true;
            _agent.speed = 0;
            healthBar.enabled = false;
        }
    }
}