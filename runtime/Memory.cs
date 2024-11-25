namespace bgeruntime
{
    internal class Memory
    {
        private readonly byte[] rom = new byte[0xa000];
        private byte[] ram = new byte[0x6000];
        public Memory(byte[] rom)
        {
            for (int i = 0; i < rom.Length; i++)
                this.rom[i] = rom[i];
        }
        public byte Load(ushort addr)
        { 
            if(addr < 0xa000)
            {
                return rom[addr];
            }
            else
            {
                return ram[addr - 0xa000];
            }
        }
        public void Store(ushort addr, byte value)
        {
            if(addr >= 0xa000)
            {
                ram[addr - 0xa000] = value;
            }
        }
    }
}
