using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code © Bijan Pourmand
 * Authored 9/8/21
 */

public class SampleAnim : MonoBehaviour
{

    AnimationCurve curve;
    Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        Keyframe kf = new Keyframe(0, 0);
        Keyframe kf2 = new Keyframe(1, 10);
        Keyframe kf3 = new Keyframe(2, 0);
        curve = new AnimationCurve(new Keyframe[]{ kf, kf2, kf3 });
        curve.preWrapMode = WrapMode.PingPong;
        curve.postWrapMode = WrapMode.PingPong;
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.name = "test";
        clip.SetCurve("", typeof(Transform), "localScale.x", curve);
        //curve = AnimationCurve.Linear(0, 0, 1, 1);
        anim.AddClip(clip, clip.name);
        anim.wrapMode = WrapMode.PingPong;
        anim.Play(clip.name);

        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(anim.isPlaying);
    }
}
