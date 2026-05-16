// Auto-generated from externLibs/freetype/src\pshinter\pshalgo.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum PSH_Dir
{
    PSH_DIR_NONE = 0,
    PSH_DIR_UP = 1,
    PSH_DIR_DOWN = 2,
    PSH_DIR_VERTICAL = 1 | 2,
    PSH_DIR_LEFT = 4,
    PSH_DIR_RIGHT = 8,
    PSH_DIR_HORIZONTAL = 4 | 8,
}

public unsafe struct PSH_HintRec
{
    public FT_Int org_pos;
    public FT_Int org_len;
    public FT_Pos cur_pos;
    public FT_Pos cur_len;
    public FT_UInt flags;
    public PSH_Hint parent;
    public FT_Int order;
}

public unsafe struct PSH_PointRec
{
    public PSH_Point prev;
    public PSH_Point next;
    public PSH_Contour contour;
    public FT_UInt flags;
    public FT_UInt flags2;
    public PSH_Dir dir_in;
    public PSH_Dir dir_out;
    public PSH_Hint hint;
    public FT_Pos org_u;
    public FT_Pos org_v;
    public FT_Pos cur_u;
    public FT_Pos org_x;
    public FT_Pos cur_x;
    public FT_Pos org_y;
    public FT_Pos cur_y;
    public FT_UInt flags_x;
    public FT_UInt flags_y;
}

public unsafe struct PSH_ContourRec
{
    public PSH_Point start;
    public FT_UInt count;
}

public static class Freetype
{
    public static extern extern FT_Error ps_hints_apply(PS_Hints ps_hints, ref FT_Outline outline, PSH_Globals globals, FT_Render_Mode hint_mode);
}
