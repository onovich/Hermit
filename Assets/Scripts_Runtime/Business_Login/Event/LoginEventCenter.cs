using System;

namespace Hermit.Login {

    public class LoginEventCenter {

        public LoginEventCenter() {

        }

        // Login
        public Action Login_OnLoginFinishHandle;
        public void Login_OnLoginFinish() {
            Login_OnLoginFinishHandle?.Invoke();
        }

        public void Clear(){
            Login_OnLoginFinishHandle = null;
        }

    }

}