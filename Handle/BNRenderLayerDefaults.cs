using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	public partial class RenderLayer
	{
		/// <summary>Transforms native disassembly block lines.</summary>
		public virtual DisassemblyTextLine[] ApplyToDisassemblyBlock(
			BasicBlock block,
			DisassemblyTextLine[] lines
		)
		{
			return lines;
		}

		/// <summary>Transforms low-level IL block lines.</summary>
		public virtual DisassemblyTextLine[] ApplyToLowLevelILBlock(
			BasicBlock block,
			DisassemblyTextLine[] lines
		)
		{
			return lines;
		}

		/// <summary>Transforms medium-level IL block lines.</summary>
		public virtual DisassemblyTextLine[] ApplyToMediumLevelILBlock(
			BasicBlock block,
			DisassemblyTextLine[] lines
		)
		{
			return lines;
		}

		/// <summary>Transforms high-level IL block lines.</summary>
		public virtual DisassemblyTextLine[] ApplyToHighLevelILBlock(
			BasicBlock block,
			DisassemblyTextLine[] lines
		)
		{
			return lines;
		}

		/// <summary>Transforms a high-level IL function body in linear view.</summary>
		public virtual LinearDisassemblyLine[] ApplyToHighLevelILBody(
			Function function,
			LinearDisassemblyLine[] lines
		)
		{
			return lines;
		}

		/// <summary>Transforms linear-view lines that do not represent code.</summary>
		public virtual LinearDisassemblyLine[] ApplyToMiscLinearLines(
			LinearViewObject linearView,
			LinearViewObject? previous,
			LinearViewObject? next,
			LinearDisassemblyLine[] lines
		)
		{
			return lines;
		}

		/// <summary>Dispatches block lines to the matching IL-specific method.</summary>
		public virtual DisassemblyTextLine[] ApplyToBlock(
			BasicBlock block,
			DisassemblyTextLine[] lines
		)
		{
			if (null == block)
			{
				throw new ArgumentNullException(nameof(block));
			}

			if (null == lines)
			{
				throw new ArgumentNullException(nameof(lines));
			}

			if (!block.IsIL)
			{
				return this.ApplyToDisassemblyBlock(block, lines);
			}

			if (block.IsLowLevelIL)
			{
				return this.ApplyToLowLevelILBlock(block, lines);
			}

			if (block.IsMediumLevelIL)
			{
				return this.ApplyToMediumLevelILBlock(block, lines);
			}

			if (block.IsHighLevelIL)
			{
				return this.ApplyToHighLevelILBlock(block, lines);
			}

			return lines;
		}

		private void ApplyToFlowGraphDefault(FlowGraph graph)
		{
			FlowGraphNode[] nodes = graph.Nodes;
			try
			{
				foreach (FlowGraphNode node in nodes)
				{
					BasicBlock? block = node.BasicBlock;
					DisassemblyTextLine[] inputLines = node.Lines;
					DisassemblyTextLine[] outputLines = inputLines;
					try
					{
						if (null != block)
						{
							outputLines = this.ApplyToBlock(block, inputLines);
						}

						node.Lines = outputLines;
					}
					finally
					{
						RenderLayer.DisposeDisassemblyLines(inputLines);
						if (null != block)
						{
							block.Dispose();
						}
					}
				}
			}
			finally
			{
				foreach (FlowGraphNode node in nodes)
				{
					node.Dispose();
				}
			}
		}

		private LinearDisassemblyLine[] ApplyToLinearViewObjectDefault(
			LinearViewObject linearView,
			LinearViewObject? previous,
			LinearViewObject? next,
			LinearDisassemblyLine[] lines
		)
		{
			string objectName = linearView.Identifier.Name;
			Function? function = 0 == lines.Length ? null : lines[0].Function;
			if (
				0 < lines.Length
				&& null != function
				&& RenderLayer.IsHighLevelBody(objectName)
			)
			{
				return this.ApplyToHighLevelILBody(function, lines);
			}

			List<LinearDisassemblyLine> result = new List<LinearDisassemblyLine>();
			int groupStart = 0;
			while (groupStart < lines.Length)
			{
				BasicBlock? block = lines[groupStart].Block;
				int groupEnd = groupStart + 1;
				while (
					groupEnd < lines.Length
					&& RenderLayer.SameBlock(block, lines[groupEnd].Block)
				)
				{
					groupEnd++;
				}

				this.ProcessLinearGroup(
					linearView,
					previous,
					next,
					lines,
					groupStart,
					groupEnd,
					block,
					result
				);
				groupStart = groupEnd;
			}

			return result.ToArray();
		}

		private void ProcessLinearGroup(
			LinearViewObject linearView,
			LinearViewObject? previous,
			LinearViewObject? next,
			LinearDisassemblyLine[] lines,
			int start,
			int end,
			BasicBlock? block,
			List<LinearDisassemblyLine> result
		)
		{
			if (null == block)
			{
				LinearDisassemblyLine[] misc = RenderLayer.CopyRange(lines, start, end);
				result.AddRange(
					this.ApplyToMiscLinearLines(linearView, previous, next, misc)
				);
				return;
			}

			int runStart = start;
			while (runStart < end)
			{
				bool isCode = LinearDisassemblyLineType.CodeDisassemblyLineType
					== lines[runStart].Type;
				int runEnd = runStart + 1;
				while (
					runEnd < end
					&& isCode
						== (LinearDisassemblyLineType.CodeDisassemblyLineType
							== lines[runEnd].Type)
				)
				{
					runEnd++;
				}

				if (isCode)
				{
					this.ProcessCodeRun(lines, runStart, runEnd, block, result);
				}
				else
				{
					LinearDisassemblyLine[] misc =
						RenderLayer.CopyRange(lines, runStart, runEnd);
					result.AddRange(
						this.ApplyToMiscLinearLines(
							linearView,
							previous,
							next,
							misc
						)
					);
				}

				runStart = runEnd;
			}
		}

		private void ProcessCodeRun(
			LinearDisassemblyLine[] lines,
			int start,
			int end,
			BasicBlock block,
			List<LinearDisassemblyLine> result
		)
		{
			DisassemblyTextLine[] contents = new DisassemblyTextLine[end - start];
			for (int i = start; i < end; i++)
			{
				contents[i - start] = lines[i].Contents;
			}

			DisassemblyTextLine[] transformed = this.ApplyToBlock(block, contents);
			Function? function = lines[start].Function;
			foreach (DisassemblyTextLine line in transformed)
			{
				result.Add(
					new LinearDisassemblyLine(
						LinearDisassemblyLineType.CodeDisassemblyLineType,
						function,
						block,
						line
					)
				);
			}
		}

		private static bool IsHighLevelBody(string name)
		{
			return "HLIL Function Body" == name
				|| "HLIL SSA Function Body" == name
				|| "Language Representation Function Body" == name;
		}

		private static bool SameBlock(BasicBlock? first, BasicBlock? second)
		{
			if (null == first)
			{
				return null == second;
			}

			return first.Equals(second);
		}

		private static LinearDisassemblyLine[] CopyRange(
			LinearDisassemblyLine[] lines,
			int start,
			int end
		)
		{
			LinearDisassemblyLine[] result = new LinearDisassemblyLine[end - start];
			Array.Copy(lines, start, result, 0, result.Length);
			return result;
		}

		private static void DisposeDisassemblyLines(DisassemblyTextLine[] lines)
		{
			foreach (DisassemblyTextLine line in lines)
			{
				foreach (Tag tag in line.Tags)
				{
					tag.Dispose();
				}
			}
		}
	}
}
