﻿/* MIT License
 * 
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * Copyright (c) 2020 colinator27
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace GameBreaker.Models;

/// <summary>
/// Contains a single code entry, which could be of an object's event, a timeline's step, a script, or a function (in GMS 2.3).
/// </summary>
public class GMCode : IGMSerializable
{
    /// <summary>
    /// The name of the code entry.
    /// </summary>
    public GMString Name;

    /// <summary>
    /// The length of the code entry in bytes.
    /// </summary>
    public int Length;

    /// <summary>
    /// The amount of locals this code entry references.
    /// </summary>
    public short LocalsCount;

    /// <summary>
    /// The amount of arguments this code entry expects.
    /// </summary>
    public short ArgumentsCount;

    /// <summary>
    /// TODO: unknown flags?
    /// </summary>
    public byte Flags;

    /// <summary>
    /// The offset at which the bytecode starts.
    /// </summary>
    public int BytecodeOffset;

    /// <summary>
    /// The bytecode instructions of this code entry.
    /// </summary>
    public Bytecode BytecodeEntry;

    /// <summary>
    /// The parent code entry, if it exists.
    /// </summary>
    public GMCode ParentEntry;

    /// <summary>
    /// A list of child code entries, if they exist.
    /// </summary>
    public readonly List<GMCode> ChildEntries = new List<GMCode>();

    public void Serialize(GMDataWriter writer)
    {
        writer.WritePointerString(Name);
        if (BytecodeEntry != null)
            Length = BytecodeEntry.GetLength() * 4;
        writer.Write(Length);

        if (writer.VersionInfo.FormatID <= 14)
        {
            BytecodeEntry.Serialize(writer);
        }
        else
        {
            writer.Write(LocalsCount);
            writer.Write((short)((int)ArgumentsCount | ((int)Flags << 13)));
            writer.Write(writer.PointerOffsets[BytecodeEntry] - writer.Offset);
            writer.Write(BytecodeOffset);
        }
    }

    public void Deserialize(GMDataReader reader)
    {
        Name = reader.ReadStringPointerObject();
        Length = reader.ReadInt32();

        // Early exit, if we're on <= 14
        if (reader.VersionInfo.FormatID <= 14)
        {
            BytecodeEntry = new Bytecode(this);
            BytecodeEntry.Deserialize(reader, Length);
            return;
        }

        LocalsCount = reader.ReadInt16();
        int v = reader.ReadInt16();
        ArgumentsCount = (short)(v & 0b1111111111111);
        Flags = (byte)(v >> 13);
        int relativeBytecodeAddr = reader.ReadInt32();
        int absoluteBytecodeAddr = (reader.Offset - 4) + relativeBytecodeAddr;
        bool childCandidate = false;
        if (reader.PointerOffsets.TryGetValue(absoluteBytecodeAddr, out IGMSerializable s))
        {
            if (s is Bytecode b)
            {
                BytecodeEntry = b;
                childCandidate = true;
            }
        }
        if (BytecodeEntry == null)
        {
            BytecodeEntry = new Bytecode(this);
            if (Length != 0) // prevent pointer overlap of entries with 0 instructions
                reader.PointerOffsets[absoluteBytecodeAddr] = BytecodeEntry;

            int returnTo = reader.Offset;
            reader.Offset = absoluteBytecodeAddr;

            BytecodeEntry.Deserialize(reader, Length);

            reader.Offset = returnTo;
        }
        BytecodeOffset = reader.ReadInt32();

        if (childCandidate && Length != 0 && BytecodeOffset != 0)
        {
            // Assign parents and children of this entry
            ParentEntry = BytecodeEntry.Parent;
            BytecodeEntry.Parent.ChildEntries.Add(this);
        }
    }

    public override string ToString()
    {
        return $"Code: \"{Name.Content}\"";
    }

    /// <summary>
    /// A sequence of GameMaker bytecode instructions.
    /// </summary>
    public class Bytecode : IGMSerializable
    {
        /// <summary>
        /// The <see cref="GMCode"/> this bytecode belongs to.
        /// </summary>
        public GMCode Parent;

        /// <summary>
        /// The list of GameMaker bytecode instructions.
        /// </summary>
        public List<Instruction> Instructions = new(64);

        /// <summary>
        /// Initializes a new <see cref="Bytecode"/> class with a specified parent.
        /// </summary>
        /// <param name="parent">Which code entry the bytecode should belong to.</param>
        public Bytecode(GMCode parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Returns length in terms of 32-bit parts of instructions.
        /// Multiply the return value by 4 to get the number of bytes.
        /// </summary>
        public int GetLength()
        {
            return Instructions.Sum(instruction => instruction.GetLength());
        }

        public void Serialize(GMDataWriter writer)
        {
            foreach (Instruction i in Instructions)
                i.Serialize(writer);
        }

        public void Deserialize(GMDataReader reader)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(GMDataReader reader, int length)
        {
            int begin = reader.Offset;
            int end = begin + length;
            while (reader.Offset < end)
            {
                Instruction i = new(reader.Offset - begin);
                i.Deserialize(reader);
                Instructions.Add(i);
            }
        }

        /// <summary>
        /// A single GameMaker bytecode instruction.
        /// </summary>
        public class Instruction : IGMSerializable
        {
            public enum VariableType : byte
            {
                Array,
                StackTop = 0x80,
                Normal = 0xA0,
                Instance = 0xE0, // used for room creation code?

                // GMS 2.3 types
                MultiPush = 0x10, // Multidimensional array, used with pushaf
                MultiPushPop = 0x90, // Multidimensional array, used with pushaf/popaf
            }

            public class Reference<T> : IGMSerializable
            {
                public int NextOccurrence;
                public VariableType Type;
                public T Target;

                public Reference()
                {
                }

                public Reference(T target)
                {
                    Target = target;
                }

                public Reference(T target, VariableType type)
                {
                    Target = target;
                    Type = type;
                }

                public Reference(int int32Value)
                {
                    NextOccurrence = int32Value & 0x07FFFFFF;
                    Type = (VariableType)((int32Value >> 24) & 0xF8);
                }

                public void Serialize(GMDataWriter writer)
                {
                    writer.WriteInt24(0);
                    writer.Write((byte)Type);
                }

                public void Deserialize(GMDataReader reader)
                {
                    int int32Value = reader.ReadInt32();
                    NextOccurrence = int32Value & 0x07FFFFFF;
                    Type = (VariableType)((int32Value >> 24) & 0xF8);
                }

                public override string ToString()
                {
                    return $"Reference to {Target}";
                }
            }

            /// <summary>
            /// Returns the number of 32-bit parts of the instruction.
            /// Multiply the return value by 4 to get the number of bytes.
            /// </summary>
            public int GetLength()
            {
                if (Variable != null || Function != null)
                    return 2;

                if (GetInstructionType(Kind) == InstructionType.Push)
                {
                    if (Type1 == DataType.Double || Type1 == DataType.Int64)
                        return 3;
                    if (Type1 != DataType.Int16)
                        return 2;
                }

                return 1;
            }

            public enum Opcode : byte
            {
                Conv = 0x07,
                Mul = 0x08,
                Div = 0x09,
                Rem = 0x0A,
                Mod = 0x0B,
                Add = 0x0C,
                Sub = 0x0D,
                And = 0x0E,
                Or = 0x0F,
                Xor = 0x10,
                Neg = 0x11,
                Not = 0x12,
                Shl = 0x13,
                Shr = 0x14,
                Cmp = 0x15,
                Pop = 0x45,
                Dup = 0x86,
                Ret = 0x9C,
                Exit = 0x9D,
                Popz = 0x9E,
                B = 0xB6,
                Bt = 0xB7,
                Bf = 0xB8,
                PushEnv = 0xBA,
                PopEnv = 0xBB,
                Push = 0xC0,
                PushLoc = 0xC1,
                PushGlb = 0xC2,
                PushBltn = 0xC3,
                PushI = 0x84,
                Call = 0xD9,
                CallV = 0x99,
                Break = 0xFF
            }

            public enum InstructionType
            {
                SingleType,
                DoubleType,
                Comparison,
                Branch,
                Push,
                Pop,
                Call,
                Break
            }

            public enum ComparisonType : byte
            {
                LT = 1,
                LTE = 2,
                EQ = 3,
                NEQ = 4,
                GTE = 5,
                GT = 6
            }

            public enum DataType : byte
            {
                Double,
                Float,
                Int32,
                Int64,
                Boolean,
                Variable,
                String,
                Instance, // these next four types seem unused, maybe?
                Delete,
                Undefined,
                UnsignedInt,
                Int16 = 0x0f,

                Unset = 0xFF
            }

            public enum BreakType : ushort
            {
                chkindex = 65535,
                pushaf = 65534,
                popaf = 65533,
                pushac = 65532,
                setowner = 65531,
                isstaticok = 65530,
                setstatic = 65529,
                savearef = 65528,
                restorearef = 65527,
                isnullish = 65526
            }

            public static int GetDataTypeStackLength(DataType type)
            {
                return type switch
                {
                    // todo? strings, instances?
                    DataType.Int16 or DataType.Int32 or DataType.Float => 4,
                    DataType.Int64 or DataType.Double => 8,
                    DataType.Variable => 16,
                    _ => 16,
                };
            }

            public enum InstanceType : short
            {
                // >= 0 are object indices, usually

                Undefined = 0,
                Self = -1,
                Other = -2,
                All = -3,
                Noone = -4,
                Global = -5,
                Builtin = -6, // Not used in bytecode itself, probably?
                Local = -7,
                StackTop = -9,
                Argument = -15,
                Static = -16
            }

            private static byte OldOpcodeToNew(byte opcode)
            {
                if (opcode <= 0x10 || opcode == 0x41 || opcode == 0x82)
                    return (byte)(opcode + 0x04);
                if (opcode <= 0x16)
                    return 0x15;
                if (opcode == 0xC0 || opcode == 0xFF)
                    return opcode;
                return (byte)(opcode - 0x01);
            }

            private static byte NewOpcodeToOld(byte opcode, byte comparison)
            {
                if (opcode <= 0x14 || opcode == 0x45 || opcode == 0x86)
                    return (byte)(opcode - 0x04);
                if (opcode == 0x15)
                    return (byte)(0x10 + comparison);
                if (opcode == 0x84)
                    return 0xC0;
                if (opcode == 0xC0 || opcode == 0xFF)
                    return opcode;
                if (opcode >= 0xC1 && opcode <= 0xC3)
                    return 0xC0;
                return (byte)(opcode + 0x01);
            }

            public static InstructionType GetInstructionType(Opcode op)
            {
                switch (op)
                {
                    case Opcode.Neg:
                    case Opcode.Not:
                    case Opcode.Dup:
                    case Opcode.Ret:
                    case Opcode.Exit:
                    case Opcode.Popz:
                    case Opcode.CallV:
                        return InstructionType.SingleType;

                    case Opcode.Conv:
                    case Opcode.Mul:
                    case Opcode.Div:
                    case Opcode.Rem:
                    case Opcode.Mod:
                    case Opcode.Add:
                    case Opcode.Sub:
                    case Opcode.And:
                    case Opcode.Or:
                    case Opcode.Xor:
                    case Opcode.Shl:
                    case Opcode.Shr:
                        return InstructionType.DoubleType;

                    case Opcode.Cmp:
                        return InstructionType.Comparison;

                    case Opcode.B:
                    case Opcode.Bt:
                    case Opcode.Bf:
                    case Opcode.PushEnv:
                    case Opcode.PopEnv:
                        return InstructionType.Branch;

                    case Opcode.Pop:
                        return InstructionType.Pop;

                    case Opcode.Push:
                    case Opcode.PushLoc:
                    case Opcode.PushGlb:
                    case Opcode.PushBltn:
                    case Opcode.PushI:
                        return InstructionType.Push;

                    case Opcode.Call:
                        return InstructionType.Call;

                    case Opcode.Break:
                        return InstructionType.Break;

                    default:
                        throw new Exception("Unknown opcode " + op.ToString());
                }
            }

            public int Address;
            public Opcode Kind;
            public ComparisonType ComparisonKind;
            public DataType Type1;
            public DataType Type2;
            public InstanceType TypeInst;
            public Reference<GMVariable> Variable;
            public Reference<GMFunctionEntry> Function;
            public object Value { get; set; }
            public int JumpOffset;
            public bool PopenvExitMagic;
            public byte Extra;

            public Instruction(int address)
            {
                Address = address;
            }

            public void Serialize(GMDataWriter writer)
            {
                if (Variable != null)
                {
                    if (Variable.Target != null)
                    {
                        List<(int, VariableType)> l;
                        if (writer.VariableReferences.TryGetValue(Variable.Target, out l))
                            l.Add((writer.Offset, Variable.Type));
                        else
                            writer.VariableReferences.Add(Variable.Target, new List<(int, VariableType)> { (writer.Offset, Variable.Type) });
                    }
                    else
                        writer.Warnings.Add(new GMWarning($"Missing variable target at {writer.Offset}"));
                }
                else if (Function != null)
                {
                    if (Function.Target != null)
                    {
                        List<(int, VariableType)> l;
                        if (writer.FunctionReferences.TryGetValue(Function.Target, out l))
                            l.Add((writer.Offset, Function.Type));
                        else
                            writer.FunctionReferences.Add(Function.Target, new List<(int, VariableType)> { (writer.Offset, Function.Type) });
                    }
                    else
                        writer.Warnings.Add(new GMWarning($"Missing function target at {writer.Offset}"));
                }

                switch (GetInstructionType(Kind))
                {
                    case InstructionType.SingleType:
                    case InstructionType.DoubleType:
                    case InstructionType.Comparison:
                        {
                            writer.Write(Extra);
                            if (writer.VersionInfo.FormatID <= 14 && Kind == Opcode.Cmp)
                                writer.Write((byte)0);
                            else
                                writer.Write((byte)ComparisonKind);
                            writer.Write((byte)((byte)Type2 << 4 | (byte)Type1));
                            if (writer.VersionInfo.FormatID <= 14)
                                writer.Write(NewOpcodeToOld((byte)Kind, (byte)ComparisonKind));
                            else
                                writer.Write((byte)Kind);
                        }
                        break;
                    case InstructionType.Branch:
                        {
                            if (writer.VersionInfo.FormatID <= 14)
                                writer.WriteInt24(JumpOffset);
                            else if (PopenvExitMagic)
                                writer.WriteInt24(0xF00000);
                            else
                                writer.WriteInt24((int)((uint)JumpOffset & ~0xFF800000));

                            if (writer.VersionInfo.FormatID <= 14)
                                writer.Write(NewOpcodeToOld((byte)Kind, 0));
                            else
                                writer.Write((byte)Kind);
                        }
                        break;
                    case InstructionType.Pop:
                        {
                            if (Type1 == DataType.Int16)
                            {
                                writer.Write((short)TypeInst);
                                writer.Write((byte)((byte)Type2 << 4 | (byte)Type1));
                                if (writer.VersionInfo.FormatID <= 14)
                                    writer.Write(NewOpcodeToOld((byte)Kind, 0));
                                else
                                    writer.Write((byte)Kind);
                            }
                            else
                            {
                                writer.Write((short)TypeInst);
                                writer.Write((byte)((byte)Type2 << 4 | (byte)Type1));
                                if (writer.VersionInfo.FormatID <= 14)
                                    writer.Write(NewOpcodeToOld((byte)Kind, 0));
                                else
                                    writer.Write((byte)Kind);
                                Variable.Serialize(writer);
                            }
                        }
                        break;
                    case InstructionType.Push:
                        {
                            if (Type1 == DataType.Int16)
                                writer.Write((short)Value);
                            else if (Type1 == DataType.Variable)
                                writer.Write((short)TypeInst);
                            else
                                writer.Write((short)0);
                            writer.Write((byte)Type1);

                            if (writer.VersionInfo.FormatID <= 14)
                                writer.Write(NewOpcodeToOld((byte)Kind, 0));
                            else
                                writer.Write((byte)Kind);

                            switch (Type1)
                            {
                                case DataType.Double:
                                    writer.Write((double)Value);
                                    break;
                                case DataType.Float:
                                    writer.Write((float)Value);
                                    break;
                                case DataType.Int32:
                                    if (Function != null)
                                    {
                                        Function.Serialize(writer);
                                        break;
                                    }
                                    writer.Write((int)Value);
                                    break;
                                case DataType.Int64:
                                    writer.Write((long)Value);
                                    break;
                                case DataType.Boolean:
                                    writer.WriteWideBoolean((bool)Value);
                                    break;
                                case DataType.Variable:
                                    Variable.Serialize(writer);
                                    break;
                                case DataType.String:
                                    writer.Write((int)Value); // string ID
                                    break;
                                //case DataType.Int16:
                                //    break;
                            }
                        }
                        break;
                    case InstructionType.Call:
                        {
                            writer.Write((short)Value);
                            writer.Write((byte)Type1);
                            if (writer.VersionInfo.FormatID <= 14)
                                writer.Write(NewOpcodeToOld((byte)Kind, 0));
                            else
                                writer.Write((byte)Kind);
                            Function.Serialize(writer);
                        }
                        break;
                    case InstructionType.Break:
                        {
                            writer.Write((ushort)Value);
                            writer.Write((byte)Type1);
                            if (writer.VersionInfo.FormatID <= 14)
                                writer.Write(NewOpcodeToOld((byte)Kind, 0));
                            else
                                writer.Write((byte)Kind);
                        }
                        break;
                    default:
                        throw new Exception("Unknown opcode " + Kind.ToString());
                }
            }

            public void Deserialize(GMDataReader reader)
            {
                int start = reader.Offset;
                reader.Instructions[start] = this;

#if DEBUG
                if (start % 4 != 0)
                    throw new Exception("Instruction reading offset");
#endif

                // Read opcode
                reader.Offset += 3;
                byte opcode = reader.ReadByte();
                if (reader.VersionInfo.FormatID <= 14)
                    opcode = OldOpcodeToNew(opcode);
                Kind = (Opcode)opcode;
                reader.Offset = start;

                switch (GetInstructionType(Kind))
                {
                    case InstructionType.SingleType:
                    case InstructionType.DoubleType:
                    case InstructionType.Comparison:
                        {
                            Extra = reader.ReadByte();
    #if DEBUG
                            if (Extra != 0 && Kind != Opcode.Dup && Kind != Opcode.CallV)
                                throw new Exception("Expected 0 byte for opcode " + Kind.ToString());
    #endif
                            ComparisonKind = (ComparisonType)reader.ReadByte();

                            byte types = reader.ReadByte();
                            Type1 = (DataType)(types & 0xf);
                            Type2 = (DataType)(types >> 4);
                            if (Kind == Opcode.Cmp && reader.VersionInfo.FormatID <= 14)
                                ComparisonKind = (ComparisonType)(reader.ReadByte() - 0x10);
                            else
                                reader.Offset += 1;

                            if (Kind == Opcode.And || Kind == Opcode.Or)
                            {
                                if (Type1 == DataType.Boolean && Type2 == DataType.Boolean)
                                    reader.VersionInfo.ShortCircuit = false;
                            }
                        }
                        break;
                    case InstructionType.Branch:
                        {
                            if (reader.VersionInfo.FormatID <= 14)
                            {
                                JumpOffset = reader.ReadInt24();
                                if (JumpOffset == -1048576)
                                    PopenvExitMagic = true;
                            }
                            else
                            {
                                uint v = reader.ReadUInt24();

                                PopenvExitMagic = (v & 0x800000) != 0;

                                // The rest is int23 signed value, so make sure
                                uint r = v & 0x003FFFFF;
                                if ((v & 0x00C00000) != 0)
                                    r |= 0xFFC00000;
                                JumpOffset = (int)r;
                            }

                            reader.Offset += 1;
                        }
                        break;
                    case InstructionType.Pop:
                        {
                            TypeInst = (InstanceType)reader.ReadInt16();

                            byte types = reader.ReadByte();
                            Type1 = (DataType)(types & 0xf);
                            Type2 = (DataType)(types >> 4);

                            reader.Offset += 1;

                            if (Type1 != DataType.Int16) // ignore swap instructions
                            {
                                Variable = new Reference<GMVariable>();
                                Variable.Deserialize(reader);
                            }
                        }
                        break;
                    case InstructionType.Push:
                        {
                            short val = reader.ReadInt16();

                            Type1 = (DataType)reader.ReadByte();
                            if (reader.VersionInfo.FormatID <= 14)
                            {
                                // Convert to new opcodes
                                if (Type1 == DataType.Variable)
                                {
                                    switch (val)
                                    {
                                        case -5:
                                            Kind = Opcode.PushGlb;
                                            break;
                                        case -6:
                                            Kind = Opcode.PushBltn;
                                            break;
                                        case -7:
                                            Kind = Opcode.PushLoc;
                                            break;
                                    }
                                }
                                else if (Type1 == DataType.Int16)
                                    Kind = Opcode.PushI;
                            }

                            reader.Offset += 1;

                            switch (Type1)
                            {
                                case DataType.Double:
                                    Value = reader.ReadDouble();
                                    break;
                                case DataType.Float:
                                    Value = reader.ReadSingle();
                                    break;
                                case DataType.Int32:
                                    Value = reader.ReadInt32();
                                    break;
                                case DataType.Int64:
                                    Value = reader.ReadInt64();
                                    break;
                                case DataType.Boolean:
                                    Value = reader.ReadWideBoolean();
                                    break;
                                case DataType.Variable:
                                    TypeInst = (InstanceType)val;
                                    Variable = new Reference<GMVariable>();
                                    Variable.Deserialize(reader);
                                    break;
                                case DataType.String:
                                    Value = reader.ReadInt32(); // string ID
                                    break;
                                case DataType.Int16:
                                    Value = val;
                                    break;
                            }
                        }
                        break;
                    case InstructionType.Call:
                        {
                            Value = reader.ReadInt16();
                            Type1 = (DataType)reader.ReadByte();

                            reader.Offset += 1;

                            Function = new Reference<GMFunctionEntry>();
                            Function.Deserialize(reader);
                        }
                        break;
                    case InstructionType.Break:
                        {
                            Value = reader.ReadUInt16();
                            Type1 = (DataType)reader.ReadByte();

                            reader.Offset += 1;
                        }
                        break;
                    default:
                        throw new Exception("Unknown opcode " + Kind.ToString());
                }
            }

            public override string ToString()
            {
                return $"Instruction: \"{Kind}\"";
            }
        }
    }
}