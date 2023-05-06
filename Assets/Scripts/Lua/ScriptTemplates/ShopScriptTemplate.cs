using CYF.Classes;
using MoonSharp.Interpreter;
using UnityEngine;

namespace CYF.Scripts {
public class ShopScriptTemplate : ScriptTemplate {
    [MoonSharpHidden] public override ScriptType scriptType { get { return ScriptType.SHOP; } }

    private ShopScript shopReference;
    public ShopScriptTemplate(ShopScript shopReference) {
        this.shopReference = shopReference;
    }

    public static DynValue background { get { return UserData.Create(GameObject.Find("Background"), LuaSpriteController.data); } }
    [CYFDontAddToScript] public static string returnscene { get { return null; } }
    [CYFDontAddToScript] public static DynValue returnpos { get { return null; } }
    [CYFDontAddToScript] public static int returndir { get { return 0; } }
    [CYFDontAddToScript] public static string music { get { return null; } }
    [CYFDontAddToScript] public static DynValue buylist { get { return null; } }
    [CYFDontAddToScript] public static DynValue talklist { get { return null; } }
    [CYFDontAddToScript] public static string maintalk { get { return null; } }
    [CYFDontAddToScript] public static string buytalk { get { return null; } }
    [CYFDontAddToScript] public static string talktalk { get { return null; } }
    [CYFDontAddToScript] public static string exittalk { get { return null; } }
    [CYFDontAddToScript] public static bool playerskipdocommand { get { return false; } }

    public void Interrupt(DynValue text, string nextstate = "MENU") { shopReference.Interrupt(text, nextstate); }

    [CYFDontAddToScript] public static void Start() {}
    [CYFDontAddToScript] public static void Update() {}
    [CYFDontAddToScript] public static void EnterMenu() {}
    [CYFDontAddToScript] public static void EnterBuy() {}
    [CYFDontAddToScript] public static void SuccessBuy(string itemName) {}
    [CYFDontAddToScript] public static void FailBuy(string error) {}
    [CYFDontAddToScript] public static void ReturnBuy() {}
    [CYFDontAddToScript] public static void EnterSell() {}
    [CYFDontAddToScript] public static void SuccessSell(string itemName) {}
    [CYFDontAddToScript] public static void FailSell(string error) {}
    [CYFDontAddToScript] public static void ReturnSell() {}
    [CYFDontAddToScript] public static void EnterTalk() {}
    [CYFDontAddToScript] public static void SuccessTalk(string talkOption) {}
    [CYFDontAddToScript] public static void EnterExit() {}
    [CYFDontAddToScript] public static void OnInterrupt(string newState) {}
}
}
