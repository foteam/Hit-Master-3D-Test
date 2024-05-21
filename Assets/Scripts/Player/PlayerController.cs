using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float playerRunSpeed = 5f;

        private PlayerManager _pm;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _pm = new PlayerManager();
            _pm.AddNewState(new IdleState(_pm, _animator));
            _pm.AddNewState(new ShootState(_pm, _animator, this.gameObject, FindObjectOfType<LeanFingerDown>()));
            _pm.AddNewState(new RunState(_pm, _animator, this.gameObject, playerRunSpeed));
            _pm.SetState<IdleState>();
            
            FindObjectOfType<LeanFingerDown>().OnFinger.AddListener(StartGame);
        }

        private void Update()
        {
            _pm.Update();
        }

        private void StartGame(LeanFinger tap)
        {
            _pm.SetState<RunState>();
        }
    }
}
