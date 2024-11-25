﻿namespace runtime
{
    public partial class Runtime
    {
        delegate int Operate(byte b, byte a);
        enum Commands: byte
        {
            push,pop,cls,
            add,sub,mul,div,rem,nand,equal,greater,
            truejump,jump,call,ret,
            load,store,
            dumpkey,
            redraw,rect,graph,
            sound,stopsound,
            io
        }
        public void EmulateFrame()
        {
            while (memory[pc] != (byte)Commands.redraw)
                EmulateNext();
        }
        public void EmulateNext()
        {
            var StackOperate = (Operate op) =>
                stack.push((byte)(op(stack.pop(), stack.pop())));
            ushort addr;
            switch ((Commands)memory[pc])
            {
                case Commands.push:
                    stack.push(memory[++pc]);
                    break;
                case Commands.pop:
                    stack.pop();
                    break;
                case Commands.cls:
                    stack.clear();
                    break;
                case Commands.add:
                    StackOperate((b, a) => a + b);
                    break;
                case Commands.sub:
                    StackOperate((b, a) => a - b);
                    break;
                case Commands.mul:
                    StackOperate((b, a) => a * b);
                    break;
                case Commands.div:
                    StackOperate((b, a) => a / b);
                    break;
                case Commands.rem:
                    StackOperate((b, a) => a & b);
                    break;
                case Commands.nand:
                    StackOperate((b, a) => ~(a&b));
                    break;
                case Commands.greater:
                    StackOperate((b, a) => a>b ? 1:0);
                    break;
                case Commands.equal:
                    StackOperate((b, a) => a == b ? 1:0);
                    break;
                case Commands.truejump:
                    addr = stack.popAddr();
                    if (stack.pop() != 0)
                    {
                        pc = addr;
                    }
                    break;
                case Commands.jump:
                    pc = stack.popAddr();
                    break;
                case Commands.call:
                    callstack.Add(pc);
                    pc = stack.popAddr();
                    break;
                case Commands.ret:
                    if(callstack.Count == 0)
                    {
                        onEnd();
                    }
                    else
                    {
                        addr = callstack.Last();
                        callstack.RemoveAt(callstack.Count - 1);
                        pc = addr;
                    }
                    break;
                case Commands.load:
                    stack.push(memory[stack.popAddr()]);
                    break;
                case Commands.store:
                    memory[stack.popAddr()] = stack.pop();
                    break;
                case Commands.dumpkey:
                    stack.push(getKeyState());
                    break;
                case Commands.redraw:
                    onRedraw(graphicsStack.ToArray());
                    graphicsStack.Clear();
                    break;
                case Commands.rect:
                    break;
            }
        }
    }
}
