// Auto-generated from externLibs/freetype/include\freetype\internal\ftrfork.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_RFork_Rule
{
    FT_RFork_Rule_invalid = -2,
    FT_RFork_Rule_uknown,
    FT_RFork_Rule_apple_double,
    FT_RFork_Rule_apple_single,
    FT_RFork_Rule_darwin_ufs_export,
    FT_RFork_Rule_darwin_newvfs,
    FT_RFork_Rule_darwin_hfsplus,
    FT_RFork_Rule_vfat,
    FT_RFork_Rule_linux_cap,
    FT_RFork_Rule_linux_double,
    FT_RFork_Rule_linux_netatalk,
}

public unsafe struct FT_RFork_Ref
{
    public FT_Short res_id;
    public FT_Long offset;
}

public unsafe struct ft_raccess_guess_rec
{
    public ft_raccess_guess_func func;
    public FT_RFork_Rule type;
}

