// Auto-generated from externLibs/freetype/include\freetype\ftcache.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FTC_ScalerRec
{
    public FTC_FaceID face_id;
    public FT_UInt width;
    public FT_UInt height;
    public FT_Int pixel;
    public FT_UInt x_res;
    public FT_UInt y_res;
}

public unsafe struct FTC_ImageTypeRec
{
    public FTC_FaceID face_id;
    public FT_UInt width;
    public FT_UInt height;
    public FT_Int32 flags;
}

public unsafe struct FTC_SBitRec
{
    public FT_Byte width;
    public FT_Byte height;
    public FT_Char left;
    public FT_Char top;
    public FT_Byte format;
    public FT_Byte max_grays;
    public FT_Short pitch;
    public FT_Char xadvance;
    public FT_Char yadvance;
    public IntPtr buffer;
}

