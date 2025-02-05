﻿global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using AdventOfCode.Solutions.Utils;

using System.Diagnostics;
using System.Net;

namespace AdventOfCode.Solutions;

public abstract class SolutionBase
{
    public int Day { get; }
    public int Year { get; }
    public string Title { get; }
    public bool Debug { get; set; }
    public string Input => LoadInput(Debug);
    public string DebugInput => LoadInput(true);

    public SolutionResult Part1 => Solve(1);
    public SolutionResult Part2 => Solve(2);

    private protected SolutionBase(int day, int year, string title, bool useDebugInput = false)
    {
        Day = day;
        Year = year;
        Title = title;
        Debug = useDebugInput;
    }

    public IEnumerable<SolutionResult> SolveAll()
    {
        yield return Solve(SolvePartOne);
        yield return Solve(SolvePartTwo);
    }

    public SolutionResult Solve(int part = 1)
    {
        if (part == 1) return Solve(SolvePartOne);
        if (part == 2) return Solve(SolvePartTwo);

        throw new InvalidOperationException("Invalid part param supplied.");
    }

    SolutionResult Solve(Func<string> SolverFunction)
    {
        if (Debug && string.IsNullOrEmpty(DebugInput))
        {
            Console.WriteLine("Debug Input is null or empty");
            return SolutionResult.Empty;
        }
        else if (string.IsNullOrEmpty(Input))
        {
            Console.WriteLine("Input is null or empty");
            return SolutionResult.Empty;
        }

        try
        {
            var then = DateTime.Now;
            var result = SolverFunction();
            var now = DateTime.Now;
            return string.IsNullOrEmpty(result)
                ? SolutionResult.Empty
                : new SolutionResult { Answer = result, Time = now - then };
        }
        catch (Exception)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
                return SolutionResult.Empty;
            }
            else
            {
                throw;
            }
        }
    }

    string LoadInput(bool debug = false)
    {
        string INPUT_FILEPATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"../../../AdventOfCode.Solutions/Year{Year}/Day{Day:D2}/{(debug ? "debug" : "input")}"));

        if (File.Exists(INPUT_FILEPATH) && new FileInfo(INPUT_FILEPATH).Length > 0)
        {
            return File.ReadAllText(INPUT_FILEPATH);
        }

        try
        {
            var input = InputService.FetchInput(Year, Day).Result;
            File.WriteAllText(INPUT_FILEPATH, input);
            return input;
        }
        catch (AggregateException)
        {
            var colour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Day {Day}: Cannot fetch puzzle input before given date (Eastern Standard Time).");
            Console.ForegroundColor = colour;
        }
        catch (InvalidOperationException)
        {
            var colour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Day {Day}: Cannot fetch puzzle input before given date (Eastern Standard Time).");
            Console.ForegroundColor = colour;
        }
        catch (HttpRequestException e)
        {
            var code = e.StatusCode;
            var colour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (code == HttpStatusCode.BadRequest)
            {
                Console.WriteLine($"Day {Day}: Received 400 when attempting to retrieve puzzle input. Your session cookie is probably not recognized.");

            }
            else if (code == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Day {Day}: Received 404 when attempting to retrieve puzzle input. The puzzle is probably not available yet.");
            }
            else
            {
                Console.ForegroundColor = colour;
                Console.WriteLine(e.ToString());
            }
            Console.ForegroundColor = colour;
        }


        return "";
    }

    public override string ToString() =>
        $"\n--- Day {Day}: {Title} --- {(Debug ? "!! Debug mode active, using DebugInput !!" : "")}\n"
        + $"{ResultToString(1, Part1)}\n"
        + $"{ResultToString(2, Part2)}";

    string ResultToString(int part, SolutionResult result) =>
        $"  - Part{part} => " + (string.IsNullOrEmpty(result.Answer)
            ? "Unsolved"
            : $"{result.Answer} ({result.Time.TotalMilliseconds}ms)");

    protected abstract string SolvePartOne();
    protected abstract string SolvePartTwo();
}
