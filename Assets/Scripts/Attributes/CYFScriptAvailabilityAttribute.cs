using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class CYFScriptAvailabilityAttribute : Attribute {
    public readonly ScriptType[] availability;

    public CYFScriptAvailabilityAttribute(params ScriptType[] availability) {
        this.availability = availability;
    }
}
