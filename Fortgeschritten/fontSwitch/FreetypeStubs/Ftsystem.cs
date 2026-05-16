// Auto-generated from externLibs/freetype/include\freetype\ftsystem.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_StreamRec
{
    public IntPtr @base;
    public ulong size;
    public ulong pos;
    public FT_StreamDesc descriptor;
    public FT_StreamDesc pathname;
    public FT_Stream_IoFunc read;
    public FT_Stream_CloseFunc close;
    public FT_Memory memory;
    public IntPtr cursor;
    public IntPtr limit;
}

