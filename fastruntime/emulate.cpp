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
    case truejump:
        ushort addr = PopAddr();
        if (Pop() != 0) {
            pc = addr;
            return;
        }
        break;
    case jump:
        pc = PopAddr();
        return;
    case call:
        PushCallstack(pc);
        pc = PopAddr();
        return;
    case ret:
        if (callstack_count == 0) {
            isEnded = true;
            onEnd();
        }
        else {
            pc = PopCallstack();
        }
        break;
    case load:
        Push(Load(PopAddr()));
        break;
    case store:
        Store(PopAddr(), Pop());
        break;
    case dumpkey:
        Push(getkeyState());
    case redraw:
        onRedraw(displayStack.data(), displayStack.size());
        displayStack.clear();
        break;
    case rect:
        displayStack.push_back(Pect(Pop(), Pop(), Pop(), Pop(), Pop()));
        break;

    case io:
        uchar* data = ram + 0x5000;
        switch (Pop()) {
        case 2: //Load
            data = onLoad();
            for (int i = 0; i < 0x1000; i++)
                ram[0x5000+i] = data[i];
            break;
        case 3:
            onSave(data);
            break;
        }
    }
}
