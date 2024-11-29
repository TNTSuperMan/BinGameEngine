#include "fastruntime.hpp"

enum Command: uchar
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

void Runtime::Emulate() {
    uchar tmp;
    switch (Load(pc)) {
    case nop:
        break;
    case push:
        Push(Load(++pc));
        break;
    case pop:
        Pop();
        break;
    case cls:
        stack_count = 0;
        break;
    case add:
        Push(Pop() + Pop());
        break;
    case sub:
        tmp = Pop();
        Push(Pop() + tmp);
        break;
    case mul:
        Push(Pop() * Pop());
        break;
    case div:
        tmp = Pop();
        Push(Pop() / tmp);
        break;
    case rem:
        tmp = Pop();
        Push(Pop() % tmp);
        break;
    case nand:
        Push(~(Pop() & Pop()));
        break;
    case equal:
        Push(Pop() == Pop() ? 1 : 0);
        break;
    case greater: //スタックの都合上逆です。
        Push(Pop() < Pop() ? 1 : 0);
        break;
    }
}
