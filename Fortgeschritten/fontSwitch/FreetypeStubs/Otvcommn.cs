// Auto-generated from externLibs/freetype/src\otvalid\otvcommn.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct OTV_ValidatorRec
{
    public FT_Validator root;
    public FT_UInt type_count;
    public IntPtr type_funcs;
    public FT_UInt lookup_count;
    public FT_UInt glyph_count;
    public FT_UInt nesting_level;
    // skipped array field: OTV_Validate_Func   func[3]
    public FT_UInt extra1;
    public FT_UInt extra2;
    public FT_Bytes extra3;
    public FT_UInt debug_indent;
    public IntPtr debug_function_name;
}

