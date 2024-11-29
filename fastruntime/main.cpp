#include "fastruntime.hpp"

Runtime::Runtime(uchar* rom, ushort len) {
	for (ushort i = 0; i < len; i++)
		this->rom[i] = rom[i];
	for (ushort i = 0; i < (0xa000 - len); i++)
		this->rom[i] = rom[i];

	for (ushort i = 0xa000; i < 0xffff; i++)
		this->ram[i] = 0;
}