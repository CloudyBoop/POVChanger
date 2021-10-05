using System;
using Il2CppSystem.Collections;
using MelonLoader;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace POVChanger
{
    public class Main : MelonMod
    {
        //HmdPivot
        private Camera _myCam;
        private Transform _neck;
        private Vector3 _originalScale;
        private Camera _playerCam;

        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(VRChat_OnUiManagerInit());
        }

        private IEnumerator VRChat_OnUiManagerInit()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
            {
                yield return new WaitForSeconds(1F);
            }
            _myCam = Camera.main;
        }

        public override void OnUpdate()
        {
            if (Input.anyKeyDown && Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    _myCam.enabled = false;
                    
                    var ply = QuickMenu.prop_QuickMenu_0.field_Private_Player_0;
                    
                    if (ply.transform.Find("ForwardDirection/Avatar").GetComponent<Animator>()
                        .GetBoneTransform(HumanBodyBones.Head).FindChild("HmdPivot").GetComponent<Camera>())
                    {
                        _playerCam = ply.transform.Find("ForwardDirection/Avatar").GetComponent<Animator>()
                            .GetBoneTransform(HumanBodyBones.Head).FindChild("HmdPivot").GetComponent<Camera>();
                        _playerCam.enabled = true;
                        _neck = ply.transform.Find("ForwardDirection/Avatar").GetComponent<Animator>()
                            .GetBoneTransform(HumanBodyBones.Neck);
                        _originalScale = _neck.localScale;
                        _neck.localScale = new Vector3(0, _originalScale.y, _originalScale.z);
                    }
                    else
                    {
                        _playerCam = ply.transform.Find("ForwardDirection/Avatar").GetComponent<Animator>()
                            .GetBoneTransform(HumanBodyBones.Head).FindChild("HmdPivot").gameObject
                            .AddComponent<Camera>();
                        _playerCam.fieldOfView = 90;
                        _playerCam.nearClipPlane = 0.01f;
                        _playerCam.enabled = true;
                        _neck = ply.transform.Find("ForwardDirection/Avatar").GetComponent<Animator>()
                            .GetBoneTransform(HumanBodyBones.Neck);
                        _originalScale = _neck.localScale;
                        _neck.localScale = new Vector3(0, _originalScale.y, _originalScale.z);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    _myCam.enabled = true;
                    _neck.localScale = _originalScale;
                    _playerCam.enabled = false;
                }
            }
        }
    }
}
