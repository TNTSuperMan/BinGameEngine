#include "fastruntime.hpp"

uchar Runtime::Load(ushort addr) {
	if (addr < 0xa000) {
		return this->rom[addr];
	}
	else {
		return this->ram[addr - 0xa000];
	}
}

void Runtime::Store(ushort addr, uchar value) {
	if (addr >= 0xa000) {
		this->ram[addr - 0xa000] = value;
	}
}
