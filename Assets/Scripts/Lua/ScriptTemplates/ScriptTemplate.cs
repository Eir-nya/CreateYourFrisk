using CYF.Classes;
using MoonSharp.Interpreter;
using UnityEngine;

namespace CYF.Scripts {
public enum ScriptType {
    ENCOUNTER,
    ENEMY,
    WAVE,
    EVENT,
    SHOP
}

/// <summary>
/// Takes care of creating <see cref="Script"/> objects with globally bound functions.
/// Doubles as a dictionary for the SetGlobal/GetGlobal functions attached to these scripts.
/// Is also used to store global variables from the game, to be accessed from Lua scripts.
/// </summary>
public abstract class ScriptTemplate {
    [MoonSharpHidden] public abstract ScriptType scriptType { get; }

    // Variables

    public static bool isCYF { get { return true; } }
    public static bool isRetro { get { return GlobalControls.retroMode; } }
    public static bool safe { get { return ControlPanel.instance.Safe; } }
    public static bool windows { get {
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return true;
        #else
        return false;
        #endif
    } }

    public static string CYFversion { get { return GlobalControls.CYFversion; } }
    public static int LTSversion { get { return GlobalControls.LTSversion; } }

    public static MusicManager Audio { get { return LuaScriptBinder.mgr; } }
    public static NewMusicManager NewAudio { get { return LuaScriptBinder.newmgr; } }
    public static LuaInventory Inventory { get { return CYF.Inventory.luaInventory; } }
    public static LuaInputBinding Input { get { return GlobalControls.luaInput; } }
    public static Misc Misc { get { return new Misc(); } }
    public static LuaUnityTime Time { get { return new LuaUnityTime(); } }
    public static LuaDiscord Discord { get { return new LuaDiscord(); } }

    // Functions

    public static void DEBUG(string mess) { UnitaleUtil.WriteInLogAndDebugger(mess); }
    public static void EnableDebugger(bool state) {
        if (UserDebugger.instance == null)
            return;

        UserDebugger.instance.canShow = state;
        if (state || !UserDebugger.instance.gameObject.activeSelf) return;
        UserDebugger.instance.gameObject.SetActive(false);
        Camera.main.GetComponent<FPSDisplay>().enabled = false;
    }

    public static void SetGlobal(Script script, string key, DynValue value) { LuaScriptBinder.SetBattle(script, key, value); }
    public static DynValue GetGlobal(Script script, string key) { return LuaScriptBinder.GetBattle(script, key); }
    public static void SetRealGlobal(Script script, string key, DynValue value) { LuaScriptBinder.Set(script, key, value); }
    public static DynValue GetRealGlobal(Script script, string key) { return LuaScriptBinder.Get(script, key); }
    public static void SetAlMightyGlobal(Script script, string key, DynValue value) { LuaScriptBinder.SetAlMighty(script, key, value, true); }
    public static DynValue GetAlMightyGlobal(Script script, string key) { return LuaScriptBinder.GetAlMighty(script, key); }

    public static void UnloadSprite(string key) { SpriteRegistry.Unload(key); }

    public static DynValue CreateSprite(string filename, string tag = "BelowArena", int childNumber = -1) { return SpriteUtil.MakeIngameSprite(filename, tag, childNumber); }

    public static bool CreateLayer(string name, string relatedTag = "BasisNewest", bool before = false) { return SpriteUtil.CreateLayer(name, relatedTag, before); }

    // Battle-specific

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void SetFrameBasedMovement(bool useFrameBased) { ControlPanel.instance.FrameBasedMovement = useFrameBased; }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void SetAction(string action) { EnemyEncounter.SetAction(action); }

    // [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    // public static void SetPPAlphaLimit(float alphaLimit) { LuaScriptBinder.SetPPAlphaLimit(alphaLimit); }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static LifeBarController CreateBar(float x, float y, float width, float height = 20) { return LifeBarController.Create(x, y, width, height); }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static LifeBarController CreateBarWithSprites(float x, float y, string backgroundSprite, string fillSprite = null) { return LifeBarController.Create(x, y, backgroundSprite, fillSprite); }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static string GetCurrentState() {
        try { return (UIController.instance.frozenState != "PAUSE") ? UIController.instance.frozenState : UIController.instance.state; }
        catch { return "NONE (error)"; }
    }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void BattleDialog(Script scr, DynValue arg) { EnemyEncounter.BattleDialog(scr, arg); }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void BattleDialogue(Script scr, DynValue arg) { EnemyEncounter.BattleDialog(scr, arg); }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void CreateState(string name) { UIController.CreateNewUIState(name); }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void State(Script scr, string state) { UIController.SwitchStateOnString(scr, state); }

    public static LuaTextManager CreateText(Script scr, DynValue text, DynValue position, int textWidth, string layer = "BelowPlayer", int bubbleHeight = -1) { return LuaTextManager.CreateText(scr, text, position, textWidth, layer, bubbleHeight); }

    [CYFDontAddToScript] public static void OnTextDisplay(LuaTextManager text) {}
}
}
