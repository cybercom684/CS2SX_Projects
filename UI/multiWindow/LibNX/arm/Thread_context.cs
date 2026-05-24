// Auto-generated from arm\thread_context.h
// DO NOT EDIT MANUALLY

#pragma warning disable CS0649, CS0169, CS8981

namespace LibNX.Arm;

public enum RegisterGroup
{
    RegisterGroup_CpuGprs = (1 << 0),
    RegisterGroup_CpuSprs = (1 << 1),
    RegisterGroup_FpuGprs = (1 << 2),
    RegisterGroup_FpuSprs = (1 << 3),
    RegisterGroup_CpuAll = RegisterGroup_CpuGprs | RegisterGroup_CpuSprs,
    RegisterGroup_FpuAll = RegisterGroup_FpuGprs | RegisterGroup_FpuSprs,
    RegisterGroup_All = RegisterGroup_CpuAll  | RegisterGroup_FpuAll,
}

public enum ThreadExceptionDesc
{
    ThreadExceptionDesc_InstructionAbort = 0x100,
    ThreadExceptionDesc_MisalignedPC = 0x102,
    ThreadExceptionDesc_MisalignedSP = 0x103,
    ThreadExceptionDesc_SError = 0x106,
    ThreadExceptionDesc_BadSVC = 0x301,
    ThreadExceptionDesc_Trap = 0x104,
    ThreadExceptionDesc_Other = 0x101,
}

public unsafe struct ThreadContext
{
    // skipped array: CpuRegister cpu_gprs[29]
    public u64 fp;
    public u64 lr;
    public u64 sp;
    public CpuRegister pc;
    public u32 psr;
    // skipped array: FpuRegister fpu_gprs[32]
    public u32 fpcr;
    public u32 fpsr;
    public u64 tpidr;
}

public unsafe struct ThreadExceptionDump
{
    public u32 error_desc;
    public fixed u32 pad[3];
    // skipped array: CpuRegister cpu_gprs[29]
    public CpuRegister fp;
    public CpuRegister lr;
    public CpuRegister sp;
    public CpuRegister pc;
    public u64 padding;
    // skipped array: FpuRegister fpu_gprs[32]
    public u32 pstate;
    public u32 afsr0;
    public u32 afsr1;
    public u32 esr;
    public CpuRegister far;
}

public unsafe struct ThreadExceptionFrameA64
{
    public fixed u64 cpu_gprs[9];
    public u64 lr;
    public u64 sp;
    public u64 elr_el1;
    public u32 pstate;
    public u32 afsr0;
    public u32 afsr1;
    public u32 esr;
    public u64 far;
}

public unsafe struct ThreadExceptionFrameA32
{
    public fixed u32 cpu_gprs[8];
    public u32 sp;
    public u32 lr;
    public u32 elr_el1;
    public u32 tpidr_el0;
    public u32 cpsr;
    public u32 afsr0;
    public u32 afsr1;
    public u32 esr;
    public u32 far;
}

