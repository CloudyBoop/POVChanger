using System;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.XR;
using IEnumerator = System.Collections.IEnumerator;
using Object = UnityEngine.Object;

namespace POVChanger
{
    public class Main : MelonMod
    {
        //HmdPivot
        private static Camera _myCam;
        private static Transform _neck;
        private static Vector3 _originalScale;
        private static Camera _playerCam;

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
            if (!XRDevice.isPresent)
            {
                ClassInjector.RegisterTypeInIl2Cpp<InputComponent>();
                var go = new GameObject("PovChangerMod");
                Object.DontDestroyOnLoad(go);
                go.AddComponent<InputComponent>();
            }
        }

        public static void OnUpdate()
        {
            if (Input.anyKeyDown && Event.current.control)
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
