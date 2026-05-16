// Auto-generated from externLibs/freetype/include\freetype\internal\tttypes.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum TT_SbitTableType
{
    TT_SBIT_TABLE_TYPE_NONE = 0,
    TT_SBIT_TABLE_TYPE_EBLC,
    TT_SBIT_TABLE_TYPE_CBLC,
    TT_SBIT_TABLE_TYPE_SBIX,
    TT_SBIT_TABLE_TYPE_MAX,
}

public unsafe struct TTC_HeaderRec
{
    public FT_ULong tag;
    public FT_Fixed version;
    public FT_Long count;
    public IntPtr offsets;
}

public unsafe struct TT_GaspRec
{
    public FT_UShort version;
    public FT_UShort numRanges;
    public TT_GaspRange gaspRanges;
}

public unsafe struct TT_FaceRec
{
    public FT_FaceRec root;
    public TTC_HeaderRec ttc_header;
    public FT_ULong format_tag;
    public FT_UShort num_tables;
    public TT_Table dir_tables;
    public TT_Header header;
    public TT_HoriHeader horizontal;
    public TT_MaxProfile max_profile;
    public FT_Bool vertical_info;
    public TT_VertHeader vertical;
    public FT_UShort num_names;
    public TT_NameTableRec name_table;
    public TT_OS2 os2;
    public TT_Postscript postscript;
    public IntPtr cmap_table;
    public FT_ULong cmap_size;
    public TT_Loader_GotoTableFunc goto_table;
    public TT_Loader_StartGlyphFunc access_glyph_frame;
    public TT_Loader_EndGlyphFunc forget_glyph_frame;
    public TT_Loader_ReadGlyphFunc read_glyph_header;
    public TT_Loader_ReadGlyphFunc read_simple_glyph;
    public TT_Loader_ReadGlyphFunc read_composite_glyph;
    public IntPtr sfnt;
    public IntPtr psnames;
    public IntPtr mm;
    public IntPtr tt_var;
    public IntPtr face_var;
    public IntPtr psaux;
    public TT_GaspRec gasp;
    public TT_PCLT pclt;
    public FT_ULong num_sbit_scales;
    public TT_SBit_Scale sbit_scales;
    public TT_Post_NamesRec postscript_names;
    public FT_Palette_Data palette_data;
    public FT_UShort palette_index;
    public IntPtr palette;
    public FT_Bool have_foreground_color;
    public FT_Color foreground_color;
    public FT_ULong font_program_size;
    public IntPtr font_program;
    public FT_ULong cvt_program_size;
    public IntPtr cvt_program;
    public FT_ULong cvt_size;
    public IntPtr cvt;
    public FT_Generic extra;
    public IntPtr postscript_name;
    public FT_ULong glyf_len;
    public FT_ULong glyf_offset;
    public FT_Bool is_cff2;
    public FT_Bool doblend;
    public GX_Blend blend;
    public FT_UInt32 variation_support;
    public IntPtr var_postscript_prefix;
    public FT_UInt var_postscript_prefix_len;
    public FT_UInt var_default_named_instance;
    public IntPtr non_var_style_name;
    public FT_ULong horz_metrics_size;
    public FT_ULong vert_metrics_size;
    public FT_ULong num_locations;
    public IntPtr glyph_locations;
    public IntPtr hdmx_table;
    public FT_ULong hdmx_table_size;
    public FT_UInt hdmx_record_count;
    public FT_ULong hdmx_record_size;
    public IntPtr hdmx_records;
    public IntPtr sbit_table;
    public FT_ULong sbit_table_size;
    public TT_SbitTableType sbit_table_type;
    public FT_UInt sbit_num_strikes;
    public IntPtr sbit_strike_map;
    public IntPtr kern_table;
    public FT_ULong kern_table_size;
    public FT_UInt num_kern_tables;
    public FT_UInt32 kern_avail_bits;
    public FT_UInt32 kern_order_bits;
    public TT_BDFRec bdf;
    public FT_ULong horz_metrics_offset;
    public FT_ULong vert_metrics_offset;
    public FT_ULong ebdt_start;
    public FT_ULong ebdt_size;
    public IntPtr cpal;
    public IntPtr colr;
    public IntPtr svg;
    public IntPtr gpos_table;
    public IntPtr gpos_lookups_kerning;
    public FT_UInt num_gpos_lookups_kerning;
}

public unsafe struct TT_LoaderRec
{
    public TT_Face face;
    public TT_Size size;
    public FT_GlyphSlot glyph;
    public FT_GlyphLoader gloader;
    public FT_ULong load_flags;
    public FT_UInt glyph_index;
    public FT_Stream stream;
    public FT_UInt byte_len;
    public FT_Short n_contours;
    public FT_BBox bbox;
    public FT_Int left_bearing;
    public FT_Int advance;
    public FT_Int linear;
    public FT_Bool linear_def;
    public FT_Vector pp1;
    public FT_Vector pp2;
    public TT_GlyphZoneRec @base;
    public TT_GlyphZoneRec zone;
    public TT_ExecContext exec;
    public FT_ULong ins_pos;
    public IntPtr other;
    public FT_Int top_bearing;
    public FT_Int vadvance;
    public FT_Vector pp3;
    public FT_Vector pp4;
    public IntPtr cursor;
    public IntPtr limit;
    public FT_ListRec composites;
    public IntPtr widthp;
}

