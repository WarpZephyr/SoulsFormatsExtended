using System;
using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// A part configuration format used in the 4th generation Armored Core.
    /// </summary>
    public partial class AcParts4 : SoulsFile<AcParts4>
    {
        /// <summary>
        /// Version to separate reading and writing AcParts in AC4 and ACFA.
        /// Values unique to each version will be defaulted.
        /// </summary>
        public enum AcParts4Version
        {
            /// <summary>
            /// Armored Core 4, has less stats than Armored Core For Answer.
            /// </summary>
            AC4,

            /// <summary>
            /// Armored Core For Answer, has more stats than Armored Core 4.
            /// </summary>
            ACFA
        }

        /// <summary>
        /// A version identifier used to separate AC4 and ACFA, not in the files.
        /// Values unique to each version will be defaulted.
        /// </summary>
        public AcParts4Version Version { get; set; }

        /// <summary>
        /// Heads in this ACPARTS file.
        /// </summary>
        public List<Head> Heads { get; set; }

        /// <summary>
        /// Cores in this ACPARTS file.
        /// </summary>
        public List<Core> Cores { get; set; }

        /// <summary>
        /// Arms in this ACPARTS file.
        /// </summary>
        public List<Arm> Arms { get; set; }

        /// <summary>
        /// Legs in this ACPARTS file.
        /// </summary>
        public List<Leg> Legs { get; set; }

        /// <summary>
        /// FCSs in this ACPARTS file.
        /// </summary>
        public List<FCS> FCSs { get; set; }

        /// <summary>
        /// Generators in this ACPARTS file.
        /// </summary>
        public List<Generator> Generators { get; set; }

        /// <summary>
        /// Main Boosters in this ACPARTS file.
        /// </summary>
        public List<MainBooster> MainBoosters { get; set; }

        /// <summary>
        /// Back Boosters in this ACPARTS file.
        /// </summary>
        public List<BackBooster> BackBoosters { get; set; }

        /// <summary>
        /// Side Boosters in this ACPARTS file.
        /// </summary>
        public List<SideBooster> SideBoosters { get; set; }

        /// <summary>
        /// Overed Boosters in this ACPARTS file.
        /// </summary>
        public List<OveredBooster> OveredBoosters { get; set; }

        /// <summary>
        /// Arm Units in this ACPARTS file.
        /// </summary>
        public List<ArmUnit> ArmUnits { get; set; }

        /// <summary>
        /// Back Units in this ACPARTS file.
        /// </summary>
        public List<BackUnit> BackUnits { get; set; }

        /// <summary>
        /// Shoulder Units in this ACPARTS file.
        /// </summary>
        public List<ShoulderUnit> ShoulderUnits { get; set; }

        /// <summary>
        /// Stabilizers on top of Head parts in this ACPARTS file.
        /// </summary>
        public List<HeadTopStabilizer> HeadTopStabilizers { get; set; }

        /// <summary>
        /// Stabilizers on the sides of Head parts in this ACPARTS file.
        /// </summary>
        public List<HeadSideStabilizer> HeadSideStabilizers { get; set; }

        /// <summary>
        /// Stabilizers on the upper sides of Core parts in this ACPARTS file.
        /// </summary>
        public List<CoreUpperSideStabilizer> CoreUpperSideStabilizers { get; set; }

        /// <summary>
        /// Stabilizers on the lower sides of Core parts in this ACPARTS file.
        /// </summary>
        public List<CoreLowerSideStabilizer> CoreLowerSideStabilizers { get; set; }

        /// <summary>
        /// Stabilizers on Arm parts in this ACPARTS file.
        /// </summary>
        public List<ArmStabilizer> ArmStabilizers { get; set; }

        /// <summary>
        /// Stabilizers on the back of Leg parts in this ACPARTS file.
        /// </summary>
        public List<LegBackStabilizer> LegBackStabilizers { get; set; }

        /// <summary>
        /// Stabilizers on the upper end of Leg parts in this ACPARTS file.
        /// </summary>
        public List<LegUpperStabilizer> LegUpperStabilizers { get; set; }

        /// <summary>
        /// Stabilizers in the middle of Leg parts in this ACPARTS file.
        /// </summary>
        public List<LegMiddleStabilizer> LegMiddleStabilizers { get; set; }

        /// <summary>
        /// Stabilizers on the lower end of Leg parts in this ACPARTS file.
        /// </summary>
        public List<LegLowerStabilizer> LegLowerStabilizers { get; set; }

        /// <summary>
        /// Returns true if the data appears to be an ACFA AcParts file.
        /// Not entirely foolproof, probably slow, probably eats more memory, only checks size in a range.
        /// Only valid for ACFA at the moment.
        /// </summary>
        public static bool Match(BinaryReaderEx br)
        {
            br.BigEndian = true;
            if (br.Length < 0x30)
                return false;

            long headLength = br.ReadUInt16() * 420;
            long coreLength = br.ReadUInt16() * 416;
            long armLength = br.ReadUInt16() * 536;
            long legLength = br.ReadUInt16() * 564;
            long fcsLength = br.ReadUInt16() * 416;
            long generatorLength = br.ReadUInt16() * 404;
            long mainBoosterLength = br.ReadUInt16() * 420;
            long backBoosterLength = br.ReadUInt16() * 404;
            long sideBoosterLength = br.ReadUInt16() * 404;
            long overedBoosterLength = br.ReadUInt16() * 432;
            long armUnitLength = br.ReadUInt16() * 456;
            long backUnitLength = br.ReadUInt16() * 524;
            long shoulderUnitLength = br.ReadUInt16() * 524;
            long headTopStabilizerLength = br.ReadUInt16() * 376;
            long headSideStabilizerLength = br.ReadUInt16() * 376;
            long coreUpperSideStabilizerLength = br.ReadUInt16() * 376;
            long coreLowerSideStabilizerLength = br.ReadUInt16() * 376;
            long armStabilizerLength = br.ReadUInt16() * 376;
            long legBackStabilizerLength = br.ReadUInt16() * 376;
            long legUpperStabilizerLength = br.ReadUInt16() * 376;
            long legMiddleStabilizerLength = br.ReadUInt16() * 376;
            long legLowerStabilizerLength = br.ReadUInt16() * 376;

            // 0x30 is header length
            long totalLength = 0x30 + headLength + coreLength + armLength + legLength + fcsLength
                + generatorLength + mainBoosterLength + backBoosterLength + sideBoosterLength + overedBoosterLength
                + armUnitLength + backUnitLength + shoulderUnitLength + headTopStabilizerLength + headSideStabilizerLength
                + coreUpperSideStabilizerLength + coreLowerSideStabilizerLength + armStabilizerLength + legBackStabilizerLength
                + legUpperStabilizerLength + legMiddleStabilizerLength + legLowerStabilizerLength;

            if (br.Length < totalLength || br.Length > totalLength * 1.1) // * 1.1 To handle extra garbage data that is on the end occasionally
                return false;
            else return true;
        }

        /// <summary>
        /// Loads an AcParts file from a byte array.
        /// </summary>
        public static AcParts4 Read(byte[] bytes, AcParts4Version version)
        {
            BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            AcParts4 acparts = new AcParts4();
            acparts.Read(br, version);
            return acparts;
        }

        /// <summary>
        /// Loads an AcParts file from the specified path.
        /// </summary>
        public static AcParts4 Read(string path, AcParts4Version version)
        {
            using FileStream stream = File.OpenRead(path);
            BinaryReaderEx br = new BinaryReaderEx(false, stream);
            AcParts4 acparts = new AcParts4();
            acparts.Read(br, version);
            return acparts;
        }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        private void Read(BinaryReaderEx br, AcParts4Version version)
        {
            br.BigEndian = true;
            Version = version;

            br.AssertUInt32(0);
            ushort headCount = br.ReadUInt16();
            ushort coreCount = br.ReadUInt16();
            ushort armCount = br.ReadUInt16();
            ushort legCount = br.ReadUInt16();
            ushort fcsCount = br.ReadUInt16();
            ushort generatorCount = br.ReadUInt16();
            ushort mainBoosterCount = br.ReadUInt16();
            ushort backBoosterCount = br.ReadUInt16();
            ushort sideBoosterCount = br.ReadUInt16();
            ushort overedBoosterCount = br.ReadUInt16();
            ushort armUnitCount = br.ReadUInt16();
            ushort backUnitCount = br.ReadUInt16();
            ushort shoulderUnitCount = br.ReadUInt16();
            ushort headTopStabilizerCount = br.ReadUInt16();
            ushort headSideStabilizerCount = br.ReadUInt16();
            ushort coreUpperSideStabilizerCount = br.ReadUInt16();
            ushort coreLowerSideStabilizerCount = br.ReadUInt16();
            ushort armStabilizerCount = br.ReadUInt16();
            ushort legBackStabilizerCount = br.ReadUInt16();
            ushort legUpperStabilizerCount = br.ReadUInt16();
            ushort legMiddleStabilizerCount = br.ReadUInt16();
            ushort legLowerStabilizerCount = br.ReadUInt16();

            Heads = new List<Head>(headCount);
            for (int i = 0; i < headCount; i++)
                Heads.Add(new Head(br, Version));

            Cores = new List<Core>(coreCount);
            for (int i = 0; i < coreCount; i++)
                Cores.Add(new Core(br, Version));
            
            Arms = new List<Arm>(armCount);
            for (int i = 0; i < armCount; i++)
                Arms.Add(new Arm(br, Version));
            
            Legs = new List<Leg>(legCount);
            for (int i = 0; i < legCount; i++)
                Legs.Add(new Leg(br, Version));
            
            FCSs = new List<FCS>(fcsCount);
            for (int i = 0; i < fcsCount; i++)
                FCSs.Add(new FCS(br, Version));
            
            Generators = new List<Generator>(generatorCount);
            for (int i = 0; i < generatorCount; i++)
                Generators.Add(new Generator(br, Version));

            MainBoosters = new List<MainBooster>(mainBoosterCount);
            for (int i = 0; i < mainBoosterCount; i++)
                MainBoosters.Add(new MainBooster(br, Version));

            BackBoosters = new List<BackBooster>(backBoosterCount);
            for (int i = 0; i < backBoosterCount; i++)
                BackBoosters.Add(new BackBooster(br, Version));

            SideBoosters = new List<SideBooster>(sideBoosterCount);
            for (int i = 0; i < sideBoosterCount; i++)
                SideBoosters.Add(new SideBooster(br, Version));
            
            OveredBoosters = new List<OveredBooster>(overedBoosterCount);
            for (int i = 0; i < overedBoosterCount; i++)
                OveredBoosters.Add(new OveredBooster(br, Version));

            ArmUnits = new List<ArmUnit>(armUnitCount);
            for (int i = 0; i < armUnitCount; i++)
                ArmUnits.Add(new ArmUnit(br, Version));

            BackUnits = new List<BackUnit>(backUnitCount);
            for (int i = 0; i < backUnitCount; i++)
                BackUnits.Add(new BackUnit(br, Version));

            ShoulderUnits = new List<ShoulderUnit>(shoulderUnitCount);
            for (int i = 0; i < shoulderUnitCount; i++)
                ShoulderUnits.Add(new ShoulderUnit(br, Version));

            HeadTopStabilizers = new List<HeadTopStabilizer>(headTopStabilizerCount);
            for (int i = 0; i < headTopStabilizerCount; i++)
                HeadTopStabilizers.Add(new HeadTopStabilizer(br, Version));

            HeadSideStabilizers = new List<HeadSideStabilizer>(headSideStabilizerCount);
            for (int i = 0; i < headSideStabilizerCount; i++)
                HeadSideStabilizers.Add(new HeadSideStabilizer(br, Version));

            CoreUpperSideStabilizers = new List<CoreUpperSideStabilizer>(coreUpperSideStabilizerCount);
            for (int i = 0; i < coreUpperSideStabilizerCount; i++)
                CoreUpperSideStabilizers.Add(new CoreUpperSideStabilizer(br, Version));

            CoreLowerSideStabilizers = new List<CoreLowerSideStabilizer>(coreLowerSideStabilizerCount);
            for (int i = 0; i < coreLowerSideStabilizerCount; i++)
                CoreLowerSideStabilizers.Add(new CoreLowerSideStabilizer(br, Version));

            ArmStabilizers = new List<ArmStabilizer>(armStabilizerCount);
            for (int i = 0; i < armStabilizerCount; i++)
                ArmStabilizers.Add(new ArmStabilizer(br, Version));

            LegBackStabilizers = new List<LegBackStabilizer>(legBackStabilizerCount);
            for (int i = 0; i < legBackStabilizerCount; i++)
                LegBackStabilizers.Add(new LegBackStabilizer(br, Version));

            LegUpperStabilizers = new List<LegUpperStabilizer>(legUpperStabilizerCount);
            for (int i = 0; i < legUpperStabilizerCount; i++)
                LegUpperStabilizers.Add(new LegUpperStabilizer(br, Version));

            LegMiddleStabilizers = new List<LegMiddleStabilizer>(legMiddleStabilizerCount);
            for (int i = 0; i < legMiddleStabilizerCount; i++)
                LegMiddleStabilizers.Add(new LegMiddleStabilizer(br, Version));

            LegLowerStabilizers = new List<LegLowerStabilizer>(legLowerStabilizerCount);
            for (int i = 0; i < legLowerStabilizerCount; i++)
                LegLowerStabilizers.Add(new LegLowerStabilizer(br, Version));
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = true;
            bw.WriteUInt32(0);
            bw.WriteUInt16((ushort)Heads.Count);
            bw.WriteUInt16((ushort)Cores.Count);
            bw.WriteUInt16((ushort)Arms.Count);
            bw.WriteUInt16((ushort)Legs.Count);
            bw.WriteUInt16((ushort)FCSs.Count);
            bw.WriteUInt16((ushort)Generators.Count);
            bw.WriteUInt16((ushort)MainBoosters.Count);
            bw.WriteUInt16((ushort)BackBoosters.Count);
            bw.WriteUInt16((ushort)SideBoosters.Count);
            bw.WriteUInt16((ushort)OveredBoosters.Count);
            bw.WriteUInt16((ushort)ArmUnits.Count);
            bw.WriteUInt16((ushort)BackUnits.Count);
            bw.WriteUInt16((ushort)ShoulderUnits.Count);
            bw.WriteUInt16((ushort)HeadTopStabilizers.Count);
            bw.WriteUInt16((ushort)HeadSideStabilizers.Count);
            bw.WriteUInt16((ushort)CoreUpperSideStabilizers.Count);
            bw.WriteUInt16((ushort)CoreLowerSideStabilizers.Count);
            bw.WriteUInt16((ushort)ArmStabilizers.Count);
            bw.WriteUInt16((ushort)LegBackStabilizers.Count);
            bw.WriteUInt16((ushort)LegUpperStabilizers.Count);
            bw.WriteUInt16((ushort)LegMiddleStabilizers.Count);
            bw.WriteUInt16((ushort)LegLowerStabilizers.Count);

            for (int i = 0; i < Heads.Count; i++)
                Heads[i].Write(bw, Version);

            for (int i = 0; i < Cores.Count; i++)
                Cores[i].Write(bw, Version);

            for (int i = 0; i < Arms.Count; i++)
                Arms[i].Write(bw, Version);

            for (int i = 0; i < Legs.Count; i++)
                Legs[i].Write(bw, Version);

            for (int i = 0; i < FCSs.Count; i++)
                FCSs[i].Write(bw, Version);

            for (int i = 0; i < Generators.Count; i++)
                Generators[i].Write(bw, Version);

            for (int i = 0; i < MainBoosters.Count; i++)
                MainBoosters[i].Write(bw, Version);

            for (int i = 0; i < BackBoosters.Count; i++)
                BackBoosters[i].Write(bw, Version);

            for (int i = 0; i < SideBoosters.Count; i++)
                SideBoosters[i].Write(bw, Version);

            for (int i = 0; i < OveredBoosters.Count; i++)
                OveredBoosters[i].Write(bw, Version);

            for (int i = 0; i < ArmUnits.Count; i++)
                ArmUnits[i].Write(bw, Version);

            for (int i = 0; i < BackUnits.Count; i++)
                BackUnits[i].Write(bw, Version);

            for (int i = 0; i < ShoulderUnits.Count; i++)
                ShoulderUnits[i].Write(bw, Version);

            for (int i = 0; i < HeadTopStabilizers.Count; i++)
                HeadTopStabilizers[i].Write(bw, Version);

            for (int i = 0; i < HeadSideStabilizers.Count; i++)
                HeadSideStabilizers[i].Write(bw, Version);

            for (int i = 0; i < CoreUpperSideStabilizers.Count; i++)
                CoreUpperSideStabilizers[i].Write(bw, Version);

            for (int i = 0; i < CoreLowerSideStabilizers.Count; i++)
                CoreLowerSideStabilizers[i].Write(bw, Version);

            for (int i = 0; i < ArmStabilizers.Count; i++)
                ArmStabilizers[i].Write(bw, Version);

            for (int i = 0; i < LegBackStabilizers.Count; i++)
                LegBackStabilizers[i].Write(bw, Version);

            for (int i = 0; i < LegUpperStabilizers.Count; i++)
                LegUpperStabilizers[i].Write(bw, Version);

            for (int i = 0; i < LegMiddleStabilizers.Count; i++)
                LegMiddleStabilizers[i].Write(bw, Version);

            for (int i = 0; i < LegLowerStabilizers.Count; i++)
                LegLowerStabilizers[i].Write(bw, Version);
        }

        /// <summary>
        /// Gets the number of parts in an ACPARTS file.
        /// </summary>
        /// <returns>An int representing the number of parts in an ACPARTS file.</returns>
        public int Count()
        {
            try
            {
                return Heads.Count +
                Cores.Count +
                Arms.Count +
                Legs.Count +
                FCSs.Count +
                Generators.Count +
                MainBoosters.Count +
                BackBoosters.Count +
                SideBoosters.Count +
                OveredBoosters.Count +
                ArmUnits.Count +
                BackUnits.Count +
                ShoulderUnits.Count +
                HeadTopStabilizers.Count +
                HeadSideStabilizers.Count +
                CoreUpperSideStabilizers.Count +
                CoreLowerSideStabilizers.Count +
                ArmStabilizers.Count +
                LegBackStabilizers.Count +
                LegUpperStabilizers.Count +
                LegMiddleStabilizers.Count +
                LegLowerStabilizers.Count;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"An error occurred, there is likely no parts read yet. Error: {e.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Get all the parts in this AcParts as a list of IPart.
        /// </summary>
        /// <returns>A list of IPart.</returns>
        public List<IPart> GetParts()
        {
            List<IPart> parts = new List<IPart>();
            parts.AddRange(Heads);
            parts.AddRange(Cores);
            parts.AddRange(Arms);
            parts.AddRange(Legs);
            parts.AddRange(FCSs);
            parts.AddRange(Generators);
            parts.AddRange(MainBoosters);
            parts.AddRange(BackBoosters);
            parts.AddRange(SideBoosters);
            parts.AddRange(OveredBoosters);
            parts.AddRange(ArmUnits);
            parts.AddRange(BackUnits);
            parts.AddRange(ShoulderUnits);
            parts.AddRange(HeadTopStabilizers);
            parts.AddRange(HeadSideStabilizers);
            parts.AddRange(CoreUpperSideStabilizers);
            parts.AddRange(CoreLowerSideStabilizers);
            parts.AddRange(ArmStabilizers);
            parts.AddRange(LegBackStabilizers);
            parts.AddRange(LegUpperStabilizers);
            parts.AddRange(LegMiddleStabilizers);
            parts.AddRange(LegLowerStabilizers);
            return parts;
        }
    }
}
