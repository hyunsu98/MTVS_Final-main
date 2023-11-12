using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudioClip_LHS : MonoBehaviour
{
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnssDetection_LHS detector;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    [SerializeField]
    float loudness = 0;

    public int retrieveCount = 10;
    int curCount = 0;
    // Update is called once per frame
    void Update()
    {
        if (retrieveCount > curCount)
        {
            curCount++;
        }
        else
        {
            loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip) * loudnessSensibility;

            if (loudness < threshold)
                loudness = 0;

            //float loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip);

            //if (loudness < threshold)
            //    loudness = 0;

            //lerp value from minscale to maxscale
            loudness /= 15.0f;
            curCount = 0;
        }
        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }
}
