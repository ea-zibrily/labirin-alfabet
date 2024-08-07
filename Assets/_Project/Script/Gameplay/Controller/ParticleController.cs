using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alphabet.Gameplay.Controller
{
    public class ParticleController : MonoBehaviour
    {
        private bool _isParticlePlaying;
        private ParticleSystem _particleSystem;

        public event Action OnParticleNotAlive;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (_isParticlePlaying && !_particleSystem.IsAlive())
            {
                _isParticlePlaying = false;
                OnParticleNotAlive?.Invoke();
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region Methods

        public void PlayParticle()
        {
            gameObject.SetActive(true);
            _isParticlePlaying = true;
            _particleSystem.Play();
        }

        public void StopParticle()
        {
            _particleSystem.Stop();
        }

        #endregion
    }
}