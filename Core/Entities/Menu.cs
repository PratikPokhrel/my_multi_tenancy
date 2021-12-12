using System;
using Core.Entities.Audit;

namespace Core.Entities
{
    public class Menu:FullAudited<int>
    {
        public string Name{get;set;}
        public int Order { get; set; }
        public bool IsOldLink { get; set; }
    }
}