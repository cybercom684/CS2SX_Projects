// Auto-generated from externLibs/freetype/src\autofit\afhints.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum AF_Dimension
{
    AF_DIMENSION_HORZ = 0,
    AF_DIMENSION_VERT = 1,
    AF_DIMENSION_MAX,
}

public enum AF_Direction
{
    AF_DIR_NONE = 4,
    AF_DIR_RIGHT = 1,
    AF_DIR_LEFT = -1,
    AF_DIR_UP = 2,
    AF_DIR_DOWN = -2,
}

public unsafe struct AF_PointRec
{
    public FT_UShort flags;
    public FT_Char in_dir;
    public FT_Char out_dir;
    public FT_Pos ox, oy;
    public FT_Short fx, fy;
    public FT_Pos x, y;
    public FT_Pos u, v;
    public AF_Point next;
    public AF_Point prev;
    // skipped array field: AF_Edge    before[2]
    // skipped array field: AF_Edge    after[2]
}

public unsafe struct AF_SegmentRec
{
    public FT_Byte flags;
    public FT_Char dir;
    public FT_Short pos;
    public FT_Short delta;
    public FT_Short min_coord;
    public FT_Short max_coord;
    public FT_Short height;
    public AF_Edge edge;
    public AF_Segment edge_next;
    public AF_Segment link;
    public AF_Segment serif;
    public FT_Pos score;
    public FT_Pos len;
    public AF_Point first;
    public AF_Point last;
}

public unsafe struct AF_EdgeRec
{
    public FT_Short fpos;
    public FT_Pos opos;
    public FT_Pos pos;
    public FT_Byte flags;
    public FT_Char dir;
    public FT_Fixed scale;
    public AF_Width blue_edge;
    public AF_Edge link;
    public AF_Edge serif;
    public FT_Int score;
    public AF_Segment first;
    public AF_Segment last;
}

