using System.Collections;
using System.Collections.Generic;
using Hermit.Business.Game;
using Hermit.Login;
using Hermit.UI;
using UnityEngine;

namespace Hermit {

    public class ClientMainContext {


        public bool isLoadedAssets;
        public bool isTearDown;

        LoginBusinessContext loginBusinessContext;
        GameBusinessContext gameBusinessContext;

        UIAppContext uiAppContext;

        public readonly AssetsInfraContext assetsInfraContext;
        public readonly TemplateInfraContext templateInfraContext;

        public readonly CameraCoreContext cameraCoreContext;

        public ClientMainContext() {

            isLoadedAssets = false;
            isTearDown = false;

            loginBusinessContext = new LoginBusinessContext();
            gameBusinessContext = new GameBusinessContext();

            uiAppContext = new UIAppContext();

            assetsInfraContext = new AssetsInfraContext();
            templateInfraContext = new TemplateInfraContext();

            cameraCoreContext = new CameraCoreContext();

        }

        public void Inject(Canvas canvas, Transform hudFakeCanvas, Camera mainCamera) {
            uiAppContext.Inject(canvas, hudFakeCanvas);
            cameraCoreContext.Inject(mainCamera);
        }

        public LoginBusinessContext BakeLoginBusiness() {
            loginBusinessContext.uiAppContext = uiAppContext;
            return loginBusinessContext;
        }

        public GameBusinessContext BakeGameBusiness() {
            gameBusinessContext.uiAppContext = uiAppContext;
            gameBusinessContext.assetsInfraContext = assetsInfraContext;
            gameBusinessContext.templateInfraContext = templateInfraContext;
            gameBusinessContext.cameraCoreContext = cameraCoreContext;
            return gameBusinessContext;
        }

        public UIAppContext BakeUIApp() {
            uiAppContext.cameraCoreContext = cameraCoreContext;
            uiAppContext.templateInfracContext = templateInfraContext;
            return uiAppContext;
        }

    }

}