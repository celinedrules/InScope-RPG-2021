using System;
using Character;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player player;

        private void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            
        }
    }
}