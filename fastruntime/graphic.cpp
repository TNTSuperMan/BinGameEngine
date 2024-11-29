#pragma once
#include "fastruntime.hpp"

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

Graphic::Graphic(std::vector<uchar> data) {
	pixels = std::vector<Pect>();
	uchar x = 0,y = 0;
	for (int i = 0; i < data.size(); i++) {
		uchar d = data.at(i);
		if ((d & 0b11000000) >> 6 != 0b01) {
			if ((d & 0b11000000) >> 6 != 0b10) {
				y++;
				x = 0;
			}
			else {
				pixels.push_back(Pect(x, y, d));
				x++;
			}
		}
	}
}

std::vector<Pect> Graphic::Draw(uchar x, uchar y) {
	std::vector<Pect> moved = std::vector<Pect>();
	for (int i = 0; i < pixels.size(); i++) {
		Pect p = pixels.at(i);
		if ((0xff - p.X) < x) continue;
		if ((0xff - p.Y) < y) continue;
		p.X += x;
		p.Y += y;
		moved.push_back(p);
	}
	return moved;
}
