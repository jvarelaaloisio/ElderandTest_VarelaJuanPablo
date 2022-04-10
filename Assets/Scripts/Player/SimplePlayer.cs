using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class SimplePlayer : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onAttacking;
        [SerializeField]
        private UnityEvent onAttacked;

        [SerializeField]
        private float speed;

        private bool _isAttacking = false;
        
        [SerializeField]
        private float attackDuration;

        private void Update()
        {
            if(_isAttacking)
                return;
            var input = Input.GetAxis("Horizontal");
            transform.position += Vector3.right * input * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, input > 0 ? 0 : 180, 0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Attack(attackDuration));
            }
        }

        private IEnumerator Attack(float duration)
        {
            onAttacking.Invoke();
            _isAttacking = true;
            yield return new WaitForSeconds(duration);
            onAttacked.Invoke();
            _isAttacking = false;
        }
    }
}