using CYF.Classes;
using MoonSharp.Interpreter;

namespace CYF.Scripts {
public class EnemyScriptTemplate : ScriptTemplate {
    [MoonSharpHidden] public override ScriptType scriptType { get { return ScriptType.ENEMY; } }

    private EnemyController enemyReference;
    public EnemyScriptTemplate(EnemyController enemyReference) {
        this.enemyReference = enemyReference;
    }

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

    [CYFDontAddToScript] public static string name { get { return null; } }
    [CYFDontAddToScript] public static string sprite { get { return null; } }
    [CYFDontAddToScript] public static DynValue commands { get { return null; } }
    [CYFDontAddToScript] public static DynValue comments { get { return null; } }
    [CYFDontAddToScript] public static DynValue randomdialogue { get { return null; } }
    [CYFDontAddToScript] public static string check { get { return null; } }
    [CYFDontAddToScript] public static int hp { get { return 0; } }
    [CYFDontAddToScript] public static int maxhp { get { return 0; } }
    [CYFDontAddToScript] public static int atk { get { return 0; } }
    [CYFDontAddToScript] public static int def { get { return 0; } }
    [CYFDontAddToScript] public static int xp { get { return 0; } }
    [CYFDontAddToScript] public static int gold { get { return 0; } }
    [CYFDontAddToScript] public static bool canspare { get { return false; } }
    [CYFDontAddToScript] public static bool cancheck { get { return false; } }
    [CYFDontAddToScript] public static bool unkillable { get { return false; } }
    [CYFDontAddToScript] public static string dialogueprefix { get { return null; } }
    [CYFDontAddToScript] public static string font { get { return null; } }
    [CYFDontAddToScript] public static string voice { get { return null; } }
    [CYFDontAddToScript] public static string defensemisstext { get { return null; } }
    [CYFDontAddToScript] public static string noattackmisstext { get { return null; } }
    [CYFDontAddToScript] public static string bubbleside { get { return null; } }
    [CYFDontAddToScript] public static int bubblewidth { get { return 0; } }
    [CYFDontAddToScript] public static DynValue currentdialogue { get { return null; } }
    [CYFDontAddToScript] public static int posx { get { return 0; } }
    [CYFDontAddToScript] public static int posy { get { return 0; } }

    public static bool isactive { get { return true; } }
    public static bool canmove { get { return true; } }
    public LuaSpriteController monstersprite { get { return enemyReference.sprite; } }
    public DynValue bubblesprite { get { return UserData.Create(LuaSpriteController.GetOrCreate(enemyReference.bubbleObject), LuaSpriteController.data); } }
    public DynValue textobject { get { return UserData.Create(enemyReference.bubbleObject.GetComponentInChildren<LuaTextManager>()); } }

    public void SetSprite(string filename) { enemyReference.SetSprite(filename); }
    public void SetActive(bool active) { enemyReference.SetActive(active); }
    public void Kill(bool playSound = true) { enemyReference.DoKill(playSound); }
    public void Spare(bool playSound = true) { enemyReference.DoSpare(playSound); }
    public void Move(float x, float y) { enemyReference.Move(x, y); }
    public void MoveTo(float x, float y) { enemyReference.MoveTo(x, y); }
    public void BindToArena(bool bind, bool isUnderArena = false) { enemyReference.BindToArena(bind, isUnderArena); }
    public void SetDamage(int dmg) { enemyReference.SetDamage(dmg); }
    public void SetBubbleOffset(int x, int y) { enemyReference.SetBubbleOffset(x, y); }
    public void SetDamageUIOffset(int x, int y) { enemyReference.SetDamageUIOffset(x, y); }
    public void SetSliceAnimOffset(int x, int y) { enemyReference.SetSliceAnimOffset(x, y); }
    public void Remove() { enemyReference.Remove(); }

    [CYFDontAddToScript] public static void HandleCustomCommand(string command) {}
    [CYFDontAddToScript] public static void HandleAttack(int damage) {}
    [CYFDontAddToScript] public static void BeforeDamageCalculation() {}
    [CYFDontAddToScript] public static void BeforeDamageValues(int damage) {}
    [CYFDontAddToScript] public static void OnSpare() {}
    [CYFDontAddToScript] public static void OnDeath() {}
}
}
