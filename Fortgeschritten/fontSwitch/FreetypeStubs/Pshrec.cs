// Auto-generated from externLibs/freetype/src\pshinter\pshrec.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum PS_Hint_Type
{
    PS_HINT_TYPE_1 = 1,
    PS_HINT_TYPE_2 = 2,
}

public unsafe struct PS_HintRec
{
    public FT_Int pos;
    public FT_Int len;
    public FT_UInt flags;
}

