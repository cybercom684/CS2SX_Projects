// Auto-generated from externLibs/freetype/src\cache\ftcmru.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FTC_MruNodeRec
{
    public FTC_MruNode next;
    public FTC_MruNode prev;
}

public unsafe struct FTC_MruListClassRec
{
    public FT_Offset node_size;
    public FTC_MruNode_CompareFunc node_compare;
    public FTC_MruNode_InitFunc node_init;
    public FTC_MruNode_DoneFunc node_done;
}

public unsafe struct FTC_MruListRec
{
    public FT_UInt num_nodes;
    public FT_UInt max_nodes;
    public FTC_MruNode nodes;
    public FT_Pointer data;
    public FTC_MruListClassRec clazz;
    public FT_Memory memory;
}

