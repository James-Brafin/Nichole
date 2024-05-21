using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole
{
    [HarmonyPatch]
    internal sealed class NegativeOverdriveManager
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Ship), "CanBeNegative")]
        private static void Ship_CanBeNegative_Postfix(Status status, ref bool __result)
        {
            if (status == Status.overdrive)
                __result = true;
        }
    }
}
