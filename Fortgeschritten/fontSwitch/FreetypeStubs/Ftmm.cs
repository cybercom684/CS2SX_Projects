// Auto-generated from externLibs/freetype/include\freetype\ftmm.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct FT_MM_Axis
{
    public IntPtr name;
    public FT_Long minimum;
    public FT_Long maximum;
}

public unsafe struct FT_Multi_Master
{
    public FT_UInt num_axis;
    public FT_UInt num_designs;
    public FT_MM_Axis axis;
}

public unsafe struct FT_Var_Axis
{
    public IntPtr name;
    public FT_Fixed minimum;
    public FT_Fixed def;
    public FT_Fixed maximum;
    public FT_ULong tag;
    public FT_UInt strid;
}

public unsafe struct FT_Var_Named_Style
{
    public IntPtr coords;
    public FT_UInt strid;
    public FT_UInt psid;
}

public unsafe struct FT_MM_Var
{
    public FT_UInt num_axis;
    public FT_UInt num_designs;
    public FT_UInt num_namedstyles;
    public IntPtr axis;
    public IntPtr namedstyle;
}

