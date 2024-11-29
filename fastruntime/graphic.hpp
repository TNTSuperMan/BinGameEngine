#pragma once
#include "fastruntime.hpp"

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
	std::vector<Pect> pixels;
	Graphic(std::vector<uchar>);
	std::vector<Pect> Draw(uchar x, uchar y);
};

struct Color {
	uchar R;
	uchar G;
	uchar B;
	bool isTransparent;
};

Color c2rgb(uchar c);
