#pragma once
#include <functional>
#include <vector>
#include "graphic.hpp"

typedef unsigned char uchar;
typedef unsigned short ushort;

class Runtime {
private:
	uchar stack[256];
	uchar stack_count = 0;

	ushort callstack[256];
	uchar callstack_count = 0;

	std::vector<Pect> displayStack = std::vector<Pect>();
	std::vector<Graphic> graphics = std::vector<Graphic>();

	uchar ram[0x6000];
	uchar rom[0xa000];

	ushort pc = 0;
	bool isEnded = false;

	void Push(uchar);
	uchar Pop();
	ushort PopAddr();

	void PushCallstack(ushort);
	ushort PopCallstack();
public:
	Runtime(uchar* rom, ushort len);

	void Emulate();
	void EmulateFrame();

	uchar Load(ushort);
	void Store(ushort, uchar);

	std::function<void(std::vector<Pect>)> onRedraw;
	std::function<uchar()> getkeyState;
	std::function<void(uchar[])> onSave;
	std::function<uchar*()> onLoad;
	std::function<void(uchar hz, uchar len)> onSound;
	std::function<void()> onStopSound;
	std::function<void()> onEnd;
};
