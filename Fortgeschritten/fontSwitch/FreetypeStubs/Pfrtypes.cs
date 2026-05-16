// Auto-generated from externLibs/freetype/src\pfr\pfrtypes.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct PFR_KernItemRec
{
    public PFR_KernItem next;
    public FT_Byte pair_count;
    public FT_Byte flags;
    public FT_Short base_adj;
    public FT_UInt pair_size;
    public FT_Offset offset;
    public FT_UInt32 pair1;
    public FT_UInt32 pair2;
}

