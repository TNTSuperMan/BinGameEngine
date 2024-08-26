#include "Windows.h"
#include "DxLib.h"
#include <cmath>

#include "Initializer.h"
//#include <string>
#define STACK_LEN 256
#define CALLSTACK_LEN 128
#define MEMORY_LEN 0xFFFF

USHORT stack[STACK_LEN];
UINT stack_used = 0;
void push(USHORT value) {
    if (stack_used >= STACK_LEN - 1) {
        throw "stack overflow";
    }
    stack[stack_used++] = value;
}
USHORT pop() {
    if (stack_used == 0) {
        throw "stack underflow";
    }
    return stack[--stack_used];
}

UINT callstack[CALLSTACK_LEN];
UINT callstack_used = 0;
void call(UINT addr) {
    if (callstack_used >= CALLSTACK_LEN - 1) {
        throw "callstack overflow";
    }
    callstack[callstack_used++] = addr;
}
UINT ret() {
    if (callstack_used == 0) {
        throw "callstack underflow";
    }
    return callstack[--callstack_used];
}

UINT toaddr(short up, short down) {
    return (up << 16) | down;
}

int WINAPI wWinMain(_In_ HINSTANCE hInstance,_In_opt_ HINSTANCE hPrevInstance,_In_ LPWSTR lpCmdLine, _In_ int nShowCmd) {
    if (InitializeDx() == false) return -1;

    UINT pc = 0;
    char program[] = { 0,0,255,0,0,0,17,0,0,0,16,0,0,1,5,0,0,0,17,2,0,0,0,0,0,0,0,255,255,0,255,255,0,0,0,16,0,0,0,0,0,0,21,19,0,0,0,16,0,0,0,14,0,0,0,0,0,7,11 };
    for (int i = 0; i < STACK_LEN; i++) stack[i] = 0;
    for (int i = 0; i < CALLSTACK_LEN; i++) callstack[i] = 0;
    callstack[0] = UINT_MAX;
    USHORT memory[MEMORY_LEN];
    for (int i = 0; i < MEMORY_LEN; i++) memory[i] = 0;

    USHORT addrdown, addrup, ptr, x, y, w, h, r, g, b, pushdata, val2;
    while (pc < 59) {
        switch (program[pc]) {
        case 0x00: //push
            pushdata = program[++pc] << 8;
            pushdata += program[++pc];
            push(pushdata);
            break;
        case 0x01: //pop
            pop();
            break;
        case 0x02: //cls
            for (int i = 0; i < STACK_LEN; i++) stack[i] = 0;
            break;
        case 0x03: //pls
            push(pop() + pop());
            break;
        case 0x04: //sub
            val2 = pop();
            push(pop() - val2);
            break;
        case 0x05: //mul
            push(pop() * pop());
            break;
        case 0x06: //div
            val2 = pop();
            push(pop() / val2);
            break;
        case 0x07: //rem
            val2 = pop();
            push(pop() & val2);
            break;
        case 0x08: //nand
            push(~(pop() & pop()));
            break;
        case 0x09: //sin
            push(sin(3.141592653589793 / 180 * pop()));
            break;
        case 0x0a: //sqrt
            push(sqrt(pop()));
            break;
        case 0x0b: //truejump
            addrdown = pop();
            addrup = pop();
            if (pop()) pc = toaddr(addrup, addrdown) - 1;
            break;
        case 0x0c: //jump
            addrdown = pop();
            addrup = pop();
            pc = toaddr(addrup, addrdown) - 1;
            break;
        case 0x0d: //call
            addrdown = pop();
            addrup = pop();
            pc = toaddr(addrup, addrdown) - 1;
            call(toaddr(addrup, addrdown));
            break;
        case 0x0e: //equal
            push((pop() == pop()) ? (USHORT)1 : (USHORT)0);
            break;
        case 0x0f: //greater
            push(pop() < pop());
            //大なりな！>な！　popの順序上の最適化だぞ！
            break;
        case 0x10: //load
            push(memory[pop()]);
            break;
        case 0x11: //store
            ptr = pop();
            val2 = pop();
            memory[ptr] = val2;
            break;
        case 0x12: //ret
            ptr = ret();
            if (ptr == UINT_MAX) {
                return 0;
            }
            else {
                pc = ptr;
            }
            break;
        case 0x13: //redraw
            if (ProcessMessage() == -1) return 0;
            ScreenFlip();
            ClearDrawScreen();
            break;
        case 0x14: //pixel
            b = pop();
            g = pop();
            r = pop();
            y = pop();
            x = pop();
            DrawPixel(x, y, GetColor(r, g, b));
            break;
        case 0x15: //rect
            b = pop();
            g = pop();
            r = pop();
            h = pop();
            w = pop();
            y = pop();
            x = pop();
            DrawBox(x, y, x + w, y + h, GetColor(r, g, b), 1);
            break;
        case 0x16: //chkkey
            push(CheckHitKey(pop()));
            break;
        }
        pc++;
    }
    DxLib_End();
    return 0;
}