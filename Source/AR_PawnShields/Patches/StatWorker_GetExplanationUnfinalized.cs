using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using RimWorld;

namespace AR_PawnShields;

[HarmonyPatch(typeof(StatWorker), nameof(StatWorker.GetExplanationUnfinalized))]
public static class StatWorker_GetExplanationUnfinalized
{
    public static IEnumerable<CodeInstruction> Transpiler_StatWorker_GetExplanationUnfinalized(
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


        var stringBuilderIndex = instructionList.FindIndex(pawnEquipmentIndex + 3,
            instruction => locals.IsLdloc(instruction, out var local) && local.LocalType == typeof(StringBuilder));

        var insertionIndex = instructionList.FindIndex(pawnEquipmentIndex + 3,
            instruction => instruction.labels.Contains(targetLabel));


        var labelsToTransfer = instructionList
            .GetRange(pawnEquipmentIndex + 3, insertionIndex - (pawnEquipmentIndex + 3))
            .Where(instruction => instruction.operand is Label)
            .Select(instruction => (Label)instruction.operand);

        instructionList.SafeInsertRange(insertionIndex, [
            instructionList[stringBuilderIndex].Clone(), // stringBuilder
            instructionList[pawnEquipmentIndex].Clone(), // pawn...
            instructionList[pawnEquipmentIndex + 1].Clone(), // ...equipment
            new CodeInstruction(OpCodes.Ldarg_0), // this...
            new CodeInstruction(OpCodes.Ldfld, HarmonyPatches.statWorkerStatField), // ...stat
            new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(HarmonyPatches),
                    nameof(HarmonyPatches.StatWorkerInjection_BuildShieldString)))
        ], labelsToTransfer);


        return instructionList;
    }
}