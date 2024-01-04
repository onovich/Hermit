using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermit.Business.Game;
using Hermit.Login;
using UnityEngine;

namespace Hermit {

    public class ClientMain : MonoBehaviour {

        ClientMainContext ctx;

        void Start() {

            Canvas mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
            Transform hudFakeCanvas = GameObject.Find("HUDFakeCanvas").transform;
            Camera mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

            ctx = new ClientMainContext();

            ctx.Inject(mainCanvas, hudFakeCanvas, mainCamera);

            Binding();

            Action onAssetsLoad = async () => {
                try {
                    await LoadAssets();
                    Init();
                    Enter();
                    ctx.isLoadedAssets = true;
                } catch (Exception e) {
                    Debug.LogError(e);
                }
            };
            onAssetsLoad.Invoke();

        }

        async Task LoadAssets() {

            UIAppContext uiAppContext = ctx.BakeUIApp();
            AssetsInfraContext assetsInfraContext = ctx.assetsInfraContext;
            TemplateInfraContext templateInfraContext = ctx.templateInfraContext;

            await UIApp.LoadAssets(uiAppContext);
            await AssetsInfra.LoadAssets(assetsInfraContext);
            await TemplateInfra.LoadAssets(templateInfraContext);

        }

        void Init() {
            GameBusinessContext gameBusinessContext = ctx.BakeGameBusiness();
            Application.targetFrameRate = 120;
            GameBusiness.Init(gameBusinessContext);
            UIApp.Init(ctx.BakeUIApp());
        }

        void Enter() {
            LoginBusinessContext loginBusinessContext = ctx.BakeLoginBusiness();
            LoginBusiness.Enter(loginBusinessContext);
        }

        void Binding() {
            LoginBusinessContext loginBusinessContext = ctx.BakeLoginBusiness();

            // Login Business
            LoginEventCenter loginEvt = loginBusinessContext.evt;
            loginEvt.Login_OnLoginFinishHandle += () => {
                Debug.Log("Login Success");
            };

        }

        void Update() {

            if (!ctx.isLoadedAssets) {
                return;
            }

            LoginBusinessContext loginBusinessContext = ctx.BakeLoginBusiness();
            GameBusinessContext gameBusinessContext = ctx.BakeGameBusiness();

            float dt = Time.deltaTime;
            LoginBusiness.Tick(loginBusinessContext, dt);
            GameBusiness.Tick(gameBusinessContext, dt);

            UIAppContext uiAppContext = ctx.BakeUIApp();

            UIApp.Tick(uiAppContext, dt);

        }

        void OnDestroy() {
            TearDown();
        }

        void OnApplicationQuit() {
            TearDown();
        }

        void TearDown() {
            if (ctx.isTearDown) {
                return;
            }
            ctx.isTearDown = true;

            ctx.BakeLoginBusiness().evt.Clear();
            ctx.BakeUIApp().evt.Clear();

            GameBusiness.TearDown(ctx.BakeGameBusiness());
            // AssetsInfra.ReleaseAssets(ctx.assetsInfraContext);

        }
    }

}