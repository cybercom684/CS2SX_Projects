// Auto-generated from externLibs/freetype/include\freetype\internal\fthash.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_HashnodeRec
{
    public FT_Hashkey key;
    public ulong data;
}

public unsafe struct FT_HashRec
{
    public FT_UInt limit;
    public FT_UInt size;
    public FT_UInt used;
    public FT_Hash_LookupFunc lookup;
    public FT_Hash_CompareFunc compare;
    public IntPtr table;
}

public static class Freetype
{
    public static extern FT_Error ft_hash_str_init(FT_Hash hash, FT_Memory memory);
    public static extern FT_Error ft_hash_num_init(FT_Hash hash, FT_Memory memory);
    public static extern void ft_hash_str_free(FT_Hash hash, FT_Memory memory);
    public static extern FT_Error ft_hash_str_insert(ref byte key, ulong data, FT_Hash hash, FT_Memory memory);
    public static extern FT_Error ft_hash_num_insert(FT_Int num, ulong data, FT_Hash hash, FT_Memory memory);
    public static extern FT_Error ft_hash_str_insert_no_overwrite(ref byte key, ulong data, FT_Hash hash, FT_Memory memory);
    public static extern FT_Error ft_hash_num_insert_no_overwrite(FT_Int num, ulong data, FT_Hash hash, FT_Memory memory);
    public static extern IntPtr ft_hash_str_lookup(ref byte key, FT_Hash hash);
    public static extern IntPtr ft_hash_num_lookup(FT_Int num, FT_Hash hash);
    public static extern FT_Bool ft_hash_num_iterator(ref FT_UInt idx, ref FT_Int key, ref ulong value, FT_Hash hash);
    public static extern FT_Bool ft_hash_str_iterator(ref FT_UInt idx, ref byte key, ref ulong value, FT_Hash hash);
}
