using System;

[AttributeUsage(AttributeTargets.Class)]
public class CYFLuaClassAttribute : Attribute {
    public string friendlyName;
    public Type descriptor;

    public CYFLuaClassAttribute(string friendlyName) : this(friendlyName, null) { }
    public CYFLuaClassAttribute(string friendlyName, Type descriptor) {
        this.friendlyName = friendlyName;
        this.descriptor = descriptor;
    }
}
