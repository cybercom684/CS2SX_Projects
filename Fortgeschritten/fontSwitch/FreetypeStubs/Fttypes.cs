// Auto-generated from externLibs/freetype/include\freetype\fttypes.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_UnitVector
{
    public FT_F2Dot14 x;
    public FT_F2Dot14 y;
}

public unsafe struct FT_Matrix
{
    public FT_Fixed xx, xy;
    public FT_Fixed yx, yy;
}

public unsafe struct FT_Data
{
    public IntPtr pointer;
    public FT_UInt length;
}

public unsafe struct FT_Generic
{
    public IntPtr data;
    public FT_Generic_Finalizer finalizer;
}

public unsafe struct FT_ListNodeRec
{
    public FT_ListNode prev;
    public FT_ListNode next;
    public IntPtr data;
}

public unsafe struct FT_ListRec
{
    public FT_ListNode head;
    public FT_ListNode tail;
}

