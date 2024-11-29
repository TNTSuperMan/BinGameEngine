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
	Graphic(uchar c, uchar h, uchar w, uchar y, uchar x);
	Graphic(uchar x, uchar y, uchar c);
};

struct Color {
	uchar R;
	uchar G;
	uchar B;
	bool isTransparent;
};

Color c2rgb(uchar c);
