// Auto-generated from externLibs/freetype/include\freetype\internal\psaux.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum T1_TokenType
{
    T1_TOKEN_TYPE_NONE = 0,
    T1_TOKEN_TYPE_ANY,
    T1_TOKEN_TYPE_STRING,
    T1_TOKEN_TYPE_ARRAY,
    T1_TOKEN_TYPE_KEY,
    T1_TOKEN_TYPE_MAX,
}

public enum T1_FieldType
{
    T1_FIELD_TYPE_NONE = 0,
    T1_FIELD_TYPE_BOOL,
    T1_FIELD_TYPE_INTEGER,
    T1_FIELD_TYPE_FIXED,
    T1_FIELD_TYPE_FIXED_1000,
    T1_FIELD_TYPE_STRING,
    T1_FIELD_TYPE_KEY,
    T1_FIELD_TYPE_BBOX,
    T1_FIELD_TYPE_MM_BBOX,
    T1_FIELD_TYPE_INTEGER_ARRAY,
    T1_FIELD_TYPE_FIXED_ARRAY,
    T1_FIELD_TYPE_CALLBACK,
    T1_FIELD_TYPE_MAX,
}

public enum T1_FieldLocation
{
    T1_FIELD_LOCATION_NONE = 0,
    T1_FIELD_LOCATION_CID_INFO,
    T1_FIELD_LOCATION_FONT_DICT,
    T1_FIELD_LOCATION_FONT_EXTRA,
    T1_FIELD_LOCATION_FONT_INFO,
    T1_FIELD_LOCATION_PRIVATE,
    T1_FIELD_LOCATION_BBOX,
    T1_FIELD_LOCATION_LOADER,
    T1_FIELD_LOCATION_FACE,
    T1_FIELD_LOCATION_BLEND,
    T1_FIELD_LOCATION_MAX,
}

public enum T1_ParseState
{
    T1_Parse_Start,
    T1_Parse_Have_Width,
    T1_Parse_Have_Moveto,
    T1_Parse_Have_Path,
}

public unsafe struct PS_Table_FuncsRec
{
    public IntPtr table,;
    public FT_Int count,;
    public FT_Memory memory );
    public IntPtr );
    public IntPtr table,;
    public FT_Int idx,;
    public IntPtr object,;
    public FT_UInt length );
    public IntPtr );
}

public unsafe struct PS_TableRec
{
    public IntPtr block;
    public FT_Offset cursor;
    public FT_Offset capacity;
    public FT_ULong init;
    public FT_Int max_elems;
    public IntPtr elements;
    public IntPtr lengths;
    public FT_Memory memory;
    public PS_Table_FuncsRec funcs;
}

public unsafe struct T1_TokenRec
{
    public IntPtr start;
    public IntPtr limit;
    public T1_TokenType type;
}

public unsafe struct T1_FieldRec
{
    public FT_UInt len;
    public IntPtr ident;
    public T1_FieldLocation location;
    public T1_FieldType type;
    public T1_Field_ParseFunc reader;
    public FT_UInt offset;
    public FT_Byte size;
    public FT_UInt array_max;
    public FT_UInt count_offset;
    public FT_UInt dict;
}

public unsafe struct PS_Parser_FuncsRec
{
    public IntPtr parser,;
    public IntPtr base,;
    public IntPtr limit,;
    public FT_Memory memory );
    public IntPtr );
    public IntPtr );
    public IntPtr );
    public IntPtr );
    public IntPtr parser,;
    public FT_Int power_ten );
    public IntPtr parser,;
    public IntPtr bytes,;
    public FT_Offset max_bytes,;
    public IntPtr pnum_bytes,;
    public FT_Bool delimiters );
    public IntPtr parser,;
    public FT_Int max_coords,;
    public IntPtr );
    public IntPtr parser,;
    public FT_Int max_values,;
    public IntPtr values,;
    public FT_Int power_ten );
    public IntPtr parser,;
    public T1_Token token );
    public IntPtr parser,;
    public T1_Token tokens,;
    public FT_UInt max_tokens,;
    public IntPtr );
    public IntPtr parser,;
    public T1_Field field,;
    public IntPtr objects,;
    public FT_UInt max_objects,;
    public IntPtr );
    public IntPtr parser,;
    public T1_Field field,;
    public IntPtr objects,;
    public FT_UInt max_objects,;
    public IntPtr );
}

public unsafe struct PS_ParserRec
{
    public IntPtr cursor;
    public IntPtr @base;
    public IntPtr limit;
    public FT_Error error;
    public FT_Memory memory;
    public PS_Parser_FuncsRec funcs;
}

public unsafe struct PS_Builder_FuncsRec
{
    public IntPtr ps_builder,;
    public IntPtr builder,;
    public FT_Bool is_t1 );
    public IntPtr );
}

public unsafe struct PS_Decoder_Zone
{
    public IntPtr @base;
    public IntPtr limit;
    public IntPtr cursor;
}

public unsafe struct PS_Decoder
{
    public PS_Builder builder;
    public FT_Fixed stack[PS_MAX_OPERANDS + _1];
    public IntPtr top;
    public PS_Decoder_Zone zones[PS_MAX_SUBRS_CALLS + _1];
    public IntPtr zone;
    public FT_Int flex_state;
    public FT_Int num_flex_vectors;
    // skipped array field: FT_Vector  flex_vectors[7]
    public CFF_Font cff;
    public CFF_SubFont current_subfont;
    public IntPtr cf2_instance;
    public IntPtr glyph_width;
    public FT_Bool width_only;
    public FT_Int num_hints;
    public FT_UInt num_locals;
    public FT_UInt num_globals;
    public FT_Int locals_bias;
    public FT_Int globals_bias;
    public IntPtr locals;
    public IntPtr globals;
    public IntPtr glyph_names;
    public FT_UInt num_glyphs;
    public FT_Render_Mode hint_mode;
    public FT_Bool seac;
    public CFF_Decoder_Get_Glyph_Callback get_glyph_callback;
    public CFF_Decoder_Free_Glyph_Callback free_glyph_callback;
    public FT_Service_PsCMaps psnames;
    public FT_Int lenIV;
    public IntPtr locals_len;
    public FT_Hash locals_hash;
    public FT_Matrix font_matrix;
    public FT_Vector font_offset;
    public PS_Blend blend;
    public IntPtr buildchar;
    public FT_UInt len_buildchar;
}

public unsafe struct T1_Builder_FuncsRec
{
    public IntPtr builder,;
    public FT_Face face,;
    public FT_Size size,;
    public FT_GlyphSlot slot,;
    public FT_Bool hinting );
    public IntPtr );
    public T1_Builder_Check_Points_Func check_points;
    public T1_Builder_Add_Point_Func add_point;
    public T1_Builder_Add_Point1_Func add_point1;
    public T1_Builder_Add_Contour_Func add_contour;
    public T1_Builder_Start_Point_Func start_point;
    public T1_Builder_Close_Contour_Func close_contour;
}

public unsafe struct T1_BuilderRec
{
    public FT_Memory memory;
    public FT_Face face;
    public FT_GlyphSlot glyph;
    public FT_GlyphLoader loader;
    public IntPtr @base;
    public IntPtr current;
    public FT_Pos pos_x;
    public FT_Pos pos_y;
    public FT_Vector left_bearing;
    public FT_Vector advance;
    public FT_BBox bbox;
    public T1_ParseState parse_state;
    public FT_Bool load_points;
    public FT_Bool no_recurse;
    public FT_Bool metrics_only;
    public IntPtr hints_funcs;
    public IntPtr hints_globals;
    public T1_Builder_FuncsRec funcs;
}

public unsafe struct T1_Decoder_FuncsRec
{
    public IntPtr decoder,;
    public FT_Face face,;
    public FT_Size size,;
    public FT_GlyphSlot slot,;
    public IntPtr glyph_names,;
    public PS_Blend blend,;
    public FT_Bool hinting,;
    public FT_Render_Mode hint_mode,;
    public T1_Decoder_Callback callback );
    public IntPtr );
    public IntPtr decoder,;
    public IntPtr base,;
    public FT_UInt len );
    public IntPtr decoder,;
    public IntPtr base,;
    public FT_UInt len );
    public IntPtr decoder,;
    public IntPtr charstring_base,;
    public FT_ULong charstring_len );
}

public unsafe struct T1_DecoderRec
{
    public T1_BuilderRec builder;
    public FT_Long stack;
    public IntPtr top;
    public T1_Decoder_ZoneRec zones[T1_MAX_SUBRS_CALLS + _1];
    public T1_Decoder_Zone zone;
    public FT_Service_PsCMaps psnames;
    public FT_UInt num_glyphs;
    public IntPtr glyph_names;
    public FT_Int lenIV;
    public FT_Int num_subrs;
    public IntPtr subrs;
    public IntPtr subrs_len;
    public FT_Hash subrs_hash;
    public FT_Matrix font_matrix;
    public FT_Vector font_offset;
    public FT_Int flex_state;
    public FT_Int num_flex_vectors;
    // skipped array field: FT_Vector            flex_vectors[7]
    public PS_Blend blend;
    public FT_Render_Mode hint_mode;
    public T1_Decoder_Callback parse_callback;
    public T1_Decoder_FuncsRec funcs;
    public IntPtr buildchar;
    public FT_UInt len_buildchar;
    public FT_Bool seac;
    public FT_Generic cf2_instance;
}

public unsafe struct CFF_Builder_FuncsRec
{
    public IntPtr builder,;
    public TT_Face face,;
    public CFF_Size size,;
    public CFF_GlyphSlot glyph,;
    public FT_Bool hinting );
    public IntPtr );
    public CFF_Builder_Check_Points_Func check_points;
    public CFF_Builder_Add_Point_Func add_point;
    public CFF_Builder_Add_Point1_Func add_point1;
    public CFF_Builder_Add_Contour_Func add_contour;
    public CFF_Builder_Start_Point_Func start_point;
    public CFF_Builder_Close_Contour_Func close_contour;
}

public unsafe struct CFF_Decoder_Zone
{
    public IntPtr @base;
    public IntPtr limit;
    public IntPtr cursor;
}

public unsafe struct CFF_Decoder
{
    public CFF_Builder builder;
    public CFF_Font cff;
    public FT_Fixed stack[CFF_MAX_OPERANDS + _1];
    public IntPtr top;
    public CFF_Decoder_Zone zones[CFF_MAX_SUBRS_CALLS + _1];
    public IntPtr zone;
    public FT_Int flex_state;
    public FT_Int num_flex_vectors;
    // skipped array field: FT_Vector  flex_vectors[7]
    public FT_Pos glyph_width;
    public FT_Pos nominal_width;
    public FT_Bool read_width;
    public FT_Bool width_only;
    public FT_Int num_hints;
    public FT_Fixed buildchar;
    public FT_UInt num_locals;
    public FT_UInt num_globals;
    public FT_Int locals_bias;
    public FT_Int globals_bias;
    public IntPtr locals;
    public IntPtr globals;
    public IntPtr glyph_names;
    public FT_UInt num_glyphs;
    public FT_Render_Mode hint_mode;
    public FT_Bool seac;
    public CFF_SubFont current_subfont;
    public CFF_Decoder_Get_Glyph_Callback get_glyph_callback;
    public CFF_Decoder_Free_Glyph_Callback free_glyph_callback;
}

public unsafe struct CFF_Decoder_FuncsRec
{
    public IntPtr decoder,;
    public TT_Face face,;
    public CFF_Size size,;
    public CFF_GlyphSlot slot,;
    public FT_Bool hinting,;
    public FT_Render_Mode hint_mode,;
    public CFF_Decoder_Get_Glyph_Callback get_callback,;
    public CFF_Decoder_Free_Glyph_Callback free_callback );
    public IntPtr decoder,;
    public CFF_Size size,;
    public FT_UInt glyph_index );
    public IntPtr decoder,;
    public IntPtr charstring_base,;
    public FT_ULong charstring_len,;
    public FT_Bool in_dict );
    public IntPtr decoder,;
    public IntPtr charstring_base,;
    public FT_ULong charstring_len );
}

public unsafe struct AFM_Parser_FuncsRec
{
    public IntPtr parser,;
    public FT_Memory memory,;
    public IntPtr base,;
    public IntPtr );
    public IntPtr );
    public IntPtr );
}

public unsafe struct AFM_ParserRec
{
    public FT_Memory memory;
    public AFM_Stream stream;
    public AFM_FontInfo FontInfo;
    public IntPtr name,;
    public FT_Offset len,;
    public IntPtr );
    public IntPtr user_data;
}

public unsafe struct T1_CMap_ClassesRec
{
    public FT_CMap_Class standard;
    public FT_CMap_Class expert;
    public FT_CMap_Class custom;
    public FT_CMap_Class unicode;
}

