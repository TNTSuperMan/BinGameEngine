#include "Initializer.h"

bool InitializeDx() {
	ChangeWindowMode(1);
	SetGraphMode(255, 255, 32);
	SetWindowSizeExtendRate(2);
	SetOutApplicationLogValidFlag(0);
	SetMainWindowText("BGERuntime");
	if (DxLib_Init() == -1) return false;
	SetDrawScreen(DX_SCREEN_BACK);
	SetAlwaysRunFlag(1);
	return true;
}