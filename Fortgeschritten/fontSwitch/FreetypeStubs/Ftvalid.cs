// Auto-generated from externLibs/freetype/include\freetype\internal\ftvalid.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_ValidationLevel
{
    FT_VALIDATE_DEFAULT = 0,
    FT_VALIDATE_TIGHT,
    FT_VALIDATE_PARANOID,
}

public unsafe struct FT_ValidatorRec
{
    public ft_jmp_buf jump_buffer;
    public IntPtr @base;
    public IntPtr limit;
    public FT_ValidationLevel level;
    public FT_Error error;
}

