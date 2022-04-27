using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualFXSystem
{
    [CreateAssetMenu(fileName = "VisualFX", menuName = "VisualFX/VisualFX", order = 1)]
    public class VisualFX : ScriptableObject
    {
        public GameObject prefab;
        public float duration;
        public bool autoStop;
        public bool detach;
        public Color[] colors = { Color.white };
        public AudioClip clip;

        public VisualFXInstance Begin(Transform t)
        {
            GameObject obj = Instantiate(prefab, detach ? null : t);

            if(detach)
                obj.transform.position = t.position;

            VisualFXInstance instance = obj.GetOrAddComponent<VisualFXInstance>();
           
            instance.Init(this, autoStop);

            if(clip != null)
            {
                AudioSource source = instance.gameObject.GetOrAddComponent<AudioSource>();

                source.clip = clip;
                source.Play();
                source.spatialBlend = 0.8f;
            }

            return instance;
        }
    }
}
