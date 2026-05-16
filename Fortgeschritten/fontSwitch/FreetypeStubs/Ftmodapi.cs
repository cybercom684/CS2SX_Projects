// Auto-generated from externLibs/freetype/include\freetype\ftmodapi.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_TrueTypeEngineType
{
    FT_TRUETYPE_ENGINE_TYPE_NONE = 0,
    FT_TRUETYPE_ENGINE_TYPE_UNPATENTED,
    FT_TRUETYPE_ENGINE_TYPE_PATENTED,
}

public unsafe struct FT_Module_Class
{
    public FT_ULong module_flags;
    public FT_Long module_size;
    public IntPtr module_name;
    public FT_Fixed module_version;
    public FT_Fixed module_requires;
    public IntPtr module_interface;
    public FT_Module_Constructor module_init;
    public FT_Module_Destructor module_done;
    public FT_Module_Requester get_interface;
}

