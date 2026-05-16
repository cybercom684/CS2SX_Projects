// Auto-generated from externLibs/freetype/src\truetype\ttinterp.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum TT_CodeRange_Tag
{
    tt_coderange_none = 0,
    tt_coderange_font,
    tt_coderange_cvt,
    tt_coderange_glyph,
}

public unsafe struct TT_CodeRange
{
    public IntPtr @base;
    public FT_Long size;
}

public unsafe struct TT_ExecContextRec
{
    public TT_Face face;
    public TT_Size size;
    public FT_Memory memory;
    public TT_Interpreter interpreter;
    public FT_Error error;
    public FT_Long top;
    public FT_Long stackSize;
    public IntPtr stack;
    public FT_Long args;
    public FT_Long new_top;
    public TT_GlyphZoneRec zp0,;
    public FT_Long pointSize;
    public FT_Size_Metrics metrics;
    public TT_Size_Metrics tt_metrics;
    public TT_GraphicsState GS;
    public FT_Int iniRange;
    public FT_Int curRange;
    public IntPtr code;
    public FT_Long IP;
    public FT_Long codeSize;
    public FT_Byte opcode;
    public FT_Int length;
    public FT_ULong cvtSize;
    public IntPtr cvt;
    public FT_ULong glyfCvtSize;
    public IntPtr glyfCvt;
    public FT_UInt glyphSize;
    public IntPtr glyphIns;
    public FT_UInt numFDefs;
    public FT_UInt maxFDefs;
    public TT_DefArray FDefs;
    public FT_UInt numIDefs;
    public FT_UInt maxIDefs;
    public TT_DefArray IDefs;
    public FT_UInt maxFunc;
    public FT_UInt maxIns;
    public FT_Int callTop,;
    public TT_CallStack callStack;
    public FT_UShort maxPoints;
    public FT_Short maxContours;
    public TT_CodeRangeTable codeRangeTable;
    public FT_UShort storeSize;
    public IntPtr storage;
    public FT_UShort glyfStoreSize;
    public IntPtr glyfStorage;
    public FT_F26Dot6 period;
    public FT_F26Dot6 phase;
    public FT_F26Dot6 threshold;
    public FT_Bool instruction_trap;
    public FT_Bool is_composite;
    public FT_Bool pedantic_hinting;
    public TT_Round_Func func_round;
    public FT_Vector moveVector;
    public TT_Project_Func func_project,;
    public TT_Move_Func func_move;
    public TT_Move_Func func_move_orig;
    public TT_Cur_Ppem_Func func_cur_ppem;
    public TT_Get_CVT_Func func_read_cvt;
    public TT_Set_CVT_Func func_write_cvt;
    public TT_Set_CVT_Func func_move_cvt;
    public FT_Bool grayscale;
    public FT_Int backward_compatibility;
    public FT_Render_Mode mode;
    public FT_ULong loopcall_counter;
    public FT_ULong loopcall_counter_max;
    public FT_ULong neg_jump_counter;
    public FT_ULong neg_jump_counter_max;
}

