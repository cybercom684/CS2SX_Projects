// Auto-generated from externLibs/freetype/src\cache\ftccache.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FTC_NodeRec
{
    public FTC_MruNodeRec mru;
    public FTC_Node link;
    public FT_Offset hash;
    public FT_UShort cache_index;
    public FT_Short ref_count;
}

public unsafe struct FTC_CacheClassRec
{
    public FTC_Node_NewFunc node_new;
    public FTC_Node_WeightFunc node_weight;
    public FTC_Node_CompareFunc node_compare;
    public FTC_Node_CompareFunc node_remove_faceid;
    public FTC_Node_FreeFunc node_free;
    public FT_Offset cache_size;
    public FTC_Cache_InitFunc cache_init;
    public FTC_Cache_DoneFunc cache_done;
}

public unsafe struct FTC_CacheRec
{
    public FT_UFast p;
    public FT_UFast mask;
    public FT_Long slack;
    public IntPtr buckets;
    public FTC_CacheClassRec clazz;
    public FTC_Manager manager;
    public FT_Memory memory;
    public FT_UInt index;
    public FTC_CacheClass org_class;
}

