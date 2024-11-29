#include <Windows.h>
#include <fstream>
#include <DxLib.h>
#include "fastruntime.hpp"

int WINAPI WinMain(_In_ HINSTANCE hInstance, _In_opt_  HINSTANCE hPrevInstance, _In_ LPSTR lpCmdLine, _In_ int nShowCmd) {
	
	SetOutApplicationLogValidFlag(0);
	ChangeWindowMode(1);
	SetMainWindowText("fastgame");
	SetGraphMode(256, 256, 16);
	if (DxLib_Init() == -1) return -1;
	vector<char> data();
	std::fstream fs;
	fs.open("debug\\out.bin");
	if (!fs) return 0;
	char c;
	while ((c = fs.get()) != EOF)
		data.push_back(c);
	Runtime vm = Runtime(data.data(),data.size());
	
	WaitKey();
	DxLib_End();
	return 0;
}
