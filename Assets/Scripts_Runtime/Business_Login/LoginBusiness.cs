using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit.Login {

    public static class LoginBusiness {

        public static void Enter(LoginBusinessContext ctx) {
            UIApp.Login_Open(ctx.uiAppContext);
        }

        public static void Tick(LoginBusinessContext ctx, float dt) {
        }

        public static void Exit(LoginBusinessContext ctx) {
            UIApp.Login_TryClose(ctx.uiAppContext);
        }

        public static void ExitApplication(LoginBusinessContext ctx) {
            Exit(ctx);
            Application.Quit();
            HLog.Log("Application.Quit()");
        }

        public static void OnLoginFinish(LoginBusinessContext ctx) {
            ctx.evt.Login_OnLoginFinish();
        }

    }

}