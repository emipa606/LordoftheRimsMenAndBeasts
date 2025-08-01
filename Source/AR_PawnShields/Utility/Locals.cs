using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace AR_PawnShields;

public class Locals(MethodBase method)
{
    private readonly IList<LocalVariableInfo> locals = method.GetMethodBody()?.LocalVariables;

    public static bool IsLdloc(CodeInstruction instruction)
    {
        return
            instruction.opcode == OpCodes.Ldloc ||
            instruction.opcode == OpCodes.Ldloc_S ||
            instruction.opcode == OpCodes.Ldloc_0 ||
            instruction.opcode == OpCodes.Ldloc_1 ||
            instruction.opcode == OpCodes.Ldloc_2 ||
            instruction.opcode == OpCodes.Ldloc_3;
    }

    public bool IsLdloc(CodeInstruction instruction, out LocalVar local)
    {
        if (instruction.opcode == OpCodes.Ldloc || instruction.opcode == OpCodes.Ldloc_S)
        {
            local = new LocalVar(
                (LocalVariableInfo)instruction.operand); // note: LocalBuilder derives from LocalVariableInfo
        }
        else if (instruction.opcode == OpCodes.Ldloc_0)
        {
            local = new LocalVar(locals[0]);
        }
        else if (instruction.opcode == OpCodes.Ldloc_1)
        {
            local = new LocalVar(locals[1]);
        }
        else if (instruction.opcode == OpCodes.Ldloc_2)
        {
            local = new LocalVar(locals[2]);
        }
        else if (instruction.opcode == OpCodes.Ldloc_3)
        {
            local = new LocalVar(locals[3]);
        }
        else
        {
            local = default;
            return false;
        }

        return true;
    }

    public LocalVar FromStloc(CodeInstruction instruction)
    {
        return IsStloc(instruction, out var local)
            ? local
            : throw new ArgumentException($"Expected stloc-type instruction, actual instruction: {instruction}");
    }

    public bool IsStloc(CodeInstruction instruction, out LocalVar local)
    {
        if (instruction.opcode == OpCodes.Stloc || instruction.opcode == OpCodes.Stloc_S)
        {
            local = new LocalVar(
                (LocalVariableInfo)instruction.operand); // note: LocalBuilder derives from LocalVariableInfo
        }
        else if (instruction.opcode == OpCodes.Stloc_0)
        {
            local = new LocalVar(locals[0]);
        }
        else if (instruction.opcode == OpCodes.Stloc_1)
        {
            local = new LocalVar(locals[1]);
        }
        else if (instruction.opcode == OpCodes.Stloc_2)
        {
            local = new LocalVar(locals[2]);
        }
        else if (instruction.opcode == OpCodes.Stloc_3)
        {
            local = new LocalVar(locals[3]);
        }
        else
        {
            local = default;
            return false;
        }

        return true;
    }
}