
#include "targetver.h"

#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#include <fstream>

#include "DxLib.h"

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow){
    SetOutApplicationLogValidFlag(0);
    ChangeWindowMode(0);
    SetGraphMode(256, 256, 16);
    SetMainWindowText(L"fastgame");
    if(DxLib_Init() == -1) return -1;
    SetDrawScreen(DX_SCREEN_BACK);

    std::ifstream ifs;
    ifs.open("")
    return 0;
}
