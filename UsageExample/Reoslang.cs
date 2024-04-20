using System;
using System.Collections.Generic;
using AdvancedConsole.Commands;
using AdvancedConsole.Commands.Attributes;
using AdvancedConsole.Reading;

namespace UsageExample;

public class Reoslang
{
    private Dictionary<string, object> memory = new();
    
    [Command("print")]
    public void Print(string text)
    {
        Console.Write(text.Replace("\\n", "\n"));
    }
    
    [Command("readString")]
    public void ReadString(string address = "res")
    {
        memory[address] = Console.ReadLine();
    }
    
    [Command("readInt")]
    public void ReadInt(string address = "res")
    {
        memory[address] = int.Parse(Console.ReadLine());
    }
    
    [Command("printMem")]
    public void PrintMem(string address)
    {
        Console.Write(memory[address]);
    }

    [Command("string")]
    [Alias("fun")]
    public void MemSetString(string key, string value) => memory[key] = value;
    
    [Command("int")]
    public void MemSetInt(string key, int val) => memory[key] = val;
    
    [Command("copy")]
    public void MemSetInt(string from, string to) => memory[to] = memory[from];

    [Command("call")]
    public void Call(string function)
    {
        CommandsListener subListener = new();
        subListener.AddModule(this);
        
        subListener.StartListening(new ConstantStringReader(memory[function] as string));
    }

    [Command("equal")]
    public void Equal(string address, string value)
    {
        memory["res"] = (string)memory[address] == value;
    }
    
    [Command("not")]
    public void Not(string address)
    {
        memory["res"] = !(bool)memory[address];
    }

    [Command("greater")]
    public void Greater(string left, string right) =>
        memory["res"] = (int)memory[left] >  (int)memory[right];
    
    [Command("sum")]
    public void Sum(string address1, string address2)
    {
        memory["res"] = (int)memory[address1] + (int)memory[address2];
    }
    
    [Command("if")]
    public void Call(string condition, string function)
    {
        CommandsListener subListener = new();
        subListener.AddModule(this);
        
        subListener.StartListening(new ConstantStringReader(memory[condition] as string));

        if ((bool)memory["res"])
        {
            subListener.StartListening(new ConstantStringReader(memory[function] as string));
        }
    }
}