// AI generated: List all public types in Azure.AI.OpenAI and OpenAI namespaces
using System;
using System.Linq;
using System.Reflection;

class Program {
    static void Main() {


        var openaiNetAssembly = typeof(OpenAI.OpenAIClient).Assembly;
        Console.WriteLine("\nTypes in OpenAI:");
        foreach (var t in openaiNetAssembly.GetTypes().Where(t => t.IsPublic && t.Namespace == "OpenAI"))
            Console.WriteLine(t.FullName);
    }
}
