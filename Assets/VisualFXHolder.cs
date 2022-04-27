using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualFXSystem
{
    public class VisualFXHolder : MonoBehaviour
    {
        [Header("VFX")]
        public VisualFX vfx;
        public Transform vfxPosition;
        VisualFXInstance vfxInstance;

        [Header("Debug")]
        [Range(0f, 1f)]
        public float debugRadius = 0.1f;


        void Start()
        {
            vfxInstance = vfx?.Begin((vfxPosition != null) ? vfxPosition : transform);            
        }

        public void SetVFXActive(bool active)
        {
            vfxInstance.gameObject.SetActive(active);
        }

        public void ToggleVFXActive()
        {
            SetVFXActive(!vfxInstance.gameObject.activeInHierarchy);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = vfx ? vfx.colors[0] : Color.white;

            Gizmos.DrawWireSphere((vfxPosition != null) ? vfxPosition.position : transform.position, debugRadius);
        }
    }
}
