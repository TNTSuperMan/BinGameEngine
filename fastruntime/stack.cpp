#include "fastruntime.hpp"

void Runtime::push(uchar value) {
	stack[stack_count++] = value;
}

uchar Runtime::pop() {
	return stack[--stack_count];
}

void Runtime::Call(ushort addr) {
	if (callstack_count == 0xff) {
		throw "Callstack overflow";
	}
	else {
		callstack[callstack_count++] = addr;
	}
}

ushort Runtime::Ret() {
	if (callstack_count == 0) {
		throw "Callstack underflow";
	}
	else {
		return callstack[--callstack_count];
	}
}
