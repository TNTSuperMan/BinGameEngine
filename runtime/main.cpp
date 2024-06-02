#include "Windows.h"
#include "DxLib.h"
#include <string>

#include "Initializer.h"

int WINAPI wWinMain(_In_ HINSTANCE hInstance,_In_opt_ HINSTANCE hPrevInstance,_In_ LPWSTR lpCmdLine, _In_ int nShowCmd) {
    if (InitializeDx() == false) return -1;

    short pc = 0;
    char* reg = new char[256];
    char* program = new char[13];
    char r1 = 0, r2 = 0;
    short tthen = 0, fthen = 0;
    program[0] = 0x0c; //=
    program[1] = 0x00; //reg
    program[2] = 0x61; //num
    program[3] = 0x0d; //INPUT
    program[4] = 0x01; //reg
    program[5] = 0x02; //IF
    program[6] = 0x00; //reg1
    program[7] = 0x01; //reg2
    program[8] = 0x00; //truejmp
    program[9] = 0x0b; //truejmp
    program[10] = 0x00;//falsejmp
    program[11] = 0x02;//falsejmp
    program[12] = 0x00;//NULL
    while (pc < 13 && ProcessMessage() != -1) {
        switch (program[pc]) {
        case 0x01:
            pc = program[++pc] * 0x0100 + program[++pc];
            pc--;
            break;
        case 0x02:
            r1 = reg[program[++pc]];
            r2 = reg[program[++pc]];
            tthen = program[++pc] * 0x0100;
            tthen += program[++pc];
            fthen = program[++pc] * 0x0100;
            fthen += program[++pc];
            if (r1 == r2) {
                pc = tthen;
            }
            else {
                pc = fthen;
            }
            pc--;
            break;
        case 0x0c:
            reg[program[++pc]] = program[++pc];
            break;
        case 0x0d:
            reg[program[++pc]] = GetInputChar(1);
            break;
        }
        pc++;
    }
    DxLib_End();
    return 0;
}