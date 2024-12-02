#pragma once
#include <functional>
#include <vector>

typedef unsigned char uchar;
typedef unsigned short ushort;

using namespace std;

// Pixel X Rect
struct Pect {
	uchar X;
	uchar Y;
	uchar Width;
	uchar Height;
	uchar R;
	uchar G;
	uchar B;
	bool isPixel;
	bool isDraw;
	Pect(uchar c, uchar h, uchar w, uchar y, uchar x);
	Pect(uchar x, uchar y, uchar c);
};

struct Graphic {
	vector<Pect> pixels;
	Graphic(vector<uchar>);
	vector<Pect> Draw(uchar x, uchar y);
};

struct Color {
	uchar R;
	uchar G;
	uchar B;
	bool isTransparent;
};

class Runtime {
private:
	uchar* stack;
	uchar stack_count = 0;

	ushort* callstack;
	uchar callstack_count = 0;

	vector<Pect> displayStack;
	vector<Graphic> graphics;

	uchar rom[0xa000];
	uchar ram[0x6000];

	ushort pc = 0;
	bool isEnded = false;

	void Push(uchar);
	uchar Pop();
	ushort PopAddr();

	void PushCallstack(ushort);
	ushort PopCallstack();
public:
	Runtime(uchar* rom, ushort len);
	Runtime();

	void Emulate();
	void EmulateFrame();

	uchar Load(ushort);
	void Store(ushort, uchar);

	function<void(vector<Pect>)> onRedraw;
	function<uchar()> getkeyState;
	function<void(uchar[])> onSave;
	function<uchar*()> onLoad;
	function<void(uchar hz, uchar len)> onSound;
	function<void()> onStopSound;
	function<void()> onEnd;
};

Color c2rgb(uchar);
