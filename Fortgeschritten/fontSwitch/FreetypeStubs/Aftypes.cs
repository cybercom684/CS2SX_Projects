// Auto-generated from externLibs/freetype/src\autofit\aftypes.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum AF_WritingSystem
{
    AF_WRITING_SYSTEM_MAX,
}

public enum AF_Script
{
    AF_SCRIPT_MAX,
}

public enum AF_Coverage
{
    AF_COVERAGE_DEFAULT,
}

public enum AF_Style
{
    AF_STYLE_MAX,
}

public unsafe struct AF_WritingSystemClassRec
{
    public AF_WritingSystem writing_system;
    public FT_Offset style_metrics_size;
    public AF_WritingSystem_InitMetricsFunc style_metrics_init;
    public AF_WritingSystem_ScaleMetricsFunc style_metrics_scale;
    public AF_WritingSystem_DoneMetricsFunc style_metrics_done;
    public AF_WritingSystem_GetStdWidthsFunc style_metrics_getstdw;
    public AF_WritingSystem_InitHintsFunc style_hints_init;
    public AF_WritingSystem_ApplyHintsFunc style_hints_apply;
}

public unsafe struct AF_Script_UniRangeRec
{
    public FT_UInt32 first;
    public FT_UInt32 last;
}

public unsafe struct AF_ScriptClassRec
{
    public AF_Script script;
    public AF_Script_UniRange script_uni_ranges;
    public AF_Script_UniRange script_uni_nonbase_ranges;
    public FT_Bool top_to_bottom_hinting;
    public IntPtr standard_charstring;
}

public unsafe struct AF_StyleClassRec
{
    public AF_Style style;
    public AF_WritingSystem writing_system;
    public AF_Script script;
    public AF_Blue_Stringset blue_stringset;
    public AF_Coverage coverage;
}

public unsafe struct AF_StyleMetricsRec
{
    public AF_StyleClass style_class;
    public AF_ScalerRec scaler;
    public FT_Bool digits_have_same_width;
    public AF_FaceGlobals globals;
    public FT_Hash reverse_charmap;
}

