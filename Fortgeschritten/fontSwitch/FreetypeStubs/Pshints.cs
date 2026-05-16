// Auto-generated from externLibs/freetype/include\freetype\internal\pshints.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public unsafe struct T1_Hints_FuncsRec
{
    public T1_Hints hints;
    public T1_Hints_OpenFunc open;
    public T1_Hints_CloseFunc close;
    public T1_Hints_SetStemFunc stem;
    public T1_Hints_SetStem3Func stem3;
    public T1_Hints_ResetFunc reset;
    public T1_Hints_ApplyFunc apply;
}

public unsafe struct T2_Hints_FuncsRec
{
    public T2_Hints hints;
    public T2_Hints_OpenFunc open;
    public T2_Hints_CloseFunc close;
    public T2_Hints_StemsFunc stems;
    public T2_Hints_MaskFunc hintmask;
    public T2_Hints_CounterFunc counter;
    public T2_Hints_ApplyFunc apply;
}

public unsafe struct PSHinter_Interface
{
    public IntPtr );
    public IntPtr );
    public IntPtr );
}

