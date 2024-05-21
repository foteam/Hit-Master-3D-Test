using System.Collections;
using System;
using Lean.Touch;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using Task = System.Threading.Tasks.Task;

namespace Player
{
    public abstract class PlayerState 
    {
        protected readonly PlayerManager _playerManager;

        public PlayerState(PlayerManager pm)
        {
            _playerManager = pm;
        }

        public virtual void Enter(params object[] args){}

        public virtual void Update(){}

        public virtual void Exit(){}
    }

    public class IdleState : PlayerState
    {
        protected readonly Animator _animator;
        private LevelManager _levelManager;

        private int _currentIndex;
        
        public IdleState(PlayerManager pm, Animator animator) : base(pm)
        {
            _animator = animator;
            _levelManager = LevelManager.Instance;
        }
        public override void Enter(params object[] args)
        {
            base.Enter();
            Debug.Log("IDLE ENTER state");
            _animator.SetInteger("Idle", 1);

            if (args.Length > 0) _currentIndex = (int)args[0];

        }

        public override void Update()
        {
            base.Update();
            Debug.Log("IDLE UPDATE state");
            if (_levelManager.GetWaypointsManager().waypoints[_currentIndex].targets.Count == 0) _playerManager.SetState<RunState>();
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("IDLE EXIT state");
            _animator.SetInteger("Idle", 0);
        }
    }
    public class RunState : PlayerState 
    {
        protected readonly Animator _animator;
        private GameObject _obj;
        
        private LevelManager _levelManager;

        private int _currentIndex = 0;
        private int _nextIndex;

        private Transform _startPoint;
        private Transform _endPoint;
        private float _runSpeed;

        public RunState(PlayerManager pm, Animator animator, GameObject obj, float runSpeed) : base(pm)
        {
            _animator = animator;
            _levelManager = LevelManager.Instance;
            _obj = obj;
            _runSpeed = runSpeed;
        }
        public override void Enter(params object[] args)
        {
            base.Enter();
            Debug.Log("RUN ENTER state");
            
            _animator.SetInteger("Run", 1);

            if (_currentIndex == 0)
            {
                _startPoint = _obj.transform;
                _endPoint = _levelManager.GetWaypointsManager().waypoints[0].waypoint;
                _currentIndex = 0;
            }

        }
        public override void Update()
        {
            base.Update();
            Debug.Log("RUN UPDATE state");
            MovePoint();
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("RUN EXIT state");
            
            _animator.SetInteger("Run", 0);
        }

        private void MovePoint()
        {
            if (Vector3.Distance(_obj.transform.position, _endPoint.position) < 0.15f)
            {
                // Check enemies at current point
                if (_levelManager.GetWaypointsManager().waypoints[_currentIndex].targets.Count > 0)
                {
                    Debug.Log("Have targets!");
                    _playerManager.SetState<ShootState>(_currentIndex);
                    return;
                }
                
                // Set next point if have...
                _currentIndex++;
                if (_currentIndex >= _levelManager.GetWaypointsManager().waypoints.Count)
                {
                    _currentIndex--;
                    SceneManager.LoadScene(0);
                }
                _startPoint.transform.position = _obj.transform.position;
                _endPoint.transform.position =
                    _levelManager.GetWaypointsManager().waypoints[_currentIndex].waypoint.position;
            }
            
            // Move to point
            if (_startPoint != null && _endPoint != null)
            {
                _obj.transform.position = Vector3.MoveTowards(_startPoint.position, _endPoint.position, 5 * Time.deltaTime);
            }
        }
        
    }
    public class ShootState : PlayerState
    {
        protected readonly Animator _animator;
        private LeanFingerDown _leanFingerDown;
        private GameObject _obj;
        private LevelManager _levelManager;
        
        private int _currentIndex;
        
        public ShootState(PlayerManager pm, Animator animator, GameObject obj, LeanFingerDown leanFingerDown) : base(pm)
        {
            _animator = animator;
            _levelManager = LevelManager.Instance;
            _leanFingerDown = leanFingerDown;
            _obj = obj;
        }
        public override void Enter(params object[] args)
        {
            base.Enter();
            Debug.Log("SHOOT ENTER state");
            _leanFingerDown.OnWorld.AddListener(Shoot);
            
            if (args.Length > 0) _currentIndex = (int)args[0];
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("SHOOT UPDATE state");
            
            if (_levelManager.GetWaypointsManager().waypoints[_currentIndex].targets.Count == 0) _playerManager.SetState<RunState>();
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("SHOOT EXIT state");
        }

        private void Shoot(Vector3 tappedWorld)
        {
            Debug.Log("Finger Tapped Count:" );

            Quaternion lookRotation = Quaternion.LookRotation(tappedWorld);
            lookRotation.x = 0;
            lookRotation.z = 0;
            _obj.transform.rotation = lookRotation;
            
            _leanFingerDown.OnWorld.RemoveListener(Shoot);
            
            BulletManager.Instance.Shoot(tappedWorld);
        }
    }
    
}