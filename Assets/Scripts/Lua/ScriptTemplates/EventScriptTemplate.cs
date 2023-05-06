using CYF.Classes.Overworld;
using MoonSharp.Interpreter;

namespace CYF.Scripts {
public class EventScriptTemplate : ScriptTemplate {
    [MoonSharpHidden] public override ScriptType scriptType { get { return ScriptType.EVENT; } }

    public static LuaPlayerOW FPlayer { get { return EventManager.instance.luaPlayerOw; } }
    public static LuaEventOW FEvent { get { return EventManager.instance.luaEventOw; } }
    public static LuaGeneralOW FGeneral { get { return EventManager.instance.luaGeneralOw; } }
    public static LuaInventoryOW FInventory { get { return EventManager.instance.luaInventoryOw; } }
    public static LuaScreenOW FScreen { get { return EventManager.instance.luaScreenOw; } }
    public static LuaMapOW FMap { get { return EventManager.instance.luaMapOw; } }
}
}
