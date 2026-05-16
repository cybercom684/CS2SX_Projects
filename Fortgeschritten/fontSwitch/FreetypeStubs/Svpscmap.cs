// Auto-generated from externLibs/freetype/include\freetype\internal\services\svpscmap.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct PS_UniMap
{
    public FT_UInt32 unicode;
    public FT_UInt glyph_index;
}

public unsafe struct PS_UnicodesRec
{
    public FT_CMapRec cmap;
    public FT_UInt num_maps;
    public IntPtr maps;
}

