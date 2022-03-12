using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Core.Enums
{
    public enum IssueType
    {
        Task=1,
        Bug,
        Story
    }

    public enum IssueStatus
    {
        Backlog = 1,
        Selected,
        InProgress,
        Done
    }

    public enum IssuePriority
    {
        Highest = 5,
        High = 4,
        Medium = 3,
        Low = 2,
        Lowest = 1
    }
}
