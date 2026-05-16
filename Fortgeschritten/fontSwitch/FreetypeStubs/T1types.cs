// Auto-generated from externLibs/freetype/include\freetype\internal\t1types.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct PS_FontExtraRec
{
    public FT_UShort fs_type;
}

public unsafe struct T1_FaceRec
{
    public FT_FaceRec root;
    public T1_FontRec type1;
    public IntPtr psnames;
    public IntPtr psaux;
    public IntPtr afm_data;
    // skipped array field: FT_CharMapRec  charmaprecs[2]
    // skipped array field: FT_CharMap     charmaps[2]
    public PS_Blend blend;
    public FT_Int ndv_idx;
    public FT_Int cdv_idx;
    public FT_UInt len_buildchar;
    public IntPtr buildchar;
    public IntPtr pshinter;
}

public unsafe struct CID_FaceRec
{
    public FT_FaceRec root;
    public IntPtr psnames;
    public IntPtr psaux;
    public CID_FaceInfoRec cid;
    public PS_FontExtraRec font_extra;
    public IntPtr afm_data;
    public CID_Subrs subrs;
    public IntPtr pshinter;
    public IntPtr binary_data;
    public FT_Stream cid_stream;
}

