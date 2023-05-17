using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// Debriefing video subtitle files for Armored Core.
    /// Only Armored Core For Answer is currently supported.
    /// </summary>
    public class DebriefingSubtitle : SoulsFile<DebriefingSubtitle>
    {
        /// <summary>
        /// The subtitles inside of the subtitle file.
        /// </summary>
        public List<Subtitle> Subtitles;

        /// <summary>
        /// Unknown; Is always 1, affects subtitle text in strange ways if edited.
        /// </summary>
        public uint Unk0C;

        /// <summary>
        /// Unknown; Is usually 0.
        /// </summary>
        public ushort Unk14;

        /// <summary>
        /// Unknown; Believed to be EventID for voice call videos.
        /// </summary>
        public ushort EventID;

        /// <summary>
        /// The name of the video file for this subtitle file without its extension.
        /// </summary>
        public string VideoName;

        /// <summary>
        /// How wide the video for this subtitle file is.
        /// </summary>
        public short Width;

        /// <summary>
        /// How tall video for this subtitle file is.
        /// </summary>
        public short Height;

        /// <summary>
        /// Returns true if the data appears to be a Debriefing Subtitle container.
        /// Not entirely foolproof as it relies on size and offset validation.
        /// </summary>
        public static bool Match(BinaryReaderEx br)
        {
            br.BigEndian = true;
            if (br.Length < 0x20)
                return false;

            uint subtitleOffset = br.ReadUInt32();
            uint subtitleCount = br.ReadUInt32();
            uint subtitleEntriesOffset = br.ReadUInt32();
            br.Skip(4);
            uint videoOffset = br.ReadUInt32();
            br.Skip(4);
            uint unk18 = br.ReadUInt32();
            uint unk1C = br.ReadUInt32();

            if (subtitleOffset > br.Length
             || subtitleEntriesOffset > br.Length
             || videoOffset > br.Length
             || subtitleCount * 0x10 > br.Length
             || unk18 != 0
             || unk1C != 0
             || subtitleEntriesOffset > videoOffset
             || subtitleOffset < videoOffset
             || subtitleOffset < subtitleEntriesOffset
             || videoOffset < subtitleCount * 0x10 + subtitleEntriesOffset)
                return false;
            else return true;
        }

        /// <summary>
        /// Returns true if the data appears to be a Debriefing Subtitle container.
        /// Not entirely foolproof as it relies on size and offset validation.
        /// </summary>
        public static bool Match(byte[] bytes)
        {
            if (bytes.Length == 0)
                return false;

            BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            return Match(SFUtil.GetDecompressedBR(br, out _));
        }

        /// <summary>
        /// Returns true if the data appears to be a Debriefing Subtitle container.
        /// Not entirely foolproof as it relies on size and offset validation.
        /// </summary>
        public static bool Match(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                if (stream.Length == 0)
                    return false;

                BinaryReaderEx br = new BinaryReaderEx(false, stream);
                return Match(SFUtil.GetDecompressedBR(br, out _));
            }
        }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = true;
            uint subtitleOffset = br.ReadUInt32();
            uint subtitleCount = br.ReadUInt32();
            uint subtitleEntriesOffset = br.ReadUInt32();
            Unk0C = br.ReadUInt32();
            uint videoOffset = br.ReadUInt32();
            Unk14 = br.ReadUInt16();
            EventID = br.ReadUInt16();
            uint unk18 = br.AssertUInt32(0);
            uint unk1C = br.AssertUInt32(0);

            br.StepIn(subtitleEntriesOffset);
            Subtitles = new List<Subtitle>();
            for (int i = 0; i < subtitleCount - 1; i++)
            {
                short frameDelay = br.ReadInt16();
                short frameTime = br.ReadInt16();
                uint textOffset = br.ReadUInt32();
                uint unk08 = br.AssertUInt32(0);
                uint unk0C = br.AssertUInt32(0);
                string text = br.GetUTF16(subtitleOffset + textOffset);
                var subtitle = new Subtitle(frameDelay, frameTime, text);
                Subtitles.Add(subtitle);
            }
            br.StepOut();

            br.StepIn(videoOffset);
            uint videoNameOffset = br.ReadUInt32();
            VideoName = br.GetUTF16(subtitleOffset + videoNameOffset);
            br.AssertUInt32(0);
            br.AssertUInt32(0);
            Width = br.ReadInt16();
            Height = br.ReadInt16();
            br.AssertUInt32(0);
            br.AssertUInt32(0);
            br.AssertUInt32(0);
            br.AssertUInt32(0);
            br.StepOut();
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = true;
            bw.ReserveUInt32("subtitleOffset");
            bw.WriteUInt32((uint)Subtitles.Count + 1);
            bw.ReserveUInt32("subtitleEntriesOffset");
            bw.WriteUInt32(Unk0C);
            bw.ReserveUInt32("videoOffset");
            bw.WriteUInt16(Unk14);
            bw.WriteUInt16(EventID);
            bw.WriteUInt32(0);
            bw.WriteUInt32(0);

            bw.FillUInt32("subtitleEntriesOffset", (uint)bw.Position);
            for (int i = 0; i < Subtitles.Count; i++)
            {
                var subtitle = Subtitles[i];
                bw.WriteInt16(subtitle.FrameDelay);
                bw.WriteInt16(subtitle.FrameTime);
                bw.ReserveUInt32($"textOffset_{i}");
                bw.WriteUInt32(0);
                bw.WriteUInt32(0);
            }

            // Empty subtitle
            bw.WriteInt16(10);
            bw.WriteInt16(0);
            bw.ReserveUInt32($"textOffset_{Subtitles.Count + 1}");
            bw.WriteUInt32(0);
            bw.WriteUInt32(0);

            bw.FillUInt32("videoOffset", (uint)bw.Position);
            bw.ReserveUInt32("videoNameOffset");
            bw.WriteUInt32(0);
            bw.WriteUInt32(0);
            bw.WriteInt16(Width);
            bw.WriteInt16(Height);
            bw.WriteUInt32(0);
            bw.WriteUInt32(0);
            bw.WriteUInt32(0);
            bw.WriteUInt32(0);

            uint subtitleOffset = (uint)bw.Position;
            bw.FillUInt32("subtitleOffset", subtitleOffset);
            for (int i = 0; i < Subtitles.Count; i++)
            {
                bw.FillUInt32($"textOffset_{i}", (uint)bw.Position - subtitleOffset);
                bw.WriteUTF16(Subtitles[i].Text, true);
                bw.WriteByte(0);
                bw.WriteByte(0);
            }

            // Fill empty subtitle
            bw.FillUInt32($"textOffset_{Subtitles.Count + 1}", (uint)bw.Position - subtitleOffset);
            bw.WriteUInt32(0x30000000);
            bw.WriteByte(0);
            bw.WriteByte(0);

            bw.FillUInt32("videoNameOffset", (uint)bw.Position - subtitleOffset);
            bw.WriteUTF16(VideoName, true);
            bw.WriteByte(0);
            bw.WriteByte(0);
        }

        /// <summary>
        /// A subtitle in a subtitle file.
        /// </summary>
        public class Subtitle
        {
            /// <summary>
            /// How many frames from the last subtitle or start until this subtitle shows.
            /// </summary>
            public short FrameDelay;

            /// <summary>
            /// How many frames the subtitle lasts for.
            /// </summary>
            public short FrameTime;

            /// <summary>
            /// The text in the subtitle.
            /// </summary>
            public string Text;

            /// <summary>
            /// Creates a new Subtitle.
            /// </summary>
            public Subtitle(short frameDelay, short frameTime, string text)
            {
                FrameDelay = frameDelay;
                FrameTime = frameTime;
                Text = text;
            }
        }
    }
}
