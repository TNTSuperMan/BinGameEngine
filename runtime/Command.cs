﻿namespace bgeruntime
{
    public partial class Runtime
    {
        private bool isEnded = false;

        private void StackOperate(Opr opr) => stack.Push((byte)opr(stack.Pop(), stack.Pop()));
        delegate int Opr(byte b, byte a);
        private Opr add = (b, a) => a + b;
        private Opr sub = (b, a) => a - b;
        private Opr mul = (b, a) => a * b;
        private Opr div = (b, a) => a / b;
        private Opr rem = (b, a) => a % b;
        private Opr nand = (b, a) => ~(a & b);
        private Opr greater = (b, a) => a > b ? 1:0;
        private Opr equal = (b, a) => a == b ? 1:0;
        public enum Command: byte
        {
            nop,push,pop,cls,
            add,sub,mul,div,rem,nand,equal,greater,
            truejump,jump,call,ret,
            load,store,
            dumpkey,
            redraw,rect,graph,
            sound,stopsound,
            io, breakpoint
        }
        public ushort popAddr()
        {
            byte bottom = stack.Pop();
            byte top = stack.Pop();
            return (ushort)((top << 8) | bottom);
        }
        public int EmulateCount;
        public bool EmulateFrame()
        {
            EmulateCount = 0;
            while ( EmulateCount < 1000000 &&
                memory.Load(pc) != (byte)Command.redraw &&
                memory.Load(pc) != (byte)Command.breakpoint && !isEnded)
                EmulateNext();
            if (!isEnded && memory.Load(pc) == (byte)Command.breakpoint)
            {
                EmulateNext();
                return true;
            }
            else if (!isEnded)
            {
                EmulateNext();
            }
            return false;
        }
        public void EmulateNext()
        {
            EmulateCount++;
            ushort addr;
            switch ((Command)memory.Load(pc))
            {
                case Command.nop:
                    break;
                case Command.push:
                    stack.Push(memory.Load(++pc));
                    break;
                case Command.pop:
                    stack.Pop();
                    break;
                case Command.cls:
                    stack.Clear();
                    break;
                case Command.add:
                    StackOperate(add);
                    break;
                case Command.sub:
                    StackOperate(sub);
                    break;
                case Command.mul:
                    StackOperate(mul);
                    break;
                case Command.div:
                    StackOperate(div);
                    break;
                case Command.rem:
                    StackOperate(rem);
                    break;
                case Command.nand:
                    StackOperate(nand);
                    break;
                case Command.greater:
                    StackOperate(greater);
                    break;
                case Command.equal:
                    StackOperate(equal);
                    break;
                case Command.truejump:
                    addr = popAddr();
                    if (stack.Pop() != 0)
                    {
                        pc = addr;
                        return;
                    }
                    break;
                case Command.jump:
                    pc = popAddr();
                    return;
                case Command.call:
                    callstack.Push(pc);
                    pc = popAddr();
                    return;
                case Command.ret:
                    if(callstack.Count == 0)
                    {
                        isEnded = true;
                        onEnd();
                    }
                    else
                    {
                        addr = callstack.Pop();
                        pc = addr;
                    }
                    break;
                case Command.load:
                    stack.Push(memory.Load(popAddr()));
                    break;
                case Command.store:
                    memory.Store(popAddr(), stack.Pop());
                    break;
                case Command.dumpkey:
                    stack.Push(getKeyState());
                    break;
                case Command.redraw:
                    onRedraw(redrawStack.ToArray());
                    redrawStack.Clear();
                    break;
                case Command.rect:
                    if (createRectangle == null) throw new NotImplementedException("Not Implemented createRectangle Function.\nPlease substitute [runtime var].createRectangle");
                    redrawStack.Add(createRectangle(
                        c : Graphic.ToColor(stack.Pop()),
                        h : stack.Pop(),
                        w : stack.Pop(),
                        y : stack.Pop(),
                        x : stack.Pop()));
                    break;
                case Command.graph:
                    byte id = stack.Pop(), y = stack.Pop(), x = stack.Pop();
                    if (graphics.Length > id)
                        redrawStack.Add(graphics[id].ImgAt(x, y));
                    break;
                case Command.sound:
                    byte id_ = stack.Pop();
                    if (id_ < sounds.Length) onSound(sounds[id_]);
                    break;
                case Command.stopsound:
                    onStopSound();
                    break;
                case Command.io:
                    byte[] data = memory.LoadIO();
                    switch (stack.Pop())
                    {
                        case 0: //Graphics
                            if (createGraphic == null) throw new NotImplementedException("Not Implemented createGraphic Function.\nPlease substitute [runtime var].createGraphic");
                            graphics = Graphic.Bin2Graphics(data, createGraphic);
                            break;
                        case 1: //Sound
                            sounds = SoundGenerator.Bin2WavBins(data);
                            break;
                        case 2: //Load
                            data = onLoad();
                            for(int i = 0;i < 0x1000; i++)
                                memory.Store((ushort)(0xf000 + i), data.Length > i ? data[i] : (byte)0);
                            break;
                        case 3: //Save
                            onSave(data);
                            break;
                        case 4: //Clear
                            for (int i = 0; i < 0x1000; i++)
                                memory.Store((ushort)(i + 0xf000), 0);
                            break;
                    }
                    break;
                case Command.breakpoint:
                    break;
            }
            pc++;
        }
    }
}
