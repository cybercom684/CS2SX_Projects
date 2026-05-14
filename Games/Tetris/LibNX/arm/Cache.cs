// Auto-generated from arm\cache.h
// DO NOT EDIT MANUALLY

#pragma warning disable CS0649, CS0169, CS8981

namespace LibNX.Arm;

public static class Cache
{
    public static extern void armDCacheFlush(IntPtr addr, ulong size);
    public static extern void armDCacheClean(IntPtr addr, ulong size);
    public static extern void armICacheInvalidate(IntPtr addr, ulong size);
    public static extern void armDCacheZero(IntPtr addr, ulong size);
}
