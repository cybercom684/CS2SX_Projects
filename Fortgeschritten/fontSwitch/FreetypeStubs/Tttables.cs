// Auto-generated from externLibs/freetype/include\freetype\tttables.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'FreetypeStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace Freetype;

public enum FT_Sfnt_Tag
{
    FT_SFNT_HEAD,
    FT_SFNT_MAXP,
    FT_SFNT_OS2,
    FT_SFNT_HHEA,
    FT_SFNT_VHEA,
    FT_SFNT_POST,
    FT_SFNT_PCLT,
    FT_SFNT_MAX,
}

public unsafe struct TT_Header
{
    public FT_Fixed Table_Version;
    public FT_Fixed Font_Revision;
    public FT_Long CheckSum_Adjust;
    public FT_Long Magic_Number;
    public FT_UShort Flags;
    public FT_UShort Units_Per_EM;
    // skipped array field: FT_ULong   Created [2]
    // skipped array field: FT_ULong   Modified[2]
    public FT_Short xMin;
    public FT_Short yMin;
    public FT_Short xMax;
    public FT_Short yMax;
    public FT_UShort Mac_Style;
    public FT_UShort Lowest_Rec_PPEM;
    public FT_Short Font_Direction;
    public FT_Short Index_To_Loc_Format;
    public FT_Short Glyph_Data_Format;
}

public unsafe struct TT_HoriHeader
{
    public FT_Fixed Version;
    public FT_Short Ascender;
    public FT_Short Descender;
    public FT_Short Line_Gap;
    public FT_UShort advance_Width_Max;
    public FT_Short min_Left_Side_Bearing;
    public FT_Short min_Right_Side_Bearing;
    public FT_Short xMax_Extent;
    public FT_Short caret_Slope_Rise;
    public FT_Short caret_Slope_Run;
    public FT_Short caret_Offset;
    // skipped array field: FT_Short   Reserved[4]
    public FT_Short metric_Data_Format;
    public FT_UShort number_Of_HMetrics;
    public IntPtr long_metrics;
    public IntPtr short_metrics;
}

public unsafe struct TT_VertHeader
{
    public FT_Fixed Version;
    public FT_Short Ascender;
    public FT_Short Descender;
    public FT_Short Line_Gap;
    public FT_UShort advance_Height_Max;
    public FT_Short min_Top_Side_Bearing;
    public FT_Short min_Bottom_Side_Bearing;
    public FT_Short yMax_Extent;
    public FT_Short caret_Slope_Rise;
    public FT_Short caret_Slope_Run;
    public FT_Short caret_Offset;
    // skipped array field: FT_Short   Reserved[4]
    public FT_Short metric_Data_Format;
    public FT_UShort number_Of_VMetrics;
    public IntPtr long_metrics;
    public IntPtr short_metrics;
}

public unsafe struct TT_OS2
{
    public FT_UShort version;
    public FT_Short xAvgCharWidth;
    public FT_UShort usWeightClass;
    public FT_UShort usWidthClass;
    public FT_UShort fsType;
    public FT_Short ySubscriptXSize;
    public FT_Short ySubscriptYSize;
    public FT_Short ySubscriptXOffset;
    public FT_Short ySubscriptYOffset;
    public FT_Short ySuperscriptXSize;
    public FT_Short ySuperscriptYSize;
    public FT_Short ySuperscriptXOffset;
    public FT_Short ySuperscriptYOffset;
    public FT_Short yStrikeoutSize;
    public FT_Short yStrikeoutPosition;
    public FT_Short sFamilyClass;
    // skipped array field: FT_Byte    panose[10]
    public FT_ULong ulUnicodeRange1;
    public FT_ULong ulUnicodeRange2;
    public FT_ULong ulUnicodeRange3;
    public FT_ULong ulUnicodeRange4;
    // skipped array field: FT_Char    achVendID[4]
    public FT_UShort fsSelection;
    public FT_UShort usFirstCharIndex;
    public FT_UShort usLastCharIndex;
    public FT_Short sTypoAscender;
    public FT_Short sTypoDescender;
    public FT_Short sTypoLineGap;
    public FT_UShort usWinAscent;
    public FT_UShort usWinDescent;
    public FT_ULong ulCodePageRange1;
    public FT_ULong ulCodePageRange2;
    public FT_Short sxHeight;
    public FT_Short sCapHeight;
    public FT_UShort usDefaultChar;
    public FT_UShort usBreakChar;
    public FT_UShort usMaxContext;
    public FT_UShort usLowerOpticalPointSize;
    public FT_UShort usUpperOpticalPointSize;
}

public unsafe struct TT_Postscript
{
    public FT_Fixed FormatType;
    public FT_Fixed italicAngle;
    public FT_Short underlinePosition;
    public FT_Short underlineThickness;
    public FT_ULong isFixedPitch;
    public FT_ULong minMemType42;
    public FT_ULong maxMemType42;
    public FT_ULong minMemType1;
    public FT_ULong maxMemType1;
}

public unsafe struct TT_PCLT
{
    public FT_Fixed Version;
    public FT_ULong FontNumber;
    public FT_UShort Pitch;
    public FT_UShort xHeight;
    public FT_UShort Style;
    public FT_UShort TypeFamily;
    public FT_UShort CapHeight;
    public FT_UShort SymbolSet;
    // skipped array field: FT_Char    TypeFace[16]
    // skipped array field: FT_Char    CharacterComplement[8]
    // skipped array field: FT_Char    FileName[6]
    public FT_Char StrokeWeight;
    public FT_Char WidthType;
    public FT_Byte SerifStyle;
    public FT_Byte Reserved;
}

public unsafe struct TT_MaxProfile
{
    public FT_Fixed version;
    public FT_UShort numGlyphs;
    public FT_UShort maxPoints;
    public FT_UShort maxContours;
    public FT_UShort maxCompositePoints;
    public FT_UShort maxCompositeContours;
    public FT_UShort maxZones;
    public FT_UShort maxTwilightPoints;
    public FT_UShort maxStorage;
    public FT_UShort maxFunctionDefs;
    public FT_UShort maxInstructionDefs;
    public FT_UShort maxStackElements;
    public FT_UShort maxSizeOfInstructions;
    public FT_UShort maxComponentElements;
    public FT_UShort maxComponentDepth;
}

