﻿namespace bgeruntime
{
    public partial class Runtime
    {
        delegate int Operate(byte b, byte a);
        enum Command: byte
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
            while (Memory[pc] != (byte)Command.redraw)
                EmulateNext();
        }
        public void EmulateNext()
        {
            var StackOperate = (Operate op) =>
                stack.push((byte)(op(stack.pop(), stack.pop())));
            ushort addr;
            switch ((Command)Memory[pc])
            {
                case Command.push:
                    stack.push(Memory[++pc]);
                    break;
                case Command.pop:
                    stack.pop();
                    break;
                case Command.cls:
                    stack.clear();
                    break;
                case Command.add:
                    StackOperate((b, a) => a + b);
                    break;
                case Command.sub:
                    StackOperate((b, a) => a - b);
                    break;
                case Command.mul:
                    StackOperate((b, a) => a * b);
                    break;
                case Command.div:
                    StackOperate((b, a) => a / b);
                    break;
                case Command.rem:
                    StackOperate((b, a) => a & b);
                    break;
                case Command.nand:
                    StackOperate((b, a) => ~(a&b));
                    break;
                case Command.greater:
                    StackOperate((b, a) => a>b ? 1:0);
                    break;
                case Command.equal:
                    StackOperate((b, a) => a == b ? 1:0);
                    break;
                case Command.truejump:
                    addr = stack.popAddr();
                    if (stack.pop() != 0)
                    {
                        pc = addr;
                    }
                    break;
                case Command.jump:
                    pc = stack.popAddr();
                    break;
                case Command.call:
                    callstack.Add(pc);
                    pc = stack.popAddr();
                    break;
                case Command.ret:
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
                case Command.load:
                    stack.push(Memory[stack.popAddr()]);
                    break;
                case Command.store:
                    Memory[stack.popAddr()] = stack.pop();
                    break;
                case Command.dumpkey:
                    stack.push(getKeyState());
                    break;
                case Command.redraw:
                    onRedraw(graphicsStack.ToArray());
                    graphicsStack.Clear();
                    break;
                case Command.rect:
                    break;
            }
            //PC
            switch ((Command)Memory[pc])
            {
                case Command.truejump:
                case Command.jump:
                case Command.call:
                    break;
                default:
                    pc++;
                    break;
            }
        }
    }
}
