namespace BundlerMiddleware
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class Replacer
    {
        private List<ReplacementPair> matchers = new List<ReplacementPair>();

        public void AddMatcher(Regex matcher, Func<Match, string> replacer)
        {
            matchers.Add(new ReplacementPair { Regex = matcher, Replacement = replacer });
        }

        public async Task<string> MatchReplacer(string path)
        {
            using (var stream = File.OpenText(path))
            {
                string line;
                var sb = new StringBuilder();

                while ((line = stream.ReadLine()) != null)
                {
                    foreach (var matcher in this.matchers)
                    {
                        line = matcher.Regex.Replace(line, new MatchEvaluator(matcher.Replacement));
                    }

                    sb.AppendLine(line);
                }

                return sb.ToString();
            }
        }
    }

    public class BundleMatcher
    {
        private readonly IBundlerResolver bundleResolver;
        public BundleMatcher(IBundlerResolver bundleResolver)
        {
            this.bundleResolver = bundleResolver;
        }
        
        public readonly Regex Matcher = new Regex(@"\!\!(scripts|styles):([^\}]+?)\!\!", RegexOptions.Compiled);

        public string BundleMatchReplace(Match match)
        {
            return match.Groups[1].Value == "scripts"
                       ? this.bundleResolver.GetScriptTags(match.Groups[2].Value)
                       : this.bundleResolver.GetStyleTags(match.Groups[2].Value);
        }
    }

    public class Matchers
    {
        public static readonly Regex ContentMatcher = new Regex(@"\!\!block content\!\!", RegexOptions.Compiled);
    }

    public class ReplacementPair
    {
        public Regex Regex { get; set; }
        public Func<Match, string> Replacement { get; set; }
    }
}
