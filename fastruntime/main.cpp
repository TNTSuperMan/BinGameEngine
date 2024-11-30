#include "fastruntime.hpp"

Runtime::Runtime(uchar* rom, ushort len) {
	stack = new uchar[256];
	callstack = new ushort[256];
	this->rom = new uchar[0xa000];
	ram = new uchar[0x6000];
	for (ushort i = 0; i < len; i++)
		this->rom[i] = rom[i];
	for (ushort i = 0; i < (0xa000 - len); i++)
		this->rom[i] = rom[i];

	for (ushort i = 0; i < 0x6000; i++)
		this->ram[i] = 0;
}

Runtime::Runtime() {
	stack = new uchar[256];
	callstack = new ushort[256];
	rom = new uchar[0xa000];
	ram = new uchar[0x6000];
}
