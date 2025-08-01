using HarmonyLib;
using Verse;

namespace AR_PawnShields;

[HarmonyPatch(typeof(PawnRenderer), nameof(PawnRenderer.RenderPawnAt))]
public static class PawnRenderer_RenderPawnAt
{
    public static void Postfix(Pawn ___pawn)
    {
        //Render shield.
        if (___pawn.GetShield() is not { } shield)
        {
            return;
        }

        var shieldComp = shield.GetCompShield();

        shieldComp.RenderShield(shield, ___pawn);
    }
}