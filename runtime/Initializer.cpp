#include "Initializer.h"

bool InitializeDx() {
	ChangeWindowMode(1);
	SetOutApplicationLogValidFlag(0);
	if (DxLib_Init() == -1) return false;
	SetMainWindowText("BGERuntime");
	SetAlwaysRunFlag(1);
	return true;
}