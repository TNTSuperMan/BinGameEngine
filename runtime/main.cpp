#include "Windows.h"
#include "DxLib.h"
#include <fstream>
#include <vector>
#include <string>
#include <cmath>
#include "stack.h"

#include "Initializer.h"

UINT toaddr(short up, short down) {
    return (up << 16) | down;
}

const int Keymap[] = {
    KEY_INPUT_0,     //00
    KEY_INPUT_1,     //01
    KEY_INPUT_2,     //02
    KEY_INPUT_3,     //03
    KEY_INPUT_4,     //04
    KEY_INPUT_5,     //05
    KEY_INPUT_6,     //06
    KEY_INPUT_7,     //07
    KEY_INPUT_8,     //08
    KEY_INPUT_9,     //09
    KEY_INPUT_SPACE, //0a
    KEY_INPUT_A,     //0b
    KEY_INPUT_B,     //0c
    KEY_INPUT_C,     //0d
    KEY_INPUT_D,     //0e
    KEY_INPUT_E,     //0f
    KEY_INPUT_F,     //10
    KEY_INPUT_G,     //11
    KEY_INPUT_H,     //12
    KEY_INPUT_I,     //13
    KEY_INPUT_J,     //14
    KEY_INPUT_K,     //15
    KEY_INPUT_L,     //16
    KEY_INPUT_M,     //17
    KEY_INPUT_N,     //18
    KEY_INPUT_O,     //19
    KEY_INPUT_P,     //1a
    KEY_INPUT_Q,     //1b
    KEY_INPUT_R,     //1c
    KEY_INPUT_S,     //1d
    KEY_INPUT_T,     //1e
    KEY_INPUT_U,     //1f
    KEY_INPUT_V,     //20
    KEY_INPUT_W,     //21
    KEY_INPUT_X,     //22
    KEY_INPUT_Y,     //23
    KEY_INPUT_Z,     //24
    KEY_INPUT_LEFT,  //25
    KEY_INPUT_RIGHT, //26
    KEY_INPUT_UP,    //27
    KEY_INPUT_DOWN,  //28
    KEY_INPUT_RETURN,//29
    KEY_INPUT_BACK,  //2a

};

int key(USHORT k) {
    if (k < (USHORT)43) {
        return Keymap[k];
    }
    else {
        return 0;
    }
}
USHORT memory[MEMORY_LEN];

int WINAPI wWinMain(_In_ HINSTANCE hInstance,_In_opt_ HINSTANCE hPrevInstance,_In_ LPWSTR lpCmdLine, _In_ int nShowCmd) {
    if (!InitializeDx()) return -1;
    FILE* file;
    char* path = new char[512];
    size_t* len = new size_t[1];
    *len = 512;
    wcstombs_s(len, path, 512, lpCmdLine, 512);
    if (fopen_s(&file, path, "r")) {
        MessageBox(0, "Please select file", "BGE Error", 0);
        return -1;
    }
    std::vector<UCHAR> vd;
    while (!feof(file)) vd.push_back(fgetc(file));
    size_t size = vd.size();
    UCHAR* program = vd.data();
    UINT pc = 0;

    USHORT addrdown, addrup, ptr, x, y, w, h, r, g, b, pushdata, val2;
    while (pc < size) {
        switch (program[pc]) {
        case 0x00: //push
            if (pc + 3 > size) {
                MessageBox(0, "a", "BGE Error", 0);
                return -1;
            }
            pushdata = program[++pc] << 8;
            pushdata += program[++pc];
            push(pushdata);
            break;
        case 0x01: //pop
            pop();
            break;
        case 0x02: //cls
            clear();
            break;
        case 0x03: //pls
            push(pop() + pop());
            break;
        case 0x04: //sub
            val2 = pop();
            push(pop() - val2);
            break;
        case 0x05: //mul
            push(pop() * pop());
            break;
        case 0x06: //div
            val2 = pop();
            push(pop() / val2);
            break;
        case 0x07: //rem
            val2 = pop();
            push(pop() % val2);
            break;
        case 0x08: //nand
            push(~(pop() & pop()));
            break;
        case 0x09: //sin
            push(sin(3.141592653589793 / 180 * pop()));
            break;
        case 0x0a: //sqrt
            push(sqrt(pop()));
            break;
        case 0x0b: //truejump
            addrdown = pop();
            addrup = pop();
            if (pop()) pc = toaddr(addrup, addrdown) - 1;
            break;
        case 0x0c: //jump
            addrdown = pop();
            addrup = pop();
            pc = toaddr(addrup, addrdown) - 1;
            break;
        case 0x0d: //call
            call(pc);
            addrdown = pop();
            addrup = pop();
            pc = toaddr(addrup, addrdown) - 1;
            break;
        case 0x0e: //equal
            push((pop() == pop()) ? (USHORT)1 : (USHORT)0);
            break;
        case 0x0f: //greater
            push(pop() < pop());
            break;
        case 0x10: //load
            push(memory[pop()]);
            break;
        case 0x11: //store
            ptr = pop();
            val2 = pop();
            memory[ptr] = val2;
            break;
        case 0x12: //ret
            ptr = ret();
            if (ptr == UINT_MAX) {
                return 0;
            }
            else {
                pc = ptr;
            }
            break;
        case 0x13: //redraw
            ScreenFlip();
            if (ProcessMessage() == -1) return 0;
            ClearDrawScreen();
            break;
        case 0x14: //pixel
            b = pop();
            g = pop();
            r = pop();
            y = pop();
            x = pop();
            DrawPixel(x, y, GetColor(r, g, b));
            break;
        case 0x15: //rect
            b = pop();
            g = pop();
            r = pop();
            h = pop();
            w = pop();
            y = pop();
            x = pop();
            DrawBox(x, y, x + w, y + h, GetColor(r, g, b), 1);
            break;
        case 0x16: //chkkey
            push(CheckHitKey(key(pop())));
            break;
        }
        pc++;
    }
    DxLib_End();
    return 0;
}