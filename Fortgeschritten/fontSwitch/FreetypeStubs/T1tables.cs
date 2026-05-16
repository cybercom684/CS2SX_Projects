// Auto-generated from externLibs/freetype/include\freetype\t1tables.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum T1_Blend_Flags
{
    T1_BLEND_UNDERLINE_POSITION = 0,
    T1_BLEND_UNDERLINE_THICKNESS,
    T1_BLEND_ITALIC_ANGLE,
    T1_BLEND_BLUE_VALUES,
    T1_BLEND_OTHER_BLUES,
    T1_BLEND_STANDARD_WIDTH,
    T1_BLEND_STANDARD_HEIGHT,
    T1_BLEND_STEM_SNAP_WIDTHS,
    T1_BLEND_STEM_SNAP_HEIGHTS,
    T1_BLEND_BLUE_SCALE,
    T1_BLEND_BLUE_SHIFT,
    T1_BLEND_FAMILY_BLUES,
    T1_BLEND_FAMILY_OTHER_BLUES,
    T1_BLEND_FORCE_BOLD,
    T1_BLEND_MAX,
}

public enum T1_EncodingType
{
    T1_ENCODING_TYPE_NONE = 0,
    T1_ENCODING_TYPE_ARRAY,
    T1_ENCODING_TYPE_STANDARD,
    T1_ENCODING_TYPE_ISOLATIN1,
    T1_ENCODING_TYPE_EXPERT,
}

public enum PS_Dict_Keys
{
    PS_DICT_FONT_TYPE,
    PS_DICT_FONT_MATRIX,
    PS_DICT_FONT_BBOX,
    PS_DICT_PAINT_TYPE,
    PS_DICT_FONT_NAME,
    PS_DICT_UNIQUE_ID,
    PS_DICT_NUM_CHAR_STRINGS,
    PS_DICT_CHAR_STRING_KEY,
    PS_DICT_CHAR_STRING,
    PS_DICT_ENCODING_TYPE,
    PS_DICT_ENCODING_ENTRY,
    PS_DICT_NUM_SUBRS,
    PS_DICT_SUBR,
    PS_DICT_STD_HW,
    PS_DICT_STD_VW,
    PS_DICT_NUM_BLUE_VALUES,
    PS_DICT_BLUE_VALUE,
    PS_DICT_BLUE_FUZZ,
    PS_DICT_NUM_OTHER_BLUES,
    PS_DICT_OTHER_BLUE,
    PS_DICT_NUM_FAMILY_BLUES,
    PS_DICT_FAMILY_BLUE,
    PS_DICT_NUM_FAMILY_OTHER_BLUES,
    PS_DICT_FAMILY_OTHER_BLUE,
    PS_DICT_BLUE_SCALE,
    PS_DICT_BLUE_SHIFT,
    PS_DICT_NUM_STEM_SNAP_H,
    PS_DICT_STEM_SNAP_H,
    PS_DICT_NUM_STEM_SNAP_V,
    PS_DICT_STEM_SNAP_V,
    PS_DICT_FORCE_BOLD,
    PS_DICT_RND_STEM_UP,
    PS_DICT_MIN_FEATURE,
    PS_DICT_LEN_IV,
    PS_DICT_PASSWORD,
    PS_DICT_LANGUAGE_GROUP,
    PS_DICT_VERSION,
    PS_DICT_NOTICE,
    PS_DICT_FULL_NAME,
    PS_DICT_FAMILY_NAME,
    PS_DICT_WEIGHT,
    PS_DICT_IS_FIXED_PITCH,
    PS_DICT_UNDERLINE_POSITION,
    PS_DICT_UNDERLINE_THICKNESS,
    PS_DICT_FS_TYPE,
    PS_DICT_ITALIC_ANGLE,
    PS_DICT_MAX = PS_DICT_ITALIC_ANGLE,
}

public unsafe struct PS_FontInfoRec
{
    public IntPtr version;
    public IntPtr notice;
    public IntPtr full_name;
    public IntPtr family_name;
    public IntPtr weight;
    public FT_Fixed italic_angle;
    public FT_Bool is_fixed_pitch;
    public FT_Short underline_position;
    public FT_UShort underline_thickness;
}

public unsafe struct PS_PrivateRec
{
    public FT_Int unique_id;
    public FT_Int lenIV;
    public FT_Byte num_blue_values;
    public FT_Byte num_other_blues;
    public FT_Byte num_family_blues;
    public FT_Byte num_family_other_blues;
    // skipped array field: FT_Short   blue_values[14]
    // skipped array field: FT_Short   other_blues[10]
    // skipped array field: FT_Short   family_blues      [14]
    // skipped array field: FT_Short   family_other_blues[10]
    public FT_Fixed blue_scale;
    public FT_Int blue_shift;
    public FT_Int blue_fuzz;
    // skipped array field: FT_UShort  standard_width[1]
    // skipped array field: FT_UShort  standard_height[1]
    public FT_Byte num_snap_widths;
    public FT_Byte num_snap_heights;
    public FT_Bool force_bold;
    public FT_Bool round_stem_up;
    // skipped array field: FT_Short   snap_widths [13]
    // skipped array field: FT_Short   snap_heights[13]
    public FT_Fixed expansion_factor;
    public FT_Long language_group;
    public FT_Long password;
    // skipped array field: FT_Short   min_feature[2]
}

public unsafe struct CID_FaceDictRec
{
    public PS_PrivateRec private_dict;
    public FT_UInt len_buildchar;
    public FT_Fixed forcebold_threshold;
    public FT_Pos stroke_width;
    public FT_Fixed expansion_factor;
    public FT_Byte paint_type;
    public FT_Byte font_type;
    public FT_Matrix font_matrix;
    public FT_Vector font_offset;
    public FT_UInt num_subrs;
    public FT_ULong subrmap_offset;
    public FT_UInt sd_bytes;
}

public unsafe struct CID_FaceInfoRec
{
    public IntPtr cid_font_name;
    public FT_Fixed cid_version;
    public FT_Int cid_font_type;
    public IntPtr registry;
    public IntPtr ordering;
    public FT_Int supplement;
    public PS_FontInfoRec font_info;
    public FT_BBox font_bbox;
    public FT_ULong uid_base;
    public FT_Int num_xuid;
    // skipped array field: FT_ULong        xuid[16]
    public FT_ULong cidmap_offset;
    public FT_UInt fd_bytes;
    public FT_UInt gd_bytes;
    public FT_ULong cid_count;
    public FT_UInt num_dicts;
    public CID_FaceDict font_dicts;
    public FT_ULong data_offset;
}

