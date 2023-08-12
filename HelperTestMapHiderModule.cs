using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Celeste.Mod.HelperTestMapHider; 

// ReSharper disable once UnusedType.Global
public class HelperTestMapHiderModule : EverestModule {

    public static List<string> helpers = new List<string> {"AltSidesHelper", "bitsbolts", "BounceHelper", "CustomPoints", "HonlyHelper", "JackalHelper", "SusanHelper"};
    public override void Load() {
        IL.Celeste.AreaData.Load += HookAreaDataLoad;
    }

    public override void Unload() {
        IL.Celeste.AreaData.Load -= HookAreaDataLoad;
    }

    private static bool IsFromHelpers(ModAsset asset) {
        return helpers.Contains(asset?.Source?.Name);
    }

    private static void HookAreaDataLoad(ILContext il) {
        ILCursor cursor = new ILCursor(il);
        int i = 0;
        if (cursor.TryGotoNext(MoveType.Before, instr => instr.MatchLdloc(out i),
                instr => instr.MatchLdfld<ModAsset>("PathVirtual"))
            && cursor.TryGotoPrev(MoveType.After, instr => instr.MatchStloc(i)))
        {
            cursor.Emit(OpCodes.Ldloc, i);
            cursor.EmitDelegate(IsFromHelpers);
            ILLabel target = cursor.DefineLabel();
            cursor.Emit(OpCodes.Brtrue, target);
            cursor.GotoNext(MoveType.After ,instr => instr.MatchStfld<AreaData>("OnLevelBegin")).MarkLabel(target);
        }
    }

}