using System.Collections.Generic;
using UnityEngine;

namespace Hermit {

    public struct InputKeybindingComponent {

        public Dictionary<InputKeyEnum, KeyCode[]> all;

        public void Ctor() {
            all = new Dictionary<InputKeyEnum, KeyCode[]>();
        }

        public void Bind(InputKeyEnum key, KeyCode[] keyCodes) {
            bool succ = all.TryAdd(key, keyCodes);
            if (succ) {
                return;
            }
            all[key] = keyCodes;
        }

        public bool IsKeyPressing(InputKeyEnum key) {
            if (!all.TryGetValue(key, out KeyCode[] keyCodes)) {
                return false;
            }
            for (int i = 0; i < keyCodes.Length; i++) {
                if (Input.GetKey(keyCodes[i])) {
                    return true;
                }
            }
            return false;
        }

        public bool IsKeyDown(InputKeyEnum key) {
            if (!all.TryGetValue(key, out KeyCode[] keyCodes)) {
                return false;
            }
            for (int i = 0; i < keyCodes.Length; i++) {
                if (Input.GetKeyDown(keyCodes[i])) {
                    return true;
                }
            }
            return false;
        }

        public bool IsKeyUp(InputKeyEnum key) {
            if (!all.TryGetValue(key, out KeyCode[] keyCodes)) {
                return false;
            }
            for (int i = 0; i < keyCodes.Length; i++) {
                if (Input.GetKeyUp(keyCodes[i])) {
                    return true;
                }
            }
            return false;
        }

    }

}