using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace VSDocPreprocessor.MSBuild
{
    public class ParseJsDocs : Task
    {
        public ITaskItem[] Assemblies { get; set; }

        public ITaskItem[] JavaScriptFiles { get; set; }

        public ITaskItem OutDir { get; set; }

        public override bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}