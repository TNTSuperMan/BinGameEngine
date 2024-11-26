namespace bgeruntime
{
    public partial class Runtime
    {
        private bool isEnded = false;

        private void StackOperate(Opr opr) => stack.push((byte)opr(stack.pop(), stack.pop()));
        delegate int Opr(byte b, byte a);
        private Opr add = (b, a) => a + b;
        private Opr sub = (b, a) => a - b;
        private Opr mul = (b, a) => a * b;
        private Opr div = (b, a) => a / b;
        private Opr rem = (b, a) => a % b;
        private Opr nand = (b, a) => ~(a & b);
        private Opr greater = (b, a) => a > b ? 1:0;
        private Opr equal = (b, a) => a == b ? 1:0;
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
            while (memory.Load(pc) != (byte)Command.redraw && !isEnded)
                EmulateNext();
        }
        public void EmulateNext()
        {
            ushort addr;
            switch ((Command)memory.Load(pc))
            {
                case Command.push:
                    stack.push(memory.Load(++pc));
                    break;
                case Command.pop:
                    stack.pop();
                    break;
                case Command.cls:
                    stack.clear();
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
                    addr = stack.popAddr();
                    if (stack.pop() != 0)
                    {
                        pc = addr;
                    }
                    else
                    {
                        pc++;
                    }
                    return;
                case Command.jump:
                    pc = stack.popAddr();
                    return;
                case Command.call:
                    callstack.Add(pc);
                    pc = stack.popAddr();
                    return;
                case Command.ret:
                    if(callstack.Count == 0)
                    {
                        isEnded = true;
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
                    stack.push(memory.Load(stack.popAddr()));
                    break;
                case Command.store:
                    memory.Store(stack.popAddr(), stack.pop());
                    break;
                case Command.dumpkey:
                    stack.push(getKeyState());
                    break;
                case Command.redraw:
                    onRedraw(redrawStack.ToArray());
                    redrawStack.Clear();
                    break;
                case Command.rect:
                    redrawStack.Add(new(stack.pop(), stack.pop(), stack.pop(), stack.pop(), stack.pop()));
                    break;
                case Command.graph:
                    byte id = stack.pop(), y = stack.pop(), x = stack.pop();
                    foreach (var g in graphics[id].Draw(x, y))
                        redrawStack.Add(g);
                    break;
                case Command.io:
                    byte[] data = memory.LoadIO();
                    switch (stack.pop())
                    {
                        case 0: //Graphics
                            List<byte> graphStack = new();
                            graphics.Clear();
                            for (int i = 0;i < 0x1000; i++)
                            {
                                if ((data[i] & 0b11000000) << 6 == 0b11)
                                {
                                    graphics.Add(new(graphStack.ToArray()));
                                    graphStack.Clear();
                                }
                                else graphStack.Add(data[i]);
                            }
                            break;
                        case 1: //Sound
                            break;
                        case 2: //Load
                            data = onLoad();
                            for(int i = 0;i < 0x1000; i++)
                                memory.Store((byte)(0xf000 + i), data[i]);
                            break;
                        case 3: //Save
                            onSave(data);
                            break;
                    }
                    break;
            }
            pc++;
        }
    }
}
