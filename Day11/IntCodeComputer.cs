using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Day11
{
    public class IntCodeComputer
    {
        private enum AddressingMode { Position = 0, Immediate = 1, Relative = 2 }
        private enum Opcode { Add = 1, Multiply = 2, Input = 3, Output = 4,
            JumpIfTrue = 5, JumpIfFalse = 6, LessThan = 7, Equals = 8, AddRelativeOffset = 9,
            Halt = 99 }

        private static Dictionary<Opcode, int> _opcodeParameters = new Dictionary<Opcode, int>()
        {
            { Opcode.Add, 3 }, { Opcode.Multiply, 3 }, { Opcode.Input, 1 }, { Opcode.Output, 1 },
            { Opcode.JumpIfTrue, 2 }, { Opcode.JumpIfFalse, 2 }, { Opcode.LessThan, 3 }, { Opcode.Equals, 3 },
            { Opcode.AddRelativeOffset, 1 }, { Opcode.Halt, 0 }
        };

        struct Instruction
        {
            public Opcode Opcode { get; }
            public OpcodeParameter A { get; set; }
            public OpcodeParameter B { get; set; }
            public OpcodeParameter C { get; set; }
            public int Length { get; set; }

            public Instruction(Opcode opcode)
            {
                Opcode = opcode;
                A = default;
                B = default;
                C = default;
                Length = default;
            }
        }

        struct OpcodeParameter
        {
            public BigInteger Value { get; }
            public AddressingMode Mode { get; }

            public OpcodeParameter(BigInteger value, AddressingMode mode)
            {
                Value = value;
                Mode = mode;
            }
        }

        public List<BigInteger> Program { get; set; }
        public int IP { get; set; }
        public bool IsHalted { get; private set; } = true;
        public int RelativeBase { get; set; }
        public IInputProvider InputProvider { get; set; }
        public List<IOutputSink> OutputSinks { get; } = new List<IOutputSink>();
        public QueueOutputSink ResultSink { get; } = new QueueOutputSink();
        public string ComputerName { get; set; }

        public IntCodeComputer() { }

        public IntCodeComputer(IInputProvider inputProvider)
        {
            InputProvider = inputProvider;
        }

        public void LoadProgramFromFile(string fileName) =>
            Program = File.ReadAllText(fileName).Split(",").Select(x => BigInteger.Parse(x)).ToList();

        public void LoadProgramFromString(string program) =>
            Program = CreateProgramFromString(program);

        public static List<BigInteger> CreateProgramFromString(string program) =>
            program.Split(",").Select(x => BigInteger.Parse(x)).ToList();

        public void ExecuteProgram()
        {
            IsHalted = false;
            while (IP < Program.Count && !IsHalted)
            {
                var instruction = ParseInstruction();
                ExecuteInstruction(instruction);
            }
        }

        public void AddOutputSink(IOutputSink sink) => OutputSinks.Add(sink);

        private Instruction ParseInstruction()
        {
            var opcodeValue = (int)(Program[IP] % 100);
            var modeA = (int)(Program[IP] / 100) % 10;
            var modeB = (int)(Program[IP] / 1000) % 10;
            var modeC = (int)(Program[IP] / 10000) % 10;

            if (!Enum.IsDefined(typeof(Opcode), opcodeValue))
                throw new NotSupportedException($"Opcode {opcodeValue} is not supported");

            var instruction = new Instruction((Opcode)opcodeValue);
            var paramCount = _opcodeParameters[instruction.Opcode];
            instruction.Length = paramCount + 1;

            if(paramCount == 1)
            {
                instruction.A = new OpcodeParameter(Program[IP + 1], (AddressingMode)modeA);
            }
            else if(paramCount == 2)
            {
                instruction.A = new OpcodeParameter(Program[IP + 1], (AddressingMode)modeA);
                instruction.B = new OpcodeParameter(Program[IP + 2], (AddressingMode)modeB);
            }
            else if(paramCount == 3)
            {
                instruction.A = new OpcodeParameter(Program[IP + 1], (AddressingMode)modeA);
                instruction.B = new OpcodeParameter(Program[IP + 2], (AddressingMode)modeB);
                instruction.C = new OpcodeParameter(Program[IP + 3], (AddressingMode)modeC);
            }

            return instruction;
        }

        private void ExecuteInstruction(Instruction inst)
        {
            BigInteger operand1;
            BigInteger operand2;

            switch(inst.Opcode)
            {
                case Opcode.Add:
                    operand1 = ReadValue(inst.A);
                    operand2 = ReadValue(inst.B);
                    WriteValue(operand1 + operand2, inst.C);
                    break;
                case Opcode.Multiply:
                    operand1 = ReadValue(inst.A);
                    operand2 = ReadValue(inst.B);
                    WriteValue(operand1 * operand2, inst.C);
                    break;
                case Opcode.Input:
                    operand1 = InputProvider.GetInput();
                    WriteValue(operand1, inst.A);
                    break;
                case Opcode.Output:
                    operand1 = ReadValue(inst.A);
                    foreach(var sink in OutputSinks)
                        sink.SendOutput(operand1);
                    ResultSink.SendOutput(operand1);
                    break;
                case Opcode.JumpIfTrue:
                    operand1 = ReadValue(inst.A);
                    if (operand1 != 0)
                        IP = (int)ReadValue(inst.B) - inst.Length;
                    break;
                case Opcode.JumpIfFalse:
                    operand1 = ReadValue(inst.A);
                    if (operand1 == 0)
                        IP = (int)ReadValue(inst.B) - inst.Length;
                    break;
                case Opcode.LessThan:
                    operand1 = ReadValue(inst.A);
                    operand2 = ReadValue(inst.B);

                    if (operand1 < operand2)
                        WriteValue(1, inst.C);
                    else
                        WriteValue(0, inst.C);
                    break;
                case Opcode.Equals:
                    operand1 = ReadValue(inst.A);
                    operand2 = ReadValue(inst.B);

                    if (operand1 == operand2)
                        WriteValue(1, inst.C);
                    else
                        WriteValue(0, inst.C);
                    break;
                case Opcode.AddRelativeOffset:
                    operand1 = ReadValue(inst.A);
                    RelativeBase += (int)operand1;
                    break;
                case Opcode.Halt:
                    IsHalted = true;
                    break;
            }

            IP += inst.Length;
        }

        private BigInteger ReadValue(OpcodeParameter readParameter)
        {
            if (readParameter.Mode == AddressingMode.Immediate)
            {
                return readParameter.Value;
            }
            else if (readParameter.Mode == AddressingMode.Position)
            {
                EnsureMemoryCapacity((int)readParameter.Value);
                return Program[(int)readParameter.Value];
            }
            else if(readParameter.Mode == AddressingMode.Relative)
            {
                EnsureMemoryCapacity((int)readParameter.Value + RelativeBase);
                return Program[(int)readParameter.Value + RelativeBase];
            }

            throw new NotSupportedException();
        }

        private void WriteValue(BigInteger value, OpcodeParameter writeParameter)
        {
            if (writeParameter.Mode == AddressingMode.Position)
            {
                EnsureMemoryCapacity((int)writeParameter.Value);
                Program[(int)writeParameter.Value] = value;
            }
            else if (writeParameter.Mode == AddressingMode.Relative)
            {
                EnsureMemoryCapacity((int)writeParameter.Value + RelativeBase);
                Program[(int)writeParameter.Value + RelativeBase] = value;
            }
        }

        private void EnsureMemoryCapacity(int maxAddress)
        {
            if(maxAddress >= Program.Count)
                Program.AddRange(Enumerable.Repeat((BigInteger)0, maxAddress - Program.Count + 1));
        }
    }
}
