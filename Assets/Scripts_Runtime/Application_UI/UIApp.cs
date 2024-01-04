using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hermit {

    public static class UIApp {

        public static async Task LoadAssets(UIAppContext ctx) {
            var list = await Addressables.LoadAssetsAsync<GameObject>("UI", null).Task;
            foreach (var item in list) {
                ctx.Assets_AddPrefab(item.name, item);
            }
        }

        public static void Init(UIAppContext ctx) {
        }

        public static void Tick(UIAppContext ctx, float dt) {

        }

        public static void Login_Open(UIAppContext ctx) {
            HLog.Log("Login_Open");
        }

        public static void Login_TryClose(UIAppContext ctx) {
            HLog.Log("Login_TryClose");
        }


    }

}
