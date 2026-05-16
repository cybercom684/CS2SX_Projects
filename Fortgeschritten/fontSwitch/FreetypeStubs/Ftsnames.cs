// Auto-generated from externLibs/freetype/include\freetype\ftsnames.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_SfntName
{
    public FT_UShort platform_id;
    public FT_UShort encoding_id;
    public FT_UShort language_id;
    public FT_UShort name_id;
    public IntPtr @string;
    public FT_UInt string_len;
}

public unsafe struct FT_SfntLangTag
{
    public IntPtr @string;
    public FT_UInt string_len;
}

