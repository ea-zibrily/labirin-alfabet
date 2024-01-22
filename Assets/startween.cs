using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabirinKata
{
    public class startween : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // LeanTween.rotateAround(gameObject, Vector3.forward, -360, 10f).setLoopClamp();
            LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f),2f).setDelay(0.2f).setEase(LeanTweenType.easeOutElastic);
        }
    }
}
