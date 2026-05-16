/*
 * freetype_build.c — Single-file build für Freetype auf Nintendo Switch
 * Lege in: externLibs/freetype/freetype_build.c
 *
 * FT2_BUILD_LIBRARY kommt via -D Flag aus cs2sx.json — nicht hier definieren.
 * FT_CONFIG_OPTION_DISABLE_STREAM_SUPPORT nicht gesetzt.
 * Alle Module die ftinit.c referenziert müssen compiliert sein.
 */

/* Basis */
#include "src/base/ftsystem.c"
#include "src/base/ftinit.c"
#include "src/base/ftdebug.c"
#include "src/base/ftbase.c"
#include "src/base/ftbbox.c"
#include "src/base/ftbdf.c"
#include "src/base/ftbitmap.c"
#include "src/base/ftcid.c"
#include "src/base/ftfstype.c"
#include "src/base/ftgasp.c"
#include "src/base/ftglyph.c"
#include "src/base/ftgxval.c"
#include "src/base/ftmm.c"
#include "src/base/ftotval.c"
#include "src/base/ftpfr.c"
#include "src/base/ftstroke.c"
#include "src/base/ftsynth.c"
#include "src/base/fttype1.c"
#include "src/base/ftwinfnt.c"

/* Font-Formate */
#include "src/truetype/truetype.c"
#include "src/cff/cff.c"
#include "src/cid/type1cid.c"
#include "src/pfr/pfr.c"
#include "src/sfnt/sfnt.c"
#include "src/type1/type1.c"
#include "src/type42/type42.c"
#include "src/winfonts/winfnt.c"
#include "src/bdf/bdf.c"
#include "src/pcf/pcf.c"

/* Renderer */
#include "src/smooth/smooth.c"
#include "src/raster/raster.c"
#include "src/sdf/sdf.c"

/* SVG — Stub damit ft_svg_renderer_class definiert ist */
#include "src/svg/ftsvg.c"

/* Hinting */
#include "src/autofit/autofit.c"
#include "src/psaux/psaux.c"
#include "src/pshinter/pshinter.c"
#include "src/psnames/psnames.c"

/* Cache & Kompression */
#include "src/cache/ftcache.c"
#include "src/gzip/ftgzip.c"
#include "src/lzw/ftlzw.c"