using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{

    public class StateMachineDriver : MonoBehaviour
    {
        // Start is called before the first frame update

        private StateMachine baseState;
        void Start()
        {
        }

        private void Awake()
        {

        //    baseState = new StateIdle(this.gameObject, GetComponent<Animator>(), GetComponent<Rigidbody2D>());
        }

        // Update is called once per frame
        void Update()
        {
            baseState = baseState.Process();
        }
    }

}