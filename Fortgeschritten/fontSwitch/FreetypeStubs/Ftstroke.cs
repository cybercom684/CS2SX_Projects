// Auto-generated from externLibs/freetype/include\freetype\ftstroke.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_Stroker_LineJoin
{
    FT_STROKER_LINEJOIN_ROUND = 0,
    FT_STROKER_LINEJOIN_BEVEL = 1,
    FT_STROKER_LINEJOIN_MITER_VARIABLE = 2,
    FT_STROKER_LINEJOIN_MITER = FT_STROKER_LINEJOIN_MITER_VARIABLE,
    FT_STROKER_LINEJOIN_MITER_FIXED = 3,
}

public enum FT_Stroker_LineCap
{
    FT_STROKER_LINECAP_BUTT = 0,
    FT_STROKER_LINECAP_ROUND,
    FT_STROKER_LINECAP_SQUARE,
}

public enum FT_StrokerBorder
{
    FT_STROKER_BORDER_LEFT = 0,
    FT_STROKER_BORDER_RIGHT,
}

