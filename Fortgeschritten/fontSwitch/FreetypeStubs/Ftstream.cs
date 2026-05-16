// Auto-generated from externLibs/freetype/include\freetype\internal\ftstream.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_Frame_Op
{
    ft_frame_end = 0,
    ft_frame_start = FT_MAKE_FRAME_OP( FT_FRAME_OP_START, 0, 0 ),
    ft_frame_byte = FT_MAKE_FRAME_OP( FT_FRAME_OP_BYTE,  0, 0 ),
    ft_frame_schar = FT_MAKE_FRAME_OP( FT_FRAME_OP_BYTE,  0, 1 ),
    ft_frame_ushort_be = FT_MAKE_FRAME_OP( FT_FRAME_OP_SHORT, 0, 0 ),
    ft_frame_short_be = FT_MAKE_FRAME_OP( FT_FRAME_OP_SHORT, 0, 1 ),
    ft_frame_ushort_le = FT_MAKE_FRAME_OP( FT_FRAME_OP_SHORT, 1, 0 ),
    ft_frame_short_le = FT_MAKE_FRAME_OP( FT_FRAME_OP_SHORT, 1, 1 ),
    ft_frame_ulong_be = FT_MAKE_FRAME_OP( FT_FRAME_OP_LONG, 0, 0 ),
    ft_frame_long_be = FT_MAKE_FRAME_OP( FT_FRAME_OP_LONG, 0, 1 ),
    ft_frame_ulong_le = FT_MAKE_FRAME_OP( FT_FRAME_OP_LONG, 1, 0 ),
    ft_frame_long_le = FT_MAKE_FRAME_OP( FT_FRAME_OP_LONG, 1, 1 ),
    ft_frame_uoff3_be = FT_MAKE_FRAME_OP( FT_FRAME_OP_OFF3, 0, 0 ),
    ft_frame_off3_be = FT_MAKE_FRAME_OP( FT_FRAME_OP_OFF3, 0, 1 ),
    ft_frame_uoff3_le = FT_MAKE_FRAME_OP( FT_FRAME_OP_OFF3, 1, 0 ),
    ft_frame_off3_le = FT_MAKE_FRAME_OP( FT_FRAME_OP_OFF3, 1, 1 ),
    ft_frame_bytes = FT_MAKE_FRAME_OP( FT_FRAME_OP_BYTES, 0, 0 ),
    ft_frame_skip = FT_MAKE_FRAME_OP( FT_FRAME_OP_BYTES, 0, 1 ),
}

public unsafe struct FT_Frame_Field
{
    public FT_Byte value;
    public FT_Byte size;
    public FT_UShort offset;
}

