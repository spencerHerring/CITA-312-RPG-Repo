﻿using UnityEngine;
using RPG.Movement;
using RPG.Core;

    namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponsRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;

        Transform target;
        float timeSinceLastAttack = 0;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            bool isInRange = GetIsInRange();
            if (target != null && !isInRange)
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }

           
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponsRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }

        // animation event
        void Hit()
        {

        }
    }
}
