
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
        public static readonly Regex ContentMatcher = new Regex(@"\!\!block content\!\!", RegexOptions.Compiled);
        private static readonly Regex bundleMatcher = new Regex(@"\!\!(scripts|styles):([^\}]+?)\!\!", RegexOptions.Compiled);

        private List<ReplacementPair> matchers = new List<ReplacementPair>() ;

        private IBundlerResolver bundleResolver;

        public Replacer(IBundlerResolver bundleResolver)
        {
            this.matchers.Add(new ReplacementPair{ Regex = bundleMatcher, Replacement = this.MatchReplace});
            this.bundleResolver = bundleResolver;
        }

        public void AddMatcher(Regex matcher, Func<Match, string> replacer)
        {
            matchers.Add(new ReplacementPair {Regex = matcher, Replacement = replacer});
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

        private string MatchReplace(Match match)
        {
            return match.Groups[1].Value == "scripts"
                       ? this.bundleResolver.GetScriptTags(match.Groups[2].Value)
                       : this.bundleResolver.GetStyleTags(match.Groups[2].Value);
        }


    }

    public class ReplacementPair
    {
        public Regex Regex { get; set; }
        public Func<Match, string> Replacement { get; set; }
    }
}
