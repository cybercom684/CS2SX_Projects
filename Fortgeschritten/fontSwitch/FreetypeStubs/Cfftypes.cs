// Auto-generated from externLibs/freetype/include\freetype\internal\cfftypes.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct CFF_VarData
{
    public FT_UInt itemCount;
    public FT_UInt shortDeltaCount;
    public FT_UInt regionIdxCount;
    public IntPtr regionIndices;
}

public unsafe struct CFF_AxisCoords
{
    public FT_Fixed startCoord;
    public FT_Fixed peakCoord;
    public FT_Fixed endCoord;
}

public unsafe struct CFF_VarRegion
{
    public IntPtr axisList;
}

public unsafe struct CFF_SubFontRec
{
    public CFF_FontRecDictRec font_dict;
    public CFF_PrivateRec private_dict;
    public CFF_BlendRec blend;
    public FT_UInt lenNDV;
    public IntPtr NDV;
    public IntPtr blend_stack;
    public IntPtr blend_top;
    public FT_UInt blend_used;
    public FT_UInt blend_alloc;
    public CFF_IndexRec local_subrs_index;
    public IntPtr local_subrs;
    public FT_UInt32 random;
}

public unsafe struct CFF_FontRec
{
    public FT_Library library;
    public FT_Stream stream;
    public FT_Memory memory;
    public FT_ULong base_offset;
    public FT_UInt num_faces;
    public FT_UInt num_glyphs;
    public FT_Byte version_major;
    public FT_Byte version_minor;
    public FT_Byte header_size;
    public FT_UInt top_dict_length;
    public FT_Bool cff2;
    public CFF_IndexRec name_index;
    public CFF_IndexRec top_dict_index;
    public CFF_IndexRec global_subrs_index;
    public CFF_EncodingRec encoding;
    public CFF_CharsetRec charset;
    public CFF_IndexRec charstrings_index;
    public CFF_IndexRec font_dict_index;
    public CFF_IndexRec private_index;
    public CFF_IndexRec local_subrs_index;
    public IntPtr font_name;
    public IntPtr global_subrs;
    public FT_UInt num_strings;
    public IntPtr strings;
    public IntPtr string_pool;
    public FT_ULong string_pool_size;
    public CFF_SubFontRec top_font;
    public FT_UInt num_subfonts;
    public CFF_SubFont subfonts;
    public CFF_FDSelectRec fd_select;
    public PSHinter_Service pshinter;
    public FT_Service_PsCMaps psnames;
    public IntPtr cffload;
    public IntPtr font_info;
    public IntPtr registry;
    public IntPtr ordering;
    public FT_Generic cf2_instance;
    public CFF_VStoreRec vstore;
    public IntPtr font_extra;
}

