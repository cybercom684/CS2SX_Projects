// Auto-generated from externLibs/freetype/include\freetype\ftwinfnt.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_WinFNT_HeaderRec
{
    public FT_UShort version;
    public FT_ULong file_size;
    // skipped array field: FT_Byte    copyright[60]
    public FT_UShort file_type;
    public FT_UShort nominal_point_size;
    public FT_UShort vertical_resolution;
    public FT_UShort horizontal_resolution;
    public FT_UShort ascent;
    public FT_UShort internal_leading;
    public FT_UShort external_leading;
    public FT_Byte italic;
    public FT_Byte underline;
    public FT_Byte strike_out;
    public FT_UShort weight;
    public FT_Byte charset;
    public FT_UShort pixel_width;
    public FT_UShort pixel_height;
    public FT_Byte pitch_and_family;
    public FT_UShort avg_width;
    public FT_UShort max_width;
    public FT_Byte first_char;
    public FT_Byte last_char;
    public FT_Byte default_char;
    public FT_Byte break_char;
    public FT_UShort bytes_per_row;
    public FT_ULong device_offset;
    public FT_ULong face_name_offset;
    public FT_ULong bits_pointer;
    public FT_ULong bits_offset;
    public FT_Byte reserved;
    public FT_ULong flags;
    public FT_UShort A_space;
    public FT_UShort B_space;
    public FT_UShort C_space;
    public FT_UShort color_table_offset;
    // skipped array field: FT_ULong   reserved1[4]
}

