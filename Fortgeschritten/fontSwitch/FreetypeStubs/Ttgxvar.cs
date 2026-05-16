// Auto-generated from externLibs/freetype/src\truetype\ttgxvar.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum GX_TupleCountFlags
{
    GX_TC_TUPLES_SHARE_POINT_NUMBERS = 0x8000,
    GX_TC_RESERVED_TUPLE_FLAGS = 0x7000,
    GX_TC_TUPLE_COUNT_MASK = 0x0FFF,
}

public enum GX_TupleIndexFlags
{
    GX_TI_EMBEDDED_TUPLE_COORD = 0x8000,
    GX_TI_INTERMEDIATE_TUPLE = 0x4000,
    GX_TI_PRIVATE_POINT_NUMBERS = 0x2000,
    GX_TI_RESERVED_TUPLE_FLAG = 0x1000,
    GX_TI_TUPLE_INDEX_MASK = 0x0FFF,
}

public unsafe struct GX_BlendRec
{
    public FT_UInt num_axis;
    public IntPtr coords;
    public IntPtr normalizedcoords;
    public IntPtr mmvar;
    public FT_Offset mmvar_len;
    public IntPtr normalized_stylecoords;
    public FT_Bool avar_loaded;
    public GX_AVarTable avar_table;
    public FT_Bool hvar_loaded;
    public FT_Bool hvar_checked;
    public FT_Error hvar_error;
    public GX_HVVarTable hvar_table;
    public FT_Bool vvar_loaded;
    public FT_Bool vvar_checked;
    public FT_Error vvar_error;
    public GX_HVVarTable vvar_table;
    public GX_MVarTable mvar_table;
    public FT_UInt tuplecount;
    public IntPtr tuplecoords;
    public IntPtr tuplescalars;
    public FT_UInt gv_glyphcnt;
    public IntPtr glyphoffsets;
    public FT_ULong gvar_size;
}

