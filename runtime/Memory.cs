namespace bgeruntime
{
    internal class Memory
    {
        private readonly byte[] rom = new byte[0xa000];
        private byte[] ram = new byte[0x6000];
        public byte[] Value
        {
            get
            {
                return rom.Concat(ram).ToArray();
            }
            set
            {
                for(int i = 0;i < value.Length; i++)
                    ram[i] = value[i];
            }
        }
        public Memory(byte[] rom)
        {
            for (int i = 0; i < rom.Length; i++)
                this.rom[i] = rom[i];
        }
    }
}
