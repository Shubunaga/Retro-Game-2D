// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// public class StoneBlock : MonoBehaviour
// {

//   [SerializeField] private bool _isBeingPushed;
//   [SerializeField] private float _speed = 3f;
//   [SerializeField] private LayerMask _blockMask = 0;
//   [SerializeField] private int _detectionRadius = 1;
  
//   private Vector3 _destination;
//   public float _speedMultiplier;
  

//   void Update() {
//     if (Vector3.Distance(transform.position, _destination) < Mathf.Epsilon)
//     {
//       CheckDirection();
//     }
//     else
//     {
//       transform.position = Vector3.MoveTowards(
//         transform.position,
//         _destination, _speed	* Time.deltaTime);
//     }
//   }

//   public void Push(Vector3 direction, float speed)
//   {
//     if(!_isBeingPushed)
//     {
//       if (CheckDirection(direction))
//       {
//         _destination = transform.position + direction;
//         _speed = speed * _speedMultiplier;
//         _isBeingPushed = true;
//       }
//     }
//   }

//   private bool CheckDirection(Vector3 direction)
//   {
//     if (Physics2D.Raycast(transform.position, direction, out RaycastHit hit, _detectionRadius, _blockMask))
//     {

//     }
//   }
// }