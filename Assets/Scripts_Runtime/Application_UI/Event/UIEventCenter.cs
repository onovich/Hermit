using System;

namespace Hermit.UI {

    public class UIEventCenter {

        public UIEventCenter() {

        }

        // Login
        public Action Login_OnLoginClickHandle;
        public void Login_OnLoginClick() {
            Login_OnLoginClickHandle?.Invoke();
        }

        public void Clear() {
            Login_OnLoginClickHandle = null;
        }

    }

}