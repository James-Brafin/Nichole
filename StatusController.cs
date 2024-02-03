using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace JamesBrafin.Nichole
{
    [HarmonyPatch]
    internal class StatusController
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin))]
        private static void DoEnflame(AAttack __instance, G g, State s, Combat c)
        {
            Ship attacker = (__instance.targetPlayer ? s.ship : c.otherShip);
            Ship defender = (__instance.targetPlayer ? c.otherShip : s.ship);
            if (attacker.Get((Status)MainManifest.statuses["enflame"].Id) <= 0) return;

            foreach(var part in attacker)
            {
                if(part.type == PType.cannon)
                {
                    Part partAtWorldX = defender.GetPartAtWorldX(num);
                    if (partAtWorldX.type != PType.empty)
                    {
                        c.QueueImmediate(new AStatus
                        {
                            targetPlayer = !__instance.targetPlayer,
                            status = Status.Heat,
                            statusAmount = attacker.Get((Status)MainManifest.statuses["enflame"].Id)
                        });
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin))]
        private static void DoCryo(AAttack __instance, G g, State s, Combat c)
        {
            Ship ship = (__instance.targetPlayer ? c.otherShip : s.ship);
            if (ship.Get((Status)MainManifest.statuses["Cryo"].Id) <= 0) return;

            c.QueueImmediate(new AHurt
            {
                targetPlayer != __instance.targetPlayer,
                hurtAmount = ship.Get((Status)MainManifest.statuses["Cryo"].Id)
            });

            ClearStatus(ship, (Status)MainManifest.statuses["cryo"].Id);
        }

        public static void ClearStatus(Ship ship, Status status)
        {
            var stacks = ship.Get(status);
            ship.Set(status, 0);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Ship), nameof(Ship.OnBeginTurn))]
        public static void HarmonyPostfix_Enflame_Cleanup(Ship __instance, State s, Combat c)
        {
            ClearStatus(__instance, (Status)MainManifest.statuses["enflame"].Id);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Ship), nameof(Ship.OnBeginTurn))]
        public static void HarmonyPostfix_Cryo_Cleanup(Ship __instance, State s, Combat c)
        {
            ClearStatus(__instance, (Status)MainManifest.statuses["cryo"].Id);
        }
    }
}
