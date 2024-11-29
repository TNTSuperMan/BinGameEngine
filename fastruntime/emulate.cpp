#include "fastruntime.hpp"

enum Command
{
    nop, push, pop, cls,
    add, sub, mul, div, rem, nand, equal, greater,
    truejump, jump, call, ret,
    load, store,
    dumpkey,
    redraw, rect, graph,
    sound, stopsound,
    io
};

void Runtime::EmulateFrame() {
    while (Load(pc) == redraw && !isEnded)
        Emulate();
    if (!isEnded) Emulate();
}