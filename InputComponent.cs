using System;
using UnityEngine;

namespace POVChanger
{
    public class InputComponent : MonoBehaviour
    {
        private void Update()
        {
            Main.OnUpdate();
        }
    }
}