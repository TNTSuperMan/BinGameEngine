#include "fastruntime.hpp"

Runtime::Runtime(uchar* rom, ushort len) {
	stack = new uchar[256];
	callstack = new ushort[256];
	for (ushort i = 0; i < len; i++)
		this->rom[i] = rom[i];
	for (ushort i = len; i < 0xa000; i++)
		this->rom[i] = 0;

	for (ushort i = 0; i < 0x6000; i++)
		this->ram[i] = 0;
}

Runtime::Runtime() {
	stack = new uchar[256];
	callstack = new ushort[256];
}
