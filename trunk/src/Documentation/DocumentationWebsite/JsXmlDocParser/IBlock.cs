using System;
using System.Collections.Generic;
using System.Linq;

namespace JsXmlDocParser
{
	public interface IBlock
	{
		IBlockStarter BlockStarter { get; }
        string Name { get; }
		List<IBlock> Children { get; }
		List<string> Comments { get; }
	}

    public static class IBlockExtensions
    {
        public static void RemoveChildren(this List<IBlock> blocks, Predicate<IBlock> predicate, bool recursive = true)
        {
            blocks.RemoveAll(predicate);

            if (recursive)
            {
                foreach (var child in blocks.Select(x => x.Children))
                {
                    RemoveChildren(child, predicate);
                }
            }
        }
    }
}