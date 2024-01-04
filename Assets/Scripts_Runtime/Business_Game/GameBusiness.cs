using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit.Business.Game {

    public static class GameBusiness {

        public static void Init(GameBusinessContext ctx) {

            // Game
            Physics.IgnoreLayerCollision(8, 8, true);

            // Input
            var inputEntity = ctx.inputEntity;
            inputEntity.Ctor();
            inputEntity.Keybinding_Set(InputKeyEnum.MoveLeft, new KeyCode[] { KeyCode.A });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveRight, new KeyCode[] { KeyCode.D });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveForward, new KeyCode[] { KeyCode.W });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveBackward, new KeyCode[] { KeyCode.S });
            inputEntity.Keybinding_Set(InputKeyEnum.Jump, new KeyCode[] { KeyCode.Space });
            inputEntity.Keybinding_Set(InputKeyEnum.Interact, new KeyCode[] { KeyCode.E });
            inputEntity.Keybinding_Set(InputKeyEnum.Cancel, new KeyCode[] { KeyCode.Escape, KeyCode.Mouse1 });
            inputEntity.Keybinding_Set(InputKeyEnum.Attack, new KeyCode[] { KeyCode.Mouse0 });

        }

        public static void TearDown(GameBusinessContext ctx) {
            ExitGame(ctx);
        }

        static void ExitGame(GameBusinessContext ctx) {

            // Tear Down
            // - Entities

            // - UI

            // - HUD

        }

        public static void ExitApplicaiton(GameBusinessContext ctx) {
            ExitGame(ctx);
            Application.Quit();
            HLog.Log("Application.Quit()");
        }

        static void RestartGame(GameBusinessContext ctx) {
            ExitGame(ctx);
            StartGame(ctx, ctx.gameEntity.gameTypeID);
        }

        public static void StartGame(GameBusinessContext ctx, int gameTypeID) {

            // Spawn
            // - Entities
            // - UI
            // - Game

        }

        public static void Tick(GameBusinessContext ctx, float dt) {

        }

        static void ProcessInput(GameBusinessContext ctx, float dt) {
            InputEntity inputEntity = ctx.inputEntity;
            inputEntity.ProcessInput(ctx.cameraCoreContext.MainCamera, dt);
        }

        static void LogicTick(GameBusinessContext ctx, float dt) {

        }

        static void FixedTick(GameBusinessContext ctx, float dt) {

        }

        static void LateTick(GameBusinessContext ctx, float dt) {

        }

        // UI Events



    }

}