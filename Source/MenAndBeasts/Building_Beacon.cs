using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace MenAndBeasts
{
    public class Building_Beacon : Building_WorkTable
    {
        private bool allySummoned = false;
        public override void Tick()
        {
            base.Tick();
            if (!allySummoned)
            {
                allySummoned = true;
                var potentialAllies = Find.World.factionManager.AllFactionsVisible.Where(x =>
                    !x.IsPlayer && x.PlayerRelationKind == FactionRelationKind.Ally);
                if (potentialAllies == null || potentialAllies.Count() == 0)
                {
                    Messages.Message("LotRM_BeaconLitNoAllies".Translate(LocalFaction.Name), this, MessageTypeDefOf.NegativeEvent);
                    return;
                }

                var potentialAlly = potentialAllies.RandomElementByWeight((Faction fac) => (float)fac.PlayerGoodwill + 120.000008f);
                
                Faction ofPlayer = Faction.OfPlayer;
                var goodwillChange = -25;
                var canSendMessage = false;
                string reason = "GoodwillChangedReason_RequestedMilitaryAid".Translate();
                potentialAlly.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, true, reason, null);
                var incidentParms = new IncidentParms
                {
                    target = MapHeld,
                    faction = potentialAlly,
                    raidArrivalModeForQuickMilitaryAid = true,
                    points = DiplomacyTuning.RequestedMilitaryAidPointsRange.RandomInRange
                };
                potentialAlly.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
                IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
            }
        }



        private Faction LocalFaction => Faction ?? Faction.OfPlayerSilentFail;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref allySummoned, "allySummoned");
        }
    }
}