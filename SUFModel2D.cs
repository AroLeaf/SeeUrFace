using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;

namespace SeeUrFace {

public class SUFModel2D : SUFModel {
  private CubismModel model;
  private Live2D.Cubism.Framework.Expression.CubismExpressionController expressions;

  void Start() {
    model = this.FindCubismModel();
    expressions = GetComponent<Live2D.Cubism.Framework.Expression.CubismExpressionController>();
  }

  public override void Translate(Vector3 translation) {
    transform.position = translation;
  }

  public override void Rotate(Vector3 rotation, Vector3 headRotation) {
    model.Parameters.FindById("ParamAngleY").Value = -(rotation.x+headRotation.x);
    model.Parameters.FindById("ParamAngleX").Value = rotation.y+headRotation.y;
    model.Parameters.FindById("ParamAngleZ").Value = rotation.z+headRotation.z;

    model.Parameters.FindById("ParamBodyAngleY").Value = -(rotation.x+headRotation.x)/3;
    model.Parameters.FindById("ParamBodyAngleX").Value = (rotation.y+headRotation.y)/3;
    model.Parameters.FindById("ParamBodyAngleZ").Value = (rotation.z+headRotation.z)/3;
  }

  public override void Mouth(Vector2 mouthPosition) {
    model.Parameters.FindById("ParamMouthForm").Value = mouthPosition.x;
    model.Parameters.FindById("ParamEyeLSmile").Value = mouthPosition.x;
    model.Parameters.FindById("ParamEyeRSmile").Value = mouthPosition.x;
    model.Parameters.FindById("ParamMouthOpenY").Value = mouthPosition.y;
  }

  public override void Eyes(Vector2 eyes) {
    model.Parameters.FindById("ParamEyeLOpen").Value = Mathf.Min(eyes.x*10-2.5f, 1); //eyes.x<0?Mathf.Pow(eyes.x+1, 10):1;
    model.Parameters.FindById("ParamEyeROpen").Value = Mathf.Min(eyes.y*10-2.5f, 1); //eyes.y<0?Mathf.Pow(eyes.y+1, 10):1;
  }

  public override void Expression(int id) {
    expressions.CurrentExpressionIndex = id-1;
  }
}

}