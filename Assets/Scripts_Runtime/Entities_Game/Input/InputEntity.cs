using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit {

    public class InputEntity {

        public Vector2 moveAxis;

        InputKeybindingComponent keybindingCom;

        public void Ctor() {
            keybindingCom.Ctor();
        }

        public void ProcessInput(Camera camera, float dt) {

            // Move Axis
            if (keybindingCom.IsKeyPressing(InputKeyEnum.MoveLeft)) {
                moveAxis.x = -1;
            } else if (keybindingCom.IsKeyPressing(InputKeyEnum.MoveRight)) {
                moveAxis.x = 1;
            }

            if (keybindingCom.IsKeyPressing(InputKeyEnum.MoveForward)) {
                moveAxis.y = 1;
            } else if (keybindingCom.IsKeyPressing(InputKeyEnum.MoveBackward)) {
                moveAxis.y = -1;
            }

            // Rotate Axis

            // Jump

            // Attack

            // Skill

            // Camera Move

        }

        public void Keybinding_Set(InputKeyEnum key, KeyCode[] keyCodes) {
            keybindingCom.Bind(key, keyCodes);
        }

        public void Reset() {
            moveAxis = Vector2.zero;
        }

    }

}