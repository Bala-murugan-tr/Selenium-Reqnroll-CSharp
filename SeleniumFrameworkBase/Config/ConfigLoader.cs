namespace SeleniumFrameworkBase.Config;

internal static class ConfigLoader {
    public static Dictionary<string, string> LoadProps(string path) {
        if (!File.Exists(path))
            throw new FileNotFoundException($"ERROR MSG : {Path.GetFileName(path)} not found at: {path}");

        return File.ReadLines(path)
            .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
            .Select(line => line.Split('=', 2))
            .Where(parts => parts.Length == 2)
            .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());
    }
}
