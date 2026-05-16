// Auto-generated from externLibs/freetype/src\cid\cidparse.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct CID_Parser
{
    public PS_ParserRec root;
    public FT_Stream stream;
    public IntPtr postscript;
    public FT_ULong postscript_len;
    public FT_ULong data_offset;
    public FT_ULong binary_length;
    public CID_FaceInfo cid;
    public FT_UInt num_dict;
}

