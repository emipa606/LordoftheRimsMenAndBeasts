using System;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace AR_PawnShields;

public readonly struct LocalVar : IEquatable<LocalVar>
{
    private readonly LocalVariableInfo local;

    public bool IsPinned => local.IsPinned;

    public int LocalIndex => local.LocalIndex;

    public Type LocalType => local.LocalType;

    internal LocalVar(LocalVariableInfo local)
    {
        this.local = local;
    }

    public CodeInstruction ToLdloca()
    {
        // ILGenerator.Emit(OpCodes.Ldloca, LocalBuilder) automatically emits the proper opcode and operand.
        if (local is LocalBuilder localBuilder)
        {
            return new CodeInstruction(OpCodes.Ldloca, localBuilder);
        }

        var index = LocalIndex;
        return index <= byte.MaxValue
            ? new CodeInstruction(OpCodes.Ldloca_S, (byte)index)
            : new CodeInstruction(OpCodes.Ldloca, (short)index);
    }

    public override bool Equals(object obj)
    {
        return obj is LocalVar other && LocalIndex == other.LocalIndex;
    }

    public bool Equals(LocalVar other)
    {
        return LocalIndex == other.LocalIndex;
    }

    public static bool operator ==(LocalVar lhs, LocalVar rhs)
    {
        return lhs.LocalIndex == rhs.LocalIndex;
    }

    public static bool operator !=(LocalVar lhs, LocalVar rhs)
    {
        return lhs.LocalIndex != rhs.LocalIndex;
    }

    public override int GetHashCode()
    {
        return LocalIndex;
    }

    public override string ToString()
    {
        return local.ToString();
    }
}