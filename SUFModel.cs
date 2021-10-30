using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeeUrFace {

public class SUFModel : MonoBehaviour {
  public Vector3 translationOffset;
  [HideInInspector]
  public Vector3 previousRotation;
  [HideInInspector]
  public Vector3 previousTranslation;

  private float smoothTime = 0.05f;
  private Vector3 translationVelocity = Vector3.zero;
  private Vector3 rotationVelocity = Vector3.zero;

  public void SUFUpdate(Vector3 translation, Vector3 rotation, Vector3 headRotation) {
    translation = Vector3.SmoothDamp(previousTranslation, translation + translationOffset, ref translationVelocity, smoothTime);
    previousTranslation = translation;
    Translate(translation);

    rotation = Vector3.SmoothDamp(previousRotation, rotation, ref rotationVelocity, smoothTime);
    previousRotation = rotation;
    Rotate(rotation, headRotation);
  }

  public virtual void Rotate(Vector3 rotation, Vector3 headRotation){}
  public virtual void Translate(Vector3 translation){}
  public virtual void Mouth(Vector2 mouthPosition){}
  public virtual void Eyes(Vector2 eyes){}
  public virtual void Expression(int id){}
}

}