using CYF.Classes;
using CYF.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;

public static class LuaScriptBinder {
    private static Dictionary<string, DynValue> dict = new Dictionary<string, DynValue>(), battleDict = new Dictionary<string, DynValue>(), alMightyDict = new Dictionary<string, DynValue>();
    internal static readonly MusicManager mgr = new MusicManager();
    internal static readonly NewMusicManager newmgr = new NewMusicManager();

    /// <summary>
    /// Registers C# types with MoonSharp so we can bind them to Lua scripts later.
    /// </summary>
    static LuaScriptBinder() {
        Assembly cyfAssembly = Assembly.GetExecutingAssembly();
        foreach (Type t in cyfAssembly.GetTypes()) {
            Attribute luaClassAttr = t.GetCustomAttribute(typeof(CYFLuaClassAttribute));
            if (luaClassAttr != null) {
                CYFLuaClassAttribute luaClass = (CYFLuaClassAttribute)luaClassAttr;
                if (luaClass != null) {
                    if (luaClass.descriptor == null)
                        UserData.RegisterType(t, InteropAccessMode.Default, luaClass.friendlyName);
                    else
                        UserData.RegisterType((MoonSharp.Interpreter.Interop.IUserDataDescriptor)cyfAssembly.CreateInstance(luaClass.descriptor.Name));
                }
            }
        }
    }

    /// <summary>
    /// Generates Script object with globally defined functions and objects bound, and the os/io/file modules taken out.
    /// </summary>
    /// <returns>Script object for use within Unitale</returns>
    public static Script BoundScript(ScriptTemplate scriptTemplate) {
        Script script = new Script(CoreModules.Preset_Complete ^ CoreModules.IO ^ CoreModules.OS_System) { Options = { ScriptLoader = new FileSystemScriptLoader() } };
        // library support
        ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = new[] { FileLoader.PathToModFile("Lua/?.lua"), FileLoader.PathToDefaultFile("Lua/?.lua"), FileLoader.PathToModFile("Lua/Libraries/?.lua"), FileLoader.PathToDefaultFile("Lua/Libraries/?.lua") };

        // ScriptTemplate types to copy from
        Type[] scriptTemplates = new Type[2];
        scriptTemplates[0] = typeof(ScriptTemplate);
        switch (scriptTemplate.scriptType) {
            case ScriptType.ENCOUNTER:
                scriptTemplates[1] = typeof(EncounterScriptTemplate);
                break;
            case ScriptType.ENEMY:
                scriptTemplates[1] = typeof(EnemyScriptTemplate);
                break;
            case ScriptType.WAVE:
                scriptTemplates[1] = typeof(WaveScriptTemplate);
                break;
            case ScriptType.EVENT:
                scriptTemplates[1] = typeof(EventScriptTemplate);
                break;
            case ScriptType.SHOP:
                scriptTemplates[1] = typeof(ShopScriptTemplate);
                break;
        }

        // Add Methods and Properties from script template types to new MoonSharp script
        foreach (Type templateType in scriptTemplates) {
            // Methods (functions)
            foreach (MethodInfo mi in templateType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)) {
                if (mi.GetCustomAttribute(typeof(CYFDontAddToScriptAttribute)) != null)
                    continue;
                if (mi.GetCustomAttribute(typeof(MoonSharpHiddenAttribute)) != null)
                    continue;
                if (mi.Name.StartsWith("get_") || mi.IsConstructor)
                    continue;

                // Availability check
                Attribute availabilityAttr = mi.GetCustomAttribute(typeof(CYFScriptAvailabilityAttribute));
                if (availabilityAttr != null) {
                    CYFScriptAvailabilityAttribute availabilityAttribute = (CYFScriptAvailabilityAttribute)availabilityAttr;
                    if (availabilityAttribute != null)
                        if (!availabilityAttribute.availability.Any((ScriptType type) => type == scriptTemplate.scriptType))
                            continue;
                }

                // Add method to script

                // Exclusive to .NET 4.x or greater:
                script.Globals[mi.Name] = mi.CreateDelegate(Expression.GetDelegateType(mi.GetParameters().Select((ParameterInfo p) => p.ParameterType).Concat(new Type[] { mi.ReturnType }).ToArray()), !mi.IsStatic ? scriptTemplate : null);
            }
            // Properties (variables)
            foreach (PropertyInfo pi in templateType.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)) {
                if (pi.GetCustomAttribute(typeof(CYFDontAddToScriptAttribute)) != null)
                    continue;

                // Availability check
                Attribute availabilityAttr = pi.GetCustomAttribute(typeof(CYFScriptAvailabilityAttribute));
                if (availabilityAttr != null) {
                    CYFScriptAvailabilityAttribute availabilityAttribute = (CYFScriptAvailabilityAttribute)availabilityAttr;
                    if (availabilityAttribute != null)
                        if (!availabilityAttribute.availability.Any((ScriptType type) => type == scriptTemplate.scriptType))
                            continue;
                }

                // Add value to script
                script.Globals[pi.Name] = DynValue.FromObject(script, pi.GetGetMethod().Invoke(scriptTemplate, new object[0]));
            }
        }

        return script;
    }

    //////////

    public static DynValue Get(Script script, string key) {
        if (key == null)
            throw new CYFException("GetRealGlobal: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
        if (!dict.ContainsKey(key))
            return null;
        return dict[key];
    }

    public static void Set(Script script, string key, DynValue value) {
        if (key == null)
            throw new CYFException("SetRealGlobal: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");

        if (value.Type != DataType.Number && value.Type != DataType.String && value.Type != DataType.Boolean && value.Type != DataType.Nil) {
            UnitaleUtil.WriteInLogAndDebugger("SetRealGlobal: The value \"" + key + "\" can't be saved in the savefile because it is a " + value.Type.ToString().ToLower() + ".");
            return;
        }

        if (dict.ContainsKey(key)) dict[key] = value;
        else                       dict.Add(key, value);
    }

    public static DynValue GetBattle(Script script, string key) {
        if (key == null)
            throw new CYFException("GetGlobal: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
        if (!battleDict.ContainsKey(key)) return null;
        // Due to how MoonSharp tables require an owner, we have to create an entirely new table if we want to work with it in other scripts.
        if (battleDict[key].Type != DataType.Table) return battleDict[key];
        DynValue t = DynValue.NewTable(script);
        foreach (TablePair pair in battleDict[key].Table.Pairs)
            t.Table.Set(pair.Key, pair.Value);
        return t;
    }

    public static void SetBattle(Script script, string key, DynValue value) {
        if (key == null)
            throw new CYFException("SetGlobal: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
        if (battleDict.ContainsKey(key))
            battleDict.Remove(key);
        battleDict.Add(key, value);
    }

    public static DynValue GetAlMighty(Script script, string key) {
        if (key == null)
            throw new CYFException("GetAlMightyGlobal: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");
        if (!alMightyDict.ContainsKey(key))
            return null;
        return alMightyDict[key];
    }

    public static void SetAlMighty(Script script, string key, DynValue value) { SetAlMighty(script, key, value, true); }
    public static void SetAlMighty(Script script, string key, DynValue value, bool reload) {
        if (key == null)
            throw new CYFException("SetAlMightyGlobal: The first argument (the index) is nil.\n\nSee the documentation for proper usage.");

        if (value.Type != DataType.Number && value.Type != DataType.String && value.Type != DataType.Boolean && value.Type != DataType.Nil) {
            UnitaleUtil.WriteInLogAndDebugger("SetAlMightyGlobal: The value \"" + key + "\" can't be saved in the almighties because it is a " + value.Type.ToString().ToLower() + ".");
            return;
        }

        if (alMightyDict.ContainsKey(key))
            alMightyDict.Remove(key);
        alMightyDict.Add(key, value);
        if (reload)
            SaveLoad.SaveAlMighty();
    }

    /// <summary>
    /// Clears the global dictionary. Used in the reset functionality, as everything is reinitialized.
    /// </summary>
    public static void Clear() { dict.Clear(); }

    public static void ClearBattleVar() {
        Dictionary<string, DynValue> a = dict;
        battleDict.Clear();
        dict = a;
    }

    public static void ClearAlMighty() {
        alMightyDict.Clear();
        SaveLoad.SaveAlMighty();
    }

    public static void ClearVariables() {
        dict.Clear();
    }

    /// <summary>
    /// Returns this script's dictionaries
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, DynValue> GetSavedDictionary()     { return dict; }
    public static Dictionary<string, DynValue> GetBattleDictionary()    { return battleDict; }
    public static Dictionary<string, DynValue> GetAlMightyDictionary()  { return alMightyDict; }

    /// <summary>
    /// Replaces the current dictionary with the given one.
    /// /!\ THIS ERASES THE CURRENT DICTIONARY /!\
    /// </summary>
    /// <param name="newDict"></param>
    public static void SetSavedDictionary(Dictionary<string, DynValue> newDict)          { dict = newDict; }
    public static void SetBattleDictionary(Dictionary<string, DynValue> newDict)    { battleDict = newDict; }
    public static void SetAlMightyDictionary(Dictionary<string, DynValue> newDict)  { alMightyDict = newDict; }

    /// <summary>
    /// Removes one or several keys from the dictionaries.
    /// </summary>
    /// <param name="str"></param>
    public static void Remove(string str)                { dict.Remove(str); }
    public static void Remove(List<string> list)         { foreach (string str in list) dict.Remove(str); }
    public static void Remove(string[] strs)             { foreach (string str in strs) dict.Remove(str); }
    public static void RemoveBattle(string str)          { battleDict.Remove(str); }
    public static void RemoveBattle(List<string> list)   { foreach (string str in list) battleDict.Remove(str); }
    public static void RemoveBattle(string[] strs)       { foreach (string str in strs) battleDict.Remove(str); }
    public static void RemoveAlMighty(string str)        { alMightyDict.Remove(str); }
    public static void RemoveAlMighty(List<string> list) { foreach (string str in list) alMightyDict.Remove(str); }
    public static void RemoveAlMighty(string[] strs)     { foreach (string str in strs) alMightyDict.Remove(str); }

    /// <summary>
    /// Returns a list that contains all keys that contains the string given in argument.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static List<string> GetKeysWithString(string str) {
        List<string> list = new List<string>();
        foreach (string key in dict.Keys)
            if (key.Contains(str))
                list.Add(key);
        return list;
    }

    public static void CopyToBattleVar() {
        dict["CYFSwitch"] = DynValue.NewBoolean(true);
        foreach (string key in dict.Keys) {
            DynValue temp;
            dict.TryGetValue(key, out temp);
            SetBattle(null, key, temp);
        }
    }

    public static void SetPPCollision(bool b) {
        ProjectileController.globalPixelPerfectCollision = b;
        foreach (LuaProjectile p in GameObject.Find("Canvas").GetComponentsInChildren<LuaProjectile>(true))
            if (!p.ppchanged)
                p.ppcollision = b;
    }

    // public static void SetPPAlphaLimit(float f) {
    //     if (f < 0 || f > 1)  UnitaleUtil.DisplayLuaError("Pixel-Perfect alpha limit", "The alpha limit should be between 0 and 1.");
    //     else                 ControlPanel.instance.MinimumAlpha = f;
    // }
}
