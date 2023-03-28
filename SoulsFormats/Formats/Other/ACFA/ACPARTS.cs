using System;
using System.Collections.Generic;

namespace SoulsFormats.Formats.Other.ACFA
{
    /// <summary>
    /// A part configuration format used in 4th generation Armored Core.
    /// </summary>
    public partial class ACPARTS : SoulsFile<ACPARTS>
    {
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
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = true;

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
            {

                Heads.Add(new Head(br));
            }

            Cores = new List<Core>(coreCount);
            for (int i = 0; i < coreCount; i++)
            {
                Cores.Add(new Core(br));
            }

            Arms = new List<Arm>(armCount);
            for (int i = 0; i < armCount; i++)
            {
                Arms.Add(new Arm(br));
            }

            Legs = new List<Leg>(legCount);
            for (int i = 0; i < legCount; i++)
            {
                Legs.Add(new Leg(br));
            }

            FCSs = new List<FCS>(fcsCount);
            for (int i = 0; i < fcsCount; i++)
            {
                FCSs.Add(new FCS(br));
            }

            Generators = new List<Generator>(generatorCount);
            for (int i = 0; i < generatorCount; i++)
            {
                Generators.Add(new Generator(br));
            }

            MainBoosters = new List<MainBooster>(mainBoosterCount);
            for (int i = 0; i < mainBoosterCount; i++)
            {
                MainBoosters.Add(new MainBooster(br));
            }

            BackBoosters = new List<BackBooster>(backBoosterCount);
            for (int i = 0; i < backBoosterCount; i++)
            {
                BackBoosters.Add(new BackBooster(br));
            }

            SideBoosters = new List<SideBooster>(sideBoosterCount);
            for (int i = 0; i < sideBoosterCount; i++)
            {
                SideBoosters.Add(new SideBooster(br));
            }

            OveredBoosters = new List<OveredBooster>(overedBoosterCount);
            for (int i = 0; i < overedBoosterCount; i++)
            {
                OveredBoosters.Add(new OveredBooster(br));
            }

            ArmUnits = new List<ArmUnit>(armUnitCount);
            for (int i = 0; i < armUnitCount; i++)
            {
                ArmUnits.Add(new ArmUnit(br));
            }

            BackUnits = new List<BackUnit>(backUnitCount);
            for (int i = 0; i < backUnitCount; i++)
            {
                BackUnits.Add(new BackUnit(br));
            }

            ShoulderUnits = new List<ShoulderUnit>(shoulderUnitCount);
            for (int i = 0; i < shoulderUnitCount; i++)
            {
                ShoulderUnits.Add(new ShoulderUnit(br));
            }

            HeadTopStabilizers = new List<HeadTopStabilizer>(headTopStabilizerCount);
            for (int i = 0; i < headTopStabilizerCount; i++)
            {
                HeadTopStabilizers.Add(new HeadTopStabilizer(br));
            }

            HeadSideStabilizers = new List<HeadSideStabilizer>(headSideStabilizerCount);
            for (int i = 0; i < headSideStabilizerCount; i++)
            {
                HeadSideStabilizers.Add(new HeadSideStabilizer(br));
            }

            CoreUpperSideStabilizers = new List<CoreUpperSideStabilizer>(coreUpperSideStabilizerCount);
            for (int i = 0; i < coreUpperSideStabilizerCount; i++)
            {
                CoreUpperSideStabilizers.Add(new CoreUpperSideStabilizer(br));
            }

            CoreLowerSideStabilizers = new List<CoreLowerSideStabilizer>(coreLowerSideStabilizerCount);
            for (int i = 0; i < coreLowerSideStabilizerCount; i++)
            {
                CoreLowerSideStabilizers.Add(new CoreLowerSideStabilizer(br));
            }

            ArmStabilizers = new List<ArmStabilizer>(armStabilizerCount);
            for (int i = 0; i < armStabilizerCount; i++)
            {
                ArmStabilizers.Add(new ArmStabilizer(br));
            }

            LegBackStabilizers = new List<LegBackStabilizer>(legBackStabilizerCount);
            for (int i = 0; i < legBackStabilizerCount; i++)
            {
                LegBackStabilizers.Add(new LegBackStabilizer(br));
            }

            LegUpperStabilizers = new List<LegUpperStabilizer>(legUpperStabilizerCount);
            for (int i = 0; i < legUpperStabilizerCount; i++)
            {
                LegUpperStabilizers.Add(new LegUpperStabilizer(br));
            }

            LegMiddleStabilizers = new List<LegMiddleStabilizer>(legMiddleStabilizerCount);
            for (int i = 0; i < legMiddleStabilizerCount; i++)
            {
                LegMiddleStabilizers.Add(new LegMiddleStabilizer(br));
            }

            LegLowerStabilizers = new List<LegLowerStabilizer>(legLowerStabilizerCount);
            for (int i = 0; i < legLowerStabilizerCount; i++)
            {
                LegLowerStabilizers.Add(new LegLowerStabilizer(br));
            }
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.WriteUInt32(0);
            bw.WriteUInt16((ushort)Heads.Count);
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
            bw.WriteUInt16((ushort)ShoulderUnits.Count) ;
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
            {
                Heads[i].Write(bw);
            }

            for (int i = 0; i < Cores.Count; i++)
            {
                Cores[i].Write(bw);
            }

            for (int i = 0; i < Arms.Count; i++)
            {
                Arms[i].Write(bw);
            }

            for (int i = 0; i < Legs.Count; i++)
            {
                Legs[i].Write(bw);
            }

            for (int i = 0; i < FCSs.Count; i++)
            {
                FCSs[i].Write(bw);
            }

            for (int i = 0; i < Generators.Count; i++)
            {
                Generators[i].Write(bw);
            }

            for (int i = 0; i < MainBoosters.Count; i++)
            {
                MainBoosters[i].Write(bw);
            }

            for (int i = 0; i < BackBoosters.Count; i++)
            {
                BackBoosters[i].Write(bw);
            }

            for (int i = 0; i < SideBoosters.Count; i++)
            {
                SideBoosters[i].Write(bw);
            }

            for (int i = 0; i < OveredBoosters.Count; i++)
            {
                OveredBoosters[i].Write(bw);
            }

            for (int i = 0; i < ArmUnits.Count; i++)
            {
                ArmUnits[i].Write(bw);
            }

            for (int i = 0; i < BackUnits.Count; i++)
            {
                BackUnits[i].Write(bw);
            }

            for (int i = 0; i < ShoulderUnits.Count; i++)
            {
                ShoulderUnits[i].Write(bw);
            }

            for (int i = 0; i < HeadTopStabilizers.Count; i++)
            {
                HeadTopStabilizers[i].Write(bw);
            }

            for (int i = 0; i < HeadSideStabilizers.Count; i++)
            {
                HeadSideStabilizers[i].Write(bw);
            }

            for (int i = 0; i < CoreUpperSideStabilizers.Count; i++)
            {
                CoreUpperSideStabilizers[i].Write(bw);
            }

            for (int i = 0; i < CoreLowerSideStabilizers.Count; i++)
            {
                CoreLowerSideStabilizers[i].Write(bw);
            }

            for (int i = 0; i < ArmStabilizers.Count; i++)
            {
                ArmStabilizers[i].Write(bw);
            }

            for (int i = 0; i < LegBackStabilizers.Count; i++)
            {
                LegBackStabilizers[i].Write(bw);
            }

            for (int i = 0; i < LegUpperStabilizers.Count; i++)
            {
                LegUpperStabilizers[i].Write(bw);
            }

            for (int i = 0; i < LegMiddleStabilizers.Count; i++)
            {
                LegMiddleStabilizers[i].Write(bw);
            }

            for (int i = 0; i < LegLowerStabilizers.Count; i++)
            {
                LegLowerStabilizers[i].Write(bw);
            }
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
                Console.WriteLine($"{e.Message} occurred, there is likely no parts read yet.");
                return 0;
            }
        }
    }
}
