using Dalamud.Game.ClientState.Objects;
using ECommons.GameFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace Splatoon.SplatoonScripting
{
    public unsafe static class Extensions
    {
        /// <summary>
        /// Gets object by it's object ID.
        /// </summary>
        /// <param name="objectID">Object ID to search.</param>
        /// <returns>GameObject if found; null otherwise.</returns>
        public static GameObject? GetObject(this uint objectID)
        {
            return Svc.Objects.FirstOrDefault(x => x.ObjectId == objectID);
        }

        /// <summary>
        /// Attempts to get object by it's object ID.
        /// </summary>
        /// <param name="objectID">Object ID to search.</param>
        /// <param name="obj">Resulting GameObject if found; null otherwise.</param>
        /// <returns>Whether object was found.</returns>
        public static bool TryGetObject(this uint objectID, [NotNullWhen(true)]out GameObject? obj)
        {
            obj = objectID.GetObject();
            return obj != null;
        }

        /// <summary>
        /// Gets Status VFX ID.
        /// </summary>
        /// <param name="chara"></param>
        /// <returns>Status VFX ID</returns>
        public static short GetStatusVFXId(this Character chara)
        {
            return chara.Struct()->StatusEffectVFXId;
        }
    }
}
