using CYF.Classes;
using MoonSharp.Interpreter;

namespace CYF.Scripts {
public class EncounterScriptTemplate : ScriptTemplate {
    [MoonSharpHidden] public override ScriptType scriptType { get { return ScriptType.ENCOUNTER; } }

    // Shared

    [CYFScriptAvailability(ScriptType.ENEMY, ScriptType.WAVE)]
    public static ScriptWrapper Encounter { get { return EnemyEncounter.script; } }
    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static LuaPlayerStatus Player { get { return PlayerController.luaStatus; } }
    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static LuaArenaStatus Arena { get { return ArenaManager.luaStatus; } }
    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static LuaPlayerUI UI { get { return new LuaPlayerUI(); } }

    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void CreateProjectileLayer(string name, string relatedTag = "", bool before = false) { SpriteUtil.CreateProjectileLayer(name, relatedTag, before); }
    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void SetPPCollision(bool usePP) { LuaScriptBinder.SetPPCollision(usePP); }
    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.ENEMY, ScriptType.WAVE)]
    public static void AllowPlayerDef(bool playerDef) { PlayerController.allowplayerdef = playerDef; }
    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.WAVE)]
    public static DynValue CreateProjectile(Script s, string sprite, float xpos, float ypos, string layerName = "") { return EnemyEncounter.CreateProjectile(s, sprite, xpos, ypos, layerName); }
    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.WAVE)]
    public static DynValue CreateProjectileAbs(Script s, string sprite, float xpos, float ypos, string layerName = "") { return EnemyEncounter.CreateProjectileAbs(s, sprite, xpos, ypos, layerName); }
    [CYFScriptAvailability(ScriptType.ENCOUNTER, ScriptType.WAVE)]
    public static void OnHit(ProjectileController p) {}

    // Specific

    [CYFDontAddToScript] public static string music { get { return null; } }
    [CYFDontAddToScript] public static string encountertext { get { return null; } }
    [CYFDontAddToScript] public static DynValue nextwaves { get { return null; } }
    [CYFDontAddToScript] public static float wavetimer { get { return 0; } }
    [CYFDontAddToScript] public static DynValue arenasize { get { return null; } }
    [CYFDontAddToScript] public static DynValue enemies { get { return null; } }
    [CYFDontAddToScript] public static DynValue enemypositions { get { return null; } }
    [CYFDontAddToScript] public static bool autolinebreak { get { return false; } }
    [CYFDontAddToScript] public static bool playerskipdocommand { get { return false; } }
    [CYFDontAddToScript] public static bool unescape { get { return false; } }
    [CYFDontAddToScript] public static bool flee { get { return false; } }
    [CYFDontAddToScript] public static bool fleesuccess { get { return false; } }
    [CYFDontAddToScript] public static DynValue fleetexts { get { return null; } }
    [CYFDontAddToScript] public static bool revive { get { return false; } }
    [CYFDontAddToScript] public static DynValue deathtext { get { return null; } }
    [CYFDontAddToScript] public static string deathmusic { get { return null; } }
    [CYFDontAddToScript] public static DynValue Wave { get { return null; } }
    [CYFDontAddToScript] public static bool noscalerotationbug { get { return false; } }
    [CYFDontAddToScript] public static bool adjusttextdisplay { get { return false; } }

    [CYFScriptAvailability(ScriptType.ENCOUNTER)]
    public static string RandomEncounterText() { return UIController.instance.encounter.RandomEncounterText(); }
    [CYFScriptAvailability(ScriptType.ENCOUNTER)]
    public static void SetButtonLayer(string layer) { EnemyEncounter.SetButtonLayer(layer); }
    [CYFScriptAvailability(ScriptType.ENCOUNTER)]
    public static DynValue CreateEnemy(string enemyScript, float x, float y) { return UIController.instance.encounter.CreateEnemy(enemyScript, x, y); }
    [CYFScriptAvailability(ScriptType.ENCOUNTER)]
    public static void Flee() { UIController.instance.encounter.Flee(); }

    [CYFDontAddToScript] public static void EncounterStarting() {}
    [CYFDontAddToScript] public static void EnteringState(string newState, string oldState) {}
    [CYFDontAddToScript] public static void HandleItem(string itemName, int position) {}
    [CYFDontAddToScript] public static void Update() {}
    [CYFDontAddToScript] public static void BeforeDeath() {}
    [CYFDontAddToScript] public static void DefenseEnding() {}
    [CYFDontAddToScript] public static void EnemyDialogueStarting() {}
    [CYFDontAddToScript] public static void EnemyDialogueEnding() {}
    [CYFDontAddToScript] public static void HandleSpare() {}
    [CYFDontAddToScript] public static void HandleFlee(bool fleeSuccess) {}
}
}
