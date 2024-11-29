#pragma once
#include "graphic.hpp"

Color c2rgb(uchar c) {
	Color color;
	color.R = ((c & 0b00110000) >> 4) * 85;
	color.G = ((c & 0b00001100) >> 2) * 85;
	color.B = ((c & 0b00000011) >> 0) * 85;
	color.isTransparent = (c & 0b11000000) >> 6 == 0b01;
	return color;
}

Pect::Pect(uchar c, uchar h, uchar w, uchar y, uchar x) {
	X = x;
	Y = x;
	Width = x;
	Height = x;
	isPixel = false;
	Color color = c2rgb(c);
	R = color.R;
	G = color.G;
	B = color.B;
	isDraw = !color.isTransparent;
}

Pect::Pect(uchar x, uchar y, uchar c) {
	X = x;
	Y = y;
	Width = 1;
	Height = 1;
	isPixel = true;
	Color color = c2rgb(c);
	R = color.R;
	G = color.G;
	B = color.B;
	isDraw = !color.isTransparent;
}
