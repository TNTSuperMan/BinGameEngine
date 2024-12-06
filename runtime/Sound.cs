using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.SoundChannels;

namespace bgeruntime
{
    public class Sound
    {
        enum Flag
        {
            Rest, Note, Tuplet, Next
        }
        enum ChannelState
        {
            Square1, Square2, Triangle, Noise
        }

        static readonly SoundFormat format = new SoundFormat(SamplingFrequencyType.FourtyFourKHz, BitRateType.EightBit, ChannelType.Monaural);
        private static Flag GetFlag(byte data) => (Flag)((data & 0b11000000) >> 6);
        private static ISoundChannel Bin2Channel(int tempo, ChannelState c, byte[] data)
        {
            ISoundChannel? channel;
            switch (c)
            {
                case ChannelState.Square1:
                case ChannelState.Square2:
                    channel = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point125, PanType.Both);
                    break;
                case ChannelState.Triangle:
                    channel = new PseudoTriangleSoundChannel(tempo, format, PanType.Both);
                    break;
                case ChannelState.Noise:
                    channel = new LowBitNoiseSoundChannel(tempo, format, PanType.Both);
                    break;
                default:
                    throw new Exception();
            }
            for(int i = 0;i < data.Length; i++)
            {
                LengthType len = (LengthType)(data[i] & 0b111111);
                switch (GetFlag(data[i]))
                {
                    case Flag.Rest:
                        channel.Add(new Rest(len));
                        break;
                    case Flag.Note:
                        i++;if (i >= data.Length) return channel;
                        channel.Add(new Note(
                            (Scale)(data[i] & 0b00001111),
                                   (data[i] & 0b11110000) >> 4,
                            len));
                        break;
                }
            }
            return channel;
        }
        public static byte[][] Bin2WavBins(byte[] data)
        {
            List<byte[]> parts = new();
            List<ISoundChannel> part = new();
            List<byte> partBytes = new();
            int pcount = 0;
            int tempo = 0;
            bool beforeNote = false;
            for(int i = 0;i < data.Length; i++)
            {
                if(tempo == 0)
                {
                    tempo = data[i];
                    continue;
                }
                if (GetFlag(data[i]) == Flag.Next && !beforeNote)
                {
                    try
                    {
                        part.Add(Bin2Channel(tempo, (ChannelState)pcount, partBytes.ToArray()));
                    }
                    catch
                    {
                        return [];
                    }
                    partBytes.Clear();
                    pcount++;
                    if (pcount >= 3)
                    {
                        var wave = new MonauralMixer(part).Mix();
                        var sound = new SoundMaker.WaveFile.SoundWaveChunk(wave.GetBytes(format.BitRate));
                        var wformat = new SoundMaker.WaveFile.FormatChunk(
                            (SoundMaker.WaveFile.SamplingFrequencyType)format.SamplingFrequency, 
                            (SoundMaker.WaveFile.BitRateType)format.BitRate,
                             SoundMaker.WaveFile.ChannelType.Monaural);
                        var stream = new MemoryStream();
                        var writer = new SoundMaker.WaveFile.WaveWriter(wformat, sound);
                        writer.Write(stream);
                        parts.Add(stream.ToArray());
                        stream.Close();

                        pcount = 0;
                        part.Clear();
                        tempo = 0;
                    }
                }
                else
                {
                    if(beforeNote) beforeNote = GetFlag(data[i]) == Flag.Note;
                    partBytes.Add(data[i]);
                }
            }

            return parts.ToArray();
        }
    }
    public partial class Runtime
    {
        private byte[][] sounds;
    }
}
