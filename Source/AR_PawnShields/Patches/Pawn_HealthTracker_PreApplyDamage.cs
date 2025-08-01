using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Sound;

namespace AR_PawnShields;

[HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.PreApplyDamage))]
public static class Pawn_HealthTracker_PreApplyDamage
{
    public static bool Prefix(Pawn ___pawn, ref DamageInfo dinfo, out bool absorbed)
    {
        absorbed = false;

        if (___pawn == null)
        {
            return true;
        }

        //Notify of agressor
        var violence = new DamageInfo(dinfo);
        violence.SetAmount(0);
        ___pawn.mindState.Notify_DamageTaken(violence);

        //Try getting equipped shield.
        var shield = ___pawn.GetShield();
        if (shield == null)
        {
            return true;
        }

        var shieldComp = shield.GetCompShield();

        var shieldSound = shieldComp.BlockSound ?? shieldComp.ShieldProps.defaultSound;
        var discardShield = false;

        //Determine if it is a melee or ranged attack.
        if (shieldComp.ShieldProps.canBlockRanged &&
            dinfo.Instigator != null &&
            !dinfo.Instigator.Position.AdjacentTo8WayOrInside(___pawn.Position) ||
            dinfo.Def.isExplosive)
        {
            //Ranged
            absorbed = shieldComp.AbsorbDamage(___pawn, dinfo, true);
            if (absorbed)
            {
                shieldSound?.PlayOneShot(___pawn);
            }
            //MoteMaker.ThrowText(dinfo.Instigator.DrawPos, dinfo.Instigator.Map, "Ranged absorbed=" + absorbed);

            if (shieldComp.IsBroken)
            {
                discardShield = true;
            }
        }
        else if (shieldComp.ShieldProps.canBlockMelee &&
                 dinfo.Instigator != null &&
                 dinfo.Instigator.Position.AdjacentTo8WayOrInside(___pawn.Position))
        {
            //Melee
            absorbed = shieldComp.AbsorbDamage(___pawn, dinfo, false);
            if (absorbed)
            {
                shieldSound?.PlayOneShot(___pawn);
            }
            //MoteMaker.ThrowText(dinfo.Instigator.DrawPos, dinfo.Instigator.Map, "Melee absorbed=" + absorbed);

            if (shieldComp.IsBroken)
            {
                discardShield = true;
            }
        }

        if (shieldComp.ShieldProps.useFatigue &&
            ___pawn.health.hediffSet.GetFirstHediffOfDef(ShieldHediffDefOf.ShieldFatigue) is { } hediff &&
            hediff.Severity >= hediff.def.maxSeverity)
        {
            discardShield = true;
        }

        //Discard shield either from damage or fatigue.
        if (!shieldComp.ShieldProps.canBeAutoDiscarded || !discardShield)
        {
            return !absorbed;
        }

        if (___pawn.equipment.TryDropEquipment(shield, out var thingWithComps, ___pawn.Position))
        {
            thingWithComps?.SetForbidden(false);
        }
        else
        {
            Log.Error($"{___pawn} couldn't discard shield {shield}");
        }

        return !absorbed;
    }
}