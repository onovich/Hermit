namespace Hermit.Login {

    public class LoginBusinessContext {

        public UIAppContext uiAppContext;
        public LoginEventCenter evt;

        public LoginBusinessContext() {
            evt = new LoginEventCenter();
        }

    }

}