#pragma once
#include "fastruntime.hpp"

struct Graphic {
	uchar X;
	uchar Y;
	uchar Width;
	uchar Height;
	uchar R;
	uchar G;
	uchar B;
	bool isPixel;
	bool isDraw;
	Graphic(uchar x, uchar y, uchar w, uchar h, uchar c);
	Graphic(uchar x, uchar y, uchar c);
};

struct Color {
	uchar R;
	uchar G;
	uchar B;
	bool isTransparent;
};

Color c2rgb(uchar c);
