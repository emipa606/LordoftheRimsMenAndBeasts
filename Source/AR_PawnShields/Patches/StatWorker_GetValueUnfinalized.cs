using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;

namespace AR_PawnShields;

[HarmonyPatch(typeof(StatWorker), nameof(StatWorker.GetValueUnfinalized))]
public static class StatWorker_GetValueUnfinalized
{
    public static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions,
        MethodBase method, ILGenerator ilGen)
    {
        var instructionList = instructions.AsList();
        var locals = new Locals(method);

        var pawnEquipmentIndex = instructionList.FindSequenceIndex(
            Locals.IsLdloc,
            instruction => instruction.LoadsField(HarmonyPatches.pawnEquipmentField),
            instruction => !instruction.IsBrfalse());
        var targetLabel = (Label)instructionList[pawnEquipmentIndex + 3].operand;

        var resultStoreIndex = instructionList.FindIndex(pawnEquipmentIndex + 3,
            instruction => locals.IsStloc(instruction, out var local) && local.LocalType == typeof(float));

        var insertionIndex = instructionList.FindIndex(pawnEquipmentIndex + 3,
            instruction => instruction.labels.Contains(targetLabel));

        var labelsToTransfer = instructionList
            .GetRange(pawnEquipmentIndex + 3, insertionIndex - (pawnEquipmentIndex + 3))
            .Where(instruction => instruction.operand is Label)
            .Select(instruction => (Label)instruction.operand);
        instructionList.SafeInsertRange(insertionIndex, [
            locals.FromStloc(instructionList[resultStoreIndex]).ToLdloca(), // &result
            instructionList[pawnEquipmentIndex].Clone(), // pawn...
            instructionList[pawnEquipmentIndex + 1].Clone(), // ...equipment
            new CodeInstruction(OpCodes.Ldarg_0), // this...
            new CodeInstruction(OpCodes.Ldfld, HarmonyPatches.statWorkerStatField), // ...stat
            new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(HarmonyPatches), nameof(HarmonyPatches.StatWorkerInjection_AddShieldValue)))
        ], labelsToTransfer);

        return instructionList;
    }
}