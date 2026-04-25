using System;
using UnityEngine;
using UnityEngine.AI;

namespace _01_Game.Scripts.Human
{
    public class Movement : MonoBehaviour, IMove
    {

        private NavMeshAgent _agent;
        private Animator _animator;
        
        private readonly int _isMove = Animator.StringToHash("IsMove");
        
        public Action onReachDestination;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void MoveToPoint(Vector3 point)
        {
            _agent.SetDestination(point);
            _animator.SetBool(_isMove, true);
        }

        public void OnReachDestination(Action action)
        {
            onReachDestination =  action;
        }

        private void Update()
        {
            if (HasReachedDestination())
            {
                _animator.SetBool(_isMove, false);
                onReachDestination?.Invoke();
                onReachDestination = null;
            }
        }
        public bool HasReachedDestination()
        {
            if (_agent.pathPending) return false;

            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }

            return false;
        }
    }
}