#pragma once
#include <functional>
#include "graphic.hpp"

typedef unsigned char uchar;
typedef unsigned short ushort;
class Runtime {
private:
	char stack[256];
	uchar stack_count;

	ushort callstack[256];
	uchar callstack_count;

	uchar ram[0x6000];
	uchar rom[0xa000];
public:
	Runtime(uchar* rom, ushort len);

	void Emulate();
	void EmulateFrame();

	uchar Load(ushort);
	void Store(ushort, uchar);

	std::function<void(Graphic[])> onRedraw;
	std::function<uchar()> getkeyState;
	std::function<void(uchar[])> onSave;
	std::function<uchar*()> onLoad;
	std::function<void(uchar hz, uchar len)> onSound;
	std::function<void()> onStopSound;
	std::function<void()> onEnd;
};
