#include "fastruntime.hpp"

enum class Command: uchar
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
    Command c = (Command)Load(pc);
    while ((c != Command::redraw) && !isEnded) {
        Emulate();
        c = (Command)Load(pc);
    }
    if (!isEnded) Emulate();
}

void Runtime::Emulate() {
    uchar tmp;
    uchar* data;
    ushort addr;
    std::vector<uchar> graphstack;
    Command c = (Command)Load(pc);
    switch (c) {
    case Command::nop:
        break;
    case Command::push:
        Push(Load(++pc));
        break;
    case Command::pop:
        Pop();
        break;
    case Command::cls:
        stack_count = 0;
        break;
    case Command::add:
        Push(Pop() + Pop());
        break;
    case Command::sub:
        tmp = Pop();
        Push(Pop() + tmp);
        break;
    case Command::mul:
        Push(Pop() * Pop());
        break;
    case Command::div:
        tmp = Pop();
        Push(Pop() / tmp);
        break;
    case Command::rem:
        tmp = Pop();
        Push(Pop() % tmp);
        break;
    case Command::nand:
        Push(~(Pop() & Pop()));
        break;
    case Command::equal:
        Push(Pop() == Pop() ? 1 : 0);
        break;
    case Command::greater: //スタックの都合上逆です。
        Push(Pop() < Pop() ? 1 : 0);
        break;
    case Command::truejump:
        addr = PopAddr();
        if (Pop() != 0) {
            pc = addr;
            return;
        }
        break;
    case Command::jump:
        pc = PopAddr();
        return;
    case Command::call:
        PushCallstack(pc);
        pc = PopAddr();
        return;
    case Command::ret:
        if (callstack_count == 0) {
            isEnded = true;
            onEnd();
        }
        else {
            pc = PopCallstack();
        }
        break;
    case Command::load:
        Push(Load(PopAddr()));
        break;
    case Command::store:
        addr = PopAddr();
        Store(addr, Pop());
        break;
    case Command::dumpkey:
        Push(getkeyState());
    case Command::redraw:
        onRedraw(displayStack);
        displayStack.clear();
        break;
    case Command::rect:
        displayStack.push_back(Pect(Pop(), Pop(), Pop(), Pop(), Pop()));
        break;

    case Command::io:
        data = ram + 0x5000;
        switch (Pop()) {
        case 0:
            graphstack = std::vector<uchar>();
            graphics.clear();
            for (int i = 0; i < 0x1000; i++) {
                if ((data[i] & 0b11000000) << 6 == 0b11) {
                    graphics.push_back(Graphic(graphstack));
                    graphstack.clear();
                }else graphstack.push_back(data[i]);
            }
            break;
        case 2: //Load
            data = onLoad();
            for (int i = 0; i < 0x1000; i++)
                ram[0x5000+i] = data[i];
            break;
        case 3: //Save
            onSave(data);
            break;
        }
    }
    pc++;
}
