using System;
using System.Collections.Generic;

namespace Celeste.Mod.HelperTestMapHider {
    public class HelperTestMapHiderModule : EverestModule
    {

        public static List<string> helpers = new List<string> {"BounceHelper", "CustomPoints", "HonlyHelper", "JackalHelper"};
        public override void Load()
        {
            On.Celeste.Mod.Everest.Content.TryAdd += OnTryAdd;
        }

        public override void Unload()
        {
            On.Celeste.Mod.Everest.Content.TryAdd -= OnTryAdd;
        }

        private static bool OnTryAdd(On.Celeste.Mod.Everest.Content.orig_TryAdd orig, string path, ModAsset metadata)
        {
            var cleanPath = path.Replace('\\', '/');

            if (helpers.Contains(metadata.Source.Name) && cleanPath.StartsWith("/Maps/"))
            {
                return false;
            }

            return orig(path, metadata);
        }

    }
}
