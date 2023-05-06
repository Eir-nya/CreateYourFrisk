using CYF.Classes;
using MoonSharp.Interpreter;

namespace CYF.Scripts {
public class WaveScriptTemplate : ScriptTemplate {
    [MoonSharpHidden] public override ScriptType scriptType { get { return ScriptType.WAVE; } }

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

    [CYFDontAddToScript] public static string wavename { get { return null; } }

    [CYFDontAddToScript] public static void Update() {}
    [CYFDontAddToScript] public static void EndingWave() {}
    public static void EndWave() { UIController.instance.encounter.EndWaveTimer(); }
}
}
