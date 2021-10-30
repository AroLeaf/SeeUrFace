using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

namespace SeeUrFace {

public class SUFModel3D : SUFModel {
  public Transform head;

  private VRMBlendShapeProxy proxy;

  void Start() {
    proxy = GetComponent<VRMBlendShapeProxy>();
  }

  public override void Translate(Vector3 translation) {
    transform.position = translation;
  }

  public override void Rotate(Vector3 rotation, Vector3 headRotation) {
    head.rotation = Quaternion.Euler(headRotation.x, headRotation.y, headRotation.z) * Quaternion.Euler(rotation.x, rotation.y, rotation.z);
  }

  public override void Mouth(Vector2 mouthPosition) {
    if (!proxy) return;
    proxy.SetValues(new Dictionary<BlendShapeKey, float> {
      {BlendShapeKey.CreateFromPreset(BlendShapePreset.I), Mathf.Clamp(mouthPosition.x, 0, 1)},
      {BlendShapeKey.CreateFromPreset(BlendShapePreset.U), Mathf.Clamp(-mouthPosition.x, 0, 1)},
      {BlendShapeKey.CreateFromPreset(BlendShapePreset.O), Mathf.Clamp(mouthPosition.y, 0, 1)}
    });
  }

  public override void Eyes(Vector2 eyes) {
    if (!proxy) return;
    proxy.SetValues(new Dictionary<BlendShapeKey, float> {
      {BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_L), 1-Mathf.Clamp(eyes.x*10-2.5f, 0, 1)},
      {BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_R), 1-Mathf.Clamp(eyes.y*10-2.5f, 0, 1)},
    });
  }

  public override void Expression(int id) {
    if (!proxy) return;
    int i = 0;
    foreach (var pair in proxy.GetValues()) {
      if (pair.Key.Name.Contains("[SUF]")||Array.IndexOf(new[]{BlendShapePreset.Neutral, BlendShapePreset.Joy, BlendShapePreset.Angry, BlendShapePreset.Sorrow, BlendShapePreset.Fun}, pair.Key.Preset)>=0) {
        proxy.ImmediatelySetValue(pair.Key, id==i?1f:0f);
        i++;
      }
    }
  }
}

}