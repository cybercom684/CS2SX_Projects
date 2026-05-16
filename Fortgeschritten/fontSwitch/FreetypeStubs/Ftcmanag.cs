// Auto-generated from externLibs/freetype/src\cache\ftcmanag.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FTC_ManagerRec
{
    public FT_Library library;
    public FT_Memory memory;
    public FTC_Node nodes_list;
    public FT_Offset max_weight;
    public FT_Offset cur_weight;
    public FT_UInt num_nodes;
    public FTC_Cache caches;
    public FT_UInt num_caches;
    public FTC_MruListRec faces;
    public FTC_MruListRec sizes;
    public FT_Pointer request_data;
    public FTC_Face_Requester request_face;
}

