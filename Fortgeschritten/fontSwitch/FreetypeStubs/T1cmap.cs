// Auto-generated from externLibs/freetype/src\psaux\t1cmap.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct T1_CMapStdRec
{
    public FT_CMapRec cmap;
    public IntPtr code_to_sid;
    public PS_Adobe_Std_StringsFunc sid_to_string;
    public FT_UInt num_glyphs;
    public IntPtr glyph_names;
}

public unsafe struct T1_CMapCustomRec
{
    public FT_CMapRec cmap;
    public FT_UInt first;
    public FT_UInt count;
    public IntPtr indices;
}

