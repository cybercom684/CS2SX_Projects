// Auto-generated from externLibs/freetype/src\cff\cffparse.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct CFF_Field_Handler
{
    public int kind;
    public int code;
    public FT_UInt offset;
    public FT_Byte size;
    public CFF_Field_Reader reader;
    public FT_UInt array_max;
    public FT_UInt count_offset;
    public IntPtr id;
}

