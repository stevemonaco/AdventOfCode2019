using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day7
{
    public class IntCodeComputer
    {
        private enum AddressingMode { Position = 0, Immediate = 1 }
        private enum Opcode { Add = 1, Multiply = 2, Input = 3, Output = 4,
            JumpIfTrue = 5, JumpIfFalse = 6, LessThan = 7, Equals = 8, Halt = 99 }

        private static Dictionary<Opcode, int> _opcodeParameters = new Dictionary<Opcode, int>()
        {
            { Opcode.Add, 3 }, { Opcode.Multiply, 3 }, { Opcode.Input, 1 }, { Opcode.Output, 1 },
            { Opcode.JumpIfTrue, 2 }, { Opcode.JumpIfFalse, 2 }, { Opcode.LessThan, 3 }, { Opcode.Equals, 3 },
            { Opcode.Halt, 0 }
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
            public int Value { get; }
            public AddressingMode Mode { get; }

            public OpcodeParameter(int value, AddressingMode mode)
            {
                Value = value;
                Mode = mode;
            }
        }

        public int[] Program { get; set; }
        public int IP { get; set; }
        public IInputProvider InputProvider { get; set; }
        public List<IOutputSink> OutputSinks { get; } = new List<IOutputSink>();
        public QueueOutputSink ResultSink { get; } = new QueueOutputSink();
        public string ComputerName { get; set; }

        public IntCodeComputer() { }

        public IntCodeComputer(IInputProvider inputProvider)
        {
            InputProvider = inputProvider;
        }

        public IntCodeComputer(ComputerState state)
        {
            Program = state.Program.Clone() as int[];
            IP = state.IP;
            InputProvider = state.InputProvider;
        }

        public void LoadProgramFromFile(string fileName) =>
            Program = File.ReadAllText(fileName).Split(",").Select(x => int.Parse(x)).ToArray();

        public void LoadProgramFromString(string program) =>
            Program = CreateProgramFromString(program);

        public static int[] CreateProgramFromString(string program) =>
            program.Split(",").Select(x => int.Parse(x)).ToArray();

        public void ExecuteProgram()
        {
            while (IP < Program.Length)
            {
                var instruction = ParseInstruction();

                if (instruction.Opcode == Opcode.Halt)
                {
                    IP++;
                    break;
                }

                ExecuteInstruction(instruction);
            }
        }

        public void AddOutputSink(IOutputSink sink) => OutputSinks.Add(sink);

        /*public ComputerState SaveState()
        {
            var state = new ComputerState();
            state.IP = IP;
            state.Program = Program.Clone() as int[];
            state.InputProvider = new StringInputProvider(InputProvider.InputQueue);
            state.OutputSink = new QueueOutputSink();
            state.OutputSink.OutputQueue = new Queue<int>(OutputSinks.OutputQueue);
            return state;
        }*/

        private Instruction ParseInstruction()
        {
            var opcodeValue = Program[IP] % 100;
            var modeA = (Program[IP] / 100) % 10;
            var modeB = (Program[IP] / 1000) % 10;
            var modeC = (Program[IP] / 10000) % 10;

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
            int operand1;
            int operand2;

            switch(inst.Opcode)
            {
                case Opcode.Add:
                    operand1 = ReadValue(inst.A);
                    operand2 = ReadValue(inst.B);
                    Program[inst.C.Value] = operand1 + operand2;
                    IP += inst.Length;
                    break;
                case Opcode.Multiply:
                    operand1 = ReadValue(inst.A);
                    operand2 = ReadValue(inst.B);
                    Program[inst.C.Value] = operand1 * operand2;
                    IP += inst.Length;
                    break;
                case Opcode.Input:
                    operand1 = InputProvider.GetInput();
                    Program[inst.A.Value] = operand1;
                    IP += inst.Length;
                    break;
                case Opcode.Output:
                    operand1 = ReadValue(inst.A);
                    foreach(var sink in OutputSinks)
                        sink.SendOutput(operand1);
                    ResultSink.SendOutput(operand1);
                    IP += inst.Length;
                    break;
                case Opcode.JumpIfTrue:
                    operand1 = ReadValue(inst.A);
                    if (operand1 != 0)
                        IP = ReadValue(inst.B);
                    else
                        IP += inst.Length;
                    break;
                case Opcode.JumpIfFalse:
                    operand1 = ReadValue(inst.A);
                    if (operand1 == 0)
                        IP = ReadValue(inst.B);
                    else
                        IP += inst.Length;
                    break;
                case Opcode.LessThan:
                    operand1 = ReadValue(inst.A);
                    operand2 = ReadValue(inst.B);

                    if (operand1 < operand2)
                        Program[inst.C.Value] = 1;
                    else
                        Program[inst.C.Value] = 0;
                    IP += inst.Length;
                    break;
                case Opcode.Equals:
                    operand1 = ReadValue(inst.A);
                    operand2 = ReadValue(inst.B);

                    if (operand1 == operand2)
                        Program[inst.C.Value] = 1;
                    else
                        Program[inst.C.Value] = 0;
                    IP += inst.Length;
                    break;
            }
        }

        private int ReadValue(OpcodeParameter readParameter)
        {
            if (readParameter.Mode == AddressingMode.Immediate)
            {
                return readParameter.Value;
            }
            else if (readParameter.Mode == AddressingMode.Position)
            {
                return Program[readParameter.Value];
            }

            throw new NotSupportedException();
        }
    }
}
