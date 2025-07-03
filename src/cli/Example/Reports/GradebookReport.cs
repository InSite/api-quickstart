using System;
using System.Linq;
using System.Text;

namespace Example;

public class GradebookReport
{
    internal static string Generate(int totalGradebookCount, string criterion, Gradebook[] gradebookMatches)
    {
        var output = new StringBuilder();

        var total = totalGradebookCount;

        output.AppendLine("## GRADEBOOK SUMMARY");
            
        output.AppendLine($"Your organization contains {total:n0} gradebooks.");

        var count = gradebookMatches.Count();

        var percent = total > 0 ? (decimal)count / total : 0m;

        output.AppendLine($"  {count} gradebooks ({percent:p2}) have a name that contains the term `{criterion}`:");

        foreach (var match in gradebookMatches)
        {
            output.AppendLine($"    - {match.GradebookTitle} ({match.GradebookEnrollmentCount} learners)");
        }
        
        return output.ToString();
    }
}
