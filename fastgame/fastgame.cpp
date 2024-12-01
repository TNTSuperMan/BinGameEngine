
#include "targetver.h"

#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#include <fstream>
#include <vector>

#include "DxLib.h"
#include "fastruntime.hpp"

using namespace std;

typedef unsigned char uchar;

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow){
    SetOutApplicationLogValidFlag(0);
    ChangeWindowMode(1);
    SetGraphMode(256, 256, 16);
    SetMainWindowText(L"fastgame");
    if(DxLib_Init() == -1) return -1;
    SetDrawScreen(DX_SCREEN_BACK);

    std::ifstream ifs;
    ifs.open(".\\out.bin");
    if(!ifs) return -2;
    vector<uchar> data;
    char c;
    while((c = ifs.get()) != EOF)
        data.push_back(c);
    Runtime vm = Runtime(data.data(), data.size());
    bool isEnded = false;
    vector<Pect> disps;
    Pect cur = Pect(0,0,0);
    vm.onEnd = [&isEnded](){isEnded = true;};
    vm.onRedraw = [&disps](vector<Pect> d){
        disps.clear();
        for(int i = 0;i < d.size();i++)
            disps.push_back(d[i]);
    };
    while(ProcessMessage() == 0 && !isEnded){
        vm.EmulateFrame();
        ClearDrawScreen();
        for(int i = 0;i < disps.size();i++){
            cur = disps[i];
            if(cur.isDraw){
                if(cur.isPixel){
                    DrawPixel(cur.X, cur.Y, GetColor(cur.R, cur.G, cur.B));
                }else{
                    DrawBox(
                        cur.X, cur.Y,
                        cur.X + cur.Width,
                        cur.Y + cur.Height,
                        GetColor(cur.R, cur.G, cur.B), 1);
                }
            }
        }
        ScreenFlip();
    }
    DxLib_End();
    return 0;
}
