using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using OpenSee;

namespace SeeUrFace {

public class SUFController : MonoBehaviour {
  public SUFModel[] models;
  public bool trackEyes;
  public Vector3 headRotation;
  public Vector3 headOffset;
  public float translationFactor;
  public Vector3 faceOffset;
  
  private OpenSee.OpenSee openSee;
  private PlayerInput playerInput;
  private OpenSee.OpenSee.OpenSeeData openSeeData;
  private SUFModel currentModel;
  private double updated = 0.0;
  private Vector3 translationOffset;
  private Vector3 translation;
  private Vector3 rotation;

  void Start() {
    openSee = GetComponent<OpenSee.OpenSee>();
    playerInput = GetComponent<PlayerInput>();
    for (int i=0; i<models.Length;i++) {
      models[i].gameObject.SetActive(false);
    }
    currentModel = models[0];
    currentModel.gameObject.SetActive(true);
    GetComponent<Camera>().orthographic = currentModel is SUFModel2D;
  }

  void LateUpdate() {
    SUFUpdate();
  }

  void SUFUpdate() {
    if (!openSee || !currentModel) return;
    openSeeData = openSee.GetOpenSeeData(0);
    if (openSeeData == null) return;
    if (openSeeData.time > updated) {
      updated = openSeeData.time;
    } else {
      return;
    }

    rotation = NormalizedRotation(new Vector3(-openSeeData.rotation.x, openSeeData.rotation.y, -openSeeData.rotation.z));
    translation = Vector3.Scale((new Vector3(-openSeeData.translation.x, openSeeData.translation.y, 0) - headOffset - Quaternion.Euler(currentModel.previousRotation.x, currentModel.previousRotation.y, currentModel.previousRotation.z) * faceOffset), new Vector3(translationFactor, translationFactor, 0));
    currentModel.SUFUpdate(translation, rotation, headRotation);
    currentModel.Mouth(new Vector2(openSeeData.features.MouthWide, openSeeData.features.MouthOpen));
    if (trackEyes) currentModel.Eyes(new Vector2(openSeeData.leftEyeOpen, openSeeData.rightEyeOpen));
  }

  void OnNumber(int number) {
    if (playerInput.actions["Modifier1"].ReadValue<float>() != 0) {
      currentModel.gameObject.SetActive(false);
      if (number < models.Length) currentModel = models[number];
      GetComponent<Camera>().orthographic = currentModel is SUFModel2D;
      currentModel.gameObject.SetActive(true);
    } else {
      currentModel.Expression(number);
    }
  }

  void OnFunction(int number) {
    currentModel.gameObject.SetActive(false);
    if (number < models.Length) currentModel = models[number];
    GetComponent<Camera>().orthographic = currentModel is SUFModel2D;
    currentModel.gameObject.SetActive(true);
  }

  void OnNumber1() { OnNumber(0); }
  void OnNumber2() { OnNumber(1); }
  void OnNumber3() { OnNumber(2); }
  void OnNumber4() { OnNumber(3); }
  void OnNumber5() { OnNumber(4); }
  void OnNumber6() { OnNumber(5); }
  void OnNumber7() { OnNumber(6); }
  void OnNumber8() { OnNumber(7); }
  void OnNumber9() { OnNumber(8); }
  void OnNumber0() { OnNumber(9); }

  void OnFunction1() { OnFunction(0); }
  void OnFunction2() { OnFunction(1); }
  void OnFunction3() { OnFunction(2); }
  void OnFunction4() { OnFunction(3); }
  void OnFunction5() { OnFunction(4); }
  void OnFunction6() { OnFunction(5); }
  void OnFunction7() { OnFunction(6); }
  void OnFunction8() { OnFunction(7); }
  void OnFunction9() { OnFunction(8); }
  void OnFunction10() { OnFunction(9); }


  Vector3 NormalizedRotation(Vector3 rotation) {
    return new Vector3(
      (rotation.x + 180) % 360 - 180,
      (rotation.y + 180) % 360 - 180,
      (rotation.z + 180) % 360 - 180
    );
  }
}

}