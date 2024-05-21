using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private int _health = 100;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private List<Rigidbody> _rigidbodies;
        private Animator _animator;

        public int waypointIndex;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            SetRagdoll(true);
        }

        private void LateUpdate()
        {
            _healthSlider.value = _health;
            if (_health <= 0)
            {
                _health = -1;
                _animator.enabled = false;
                LevelManager.Instance.GetWaypointsManager().waypoints[waypointIndex].targets.Remove(this.gameObject);
                SetRagdoll(false);
            }
        }

        private void OnRagdollCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                Debug.Log("SHOOT");
                _health -= 50;
            }
        }

        private void SetRagdoll(bool status)
        {
            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                _rigidbodies[i].AddComponent<RagdollHandler>().parentObject = this.gameObject;
                _rigidbodies[i].isKinematic = status;
            }
        }
    }
}