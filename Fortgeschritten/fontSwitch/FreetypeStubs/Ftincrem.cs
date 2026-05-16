// Auto-generated from externLibs/freetype/include\freetype\ftincrem.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_Incremental_MetricsRec
{
    public FT_Long bearing_x;
    public FT_Long bearing_y;
    public FT_Long advance;
    public FT_Long advance_v;
}

public unsafe struct FT_Incremental_FuncsRec
{
    public FT_Incremental_GetGlyphDataFunc get_glyph_data;
    public FT_Incremental_FreeGlyphDataFunc free_glyph_data;
    public FT_Incremental_GetGlyphMetricsFunc get_glyph_metrics;
}

public unsafe struct FT_Incremental_InterfaceRec
{
    public IntPtr funcs;
    public FT_Incremental @object;
}

