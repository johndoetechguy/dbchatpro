// AI generated: List all public types in Azure.AI.OpenAI and OpenAI namespaces
using System;
using System.Linq;
using System.Reflection;

class Program {
    static void Main() {
        var openaiAssembly = typeof(Azure.AI.OpenAI.OpenAIClient).Assembly;
        Console.WriteLine("Types in Azure.AI.OpenAI:");
        foreach (var t in openaiAssembly.GetTypes().Where(t => t.IsPublic && t.Namespace == "Azure.AI.OpenAI"))
            Console.WriteLine(t.FullName);

        var openaiNetAssembly = typeof(OpenAI.OpenAIClient).Assembly;
        Console.WriteLine("\nTypes in OpenAI:");
        foreach (var t in openaiNetAssembly.GetTypes().Where(t => t.IsPublic && t.Namespace == "OpenAI"))
            Console.WriteLine(t.FullName);
    }
}
