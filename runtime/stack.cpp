#include "stack.h"
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
void clear() {
    stack_used = 0;
}

UINT callstack[CALLSTACK_LEN];
UINT callstack_used = 1;
void call(UINT addr) {
    if (callstack_used >= CALLSTACK_LEN - 1) {
        throw "callstack overflow";
    }
    callstack[callstack_used++] = addr;
}
UINT ret() {
    if (callstack_used == 0) {
        return UINT_MAX;
    }
    return callstack[--callstack_used];
}