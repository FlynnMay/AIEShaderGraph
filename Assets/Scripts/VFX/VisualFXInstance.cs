using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VisualFXSystem
{
    public class VisualFXInstance : MonoBehaviour
    {
        float countdown;
        public bool countingDown;
        public List<ParticleSystem> particles;
        public void Init(VisualFX fx, bool autoStop)
        {
            countingDown = autoStop;
            countdown = fx.duration;

            particles = GetComponentsInChildren<ParticleSystem>().ToList();

            int index = 0;
            foreach (ParticleSystem particleSystem in particles)
            {
                if (fx.colors.Length <= 0)
                    break;

                ParticleSystem.MainModule main = particleSystem.main;
                main.startColor = fx.colors[index];

                ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
                if (colorOverLifetime.enabled)
                    colorOverLifetime.color = TintGradient(colorOverLifetime.color, fx.colors[index]);

                index++;
                index %= fx.colors.Length;
            }
        }

        void Update()
        {
            if(!countingDown)
                return;
            countdown -= Time.deltaTime;

            if(countdown < 0)
                Destroy(gameObject);
        }
        public bool IsFinished() { return countdown <= 0; }

        public static ParticleSystem.MinMaxGradient TintGradient(ParticleSystem.MinMaxGradient minMaxGradient, Color color)
        {
            switch (minMaxGradient.mode)
            {

                case ParticleSystemGradientMode.Color:
                    minMaxGradient.color = color;
                    break;
                case ParticleSystemGradientMode.Gradient:
                    Gradient gradient = minMaxGradient.gradient;
                    GradientColorKey[] colorKeys = gradient.colorKeys;
                    for (int i = 0; i < colorKeys.Length; i++)
                        colorKeys[i].color = color;
                    gradient.SetKeys(colorKeys, gradient.alphaKeys);
                    minMaxGradient.gradient = gradient;
                    break;
                //case ParticleSystemGradientMode.TwoColors:
                //    break;
                //case ParticleSystemGradientMode.TwoGradients:
                //    break;
                //case ParticleSystemGradientMode.RandomColor:
                //    break;
                //default:
                    //break;
            }
            return minMaxGradient;
        }
    }
}
