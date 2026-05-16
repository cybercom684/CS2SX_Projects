// Auto-generated from externLibs/freetype/src\gxvalid\gxvcommn.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum GXV_LookupValue_SignSpec
{
    GXV_LOOKUPVALUE_UNSIGNED = 0,
    GXV_LOOKUPVALUE_SIGNED,
}

public enum GXV_GlyphOffset_Format
{
    GXV_GLYPHOFFSET_NONE = -1,
    GXV_GLYPHOFFSET_UCHAR = 2,
    GXV_GLYPHOFFSET_CHAR,
    GXV_GLYPHOFFSET_USHORT = 4,
    GXV_GLYPHOFFSET_SHORT,
    GXV_GLYPHOFFSET_ULONG = 8,
    GXV_GLYPHOFFSET_LONG,
}

public unsafe struct GXV_ValidatorRec
{
    public FT_Validator root;
    public FT_Face face;
    public IntPtr table_data;
    public FT_ULong subtable_length;
    public GXV_LookupValue_SignSpec lookupval_sign;
    public GXV_Lookup_Value_Validate_Func lookupval_func;
    public GXV_Lookup_Fmt4_Transit_Func lookupfmt4_trans;
    public FT_Bytes lookuptbl_head;
    public FT_UShort min_gid;
    public FT_UShort max_gid;
    public GXV_StateTable_ValidatorRec statetable;
    public GXV_XStateTable_ValidatorRec xstatetable;
    public FT_UInt debug_indent;
    public IntPtr debug_function_name;
}

