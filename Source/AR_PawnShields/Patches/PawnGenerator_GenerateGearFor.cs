using HarmonyLib;
using Verse;

namespace AR_PawnShields;

[HarmonyPatch(typeof(PawnGenerator), "GenerateGearFor")]
public static class PawnGenerator_GenerateGearFor
{
    public static void Postfix(Pawn pawn, ref PawnGenerationRequest request)
    {
        PawnShieldGenerator.TryGenerateShieldFor(pawn, request);
    }
}