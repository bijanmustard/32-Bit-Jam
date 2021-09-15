using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Code © Bijan Pourmand
 *  Authored 9/9/21
 *  Sample for scripted animation for use with Textbox Engine
 */

public class TbxAnim_Sample : Textbox_Animation
{
    protected override AnimationClip CreateClip()
    {
        //1. Create an animation curve
        Keyframe k1, k2;
        k1 = new Keyframe(0, transform.position.x - 30);
        k2 = new Keyframe(1, transform.position.x + 30);
        AnimationCurve curve = new AnimationCurve(new Keyframe[] { k1, k2 });
        //2. Set curve wrap mode
        curve.preWrapMode = WrapMode.PingPong;
        curve.postWrapMode = WrapMode.PingPong;
        //3. Create clip from curve
        AnimationClip clip = new AnimationClip();
        //3a. Set clip name
        clip.name = "Horizontal Move";
        //3b. Map curve to object's Transform.position.x property
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
        //4. Return clip to add it
        clip.legacy = true;
        clip.wrapMode = WrapMode.PingPong;
        return clip;
    }

}
