using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        Vector3 gaurdPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");

            gaurdPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SupicionBehaviour();
            }
            else
            {
                GaurdBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            
        }

        private void GaurdBehaviour()
        {
            mover.StartMoveAction(gaurdPosition);
        }

        private void SupicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
